using System;
using AOneButtonDefence.Scripts.Data;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, IDamagable
{
    [SerializeField] private PlayerHealthbar healthbar;
    
    private PlayerControllerData data;
    private Transform cameraTransform;
    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Health health;
    private CharacterStatsCounter stats;
    private EffectReceiver effectReceiver;
    private PlayerEffectsHandler effectsHandler;
    private CapsuleCollider capsuleCollider;
    
    private readonly Vector3 gravityDirection = Vector3.down;

    public static Action<Transform> CharacterEnabled;
    public static Action CharacterDisabled;
    public static Action PlayerDead;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        if (cameraTransform == null) Debug.Log("No camera have been assigned for character movement, used main camera.");
        cameraTransform = cameraTransform == null ? Camera.main.transform : cameraTransform;
    }

    public void BindSpellStat(SpellCanvasInitializer spellCanvasInitializer)
    {
        spellCanvasInitializer.SetSpellStat(stats.GetStat<ICharacterStat>(ResourceData.ResourceType.SpellsStrength));
    }

    public void Initialize(PlayerControllerData data, Camera camera)
    {
        this.data = data;
        cameraTransform = camera.transform;
        InitializeStats();
        health = stats.GetStat<Health>(ResourceData.ResourceType.WarriorHealth);
        
        effectsHandler = gameObject.AddComponent<PlayerEffectsHandler>();
        effectsHandler.Initialize(stats);

        effectReceiver = new EffectReceiver(effectsHandler, transform, stats);
        
        healthbar.Initialize(health, camera);
        health.Death += NotifyOfPlayerDeath;
    }

    private void InitializeStats()
    {
        stats = new CharacterStatsCounter();
        stats.AddStat(ResourceData.ResourceType.WarriorHealth, new Health(data.StartHealth));
        stats.AddStat(ResourceData.ResourceType.SpellsStrength, new DefaultStat(0));
    }

    public bool IsAlive() => health.Value > 0;

    public void TakeDamage(IDamagable damagerTransform, float damage) => health.TakeDamage(damagerTransform.GetTransform(), damage);
    
    public Transform GetTransform() => transform;
    
    public string GetName()
    {
        return gameObject.name;
    }

    private void NotifyOfPlayerDeath()
    {
        PlayerDead?.Invoke();
        Disable();
        Destroy(gameObject, 0.1f);
        Debug.Log("Player died");
    }

    public void Enable()
    {
        if (controller != null) controller.enabled = true;
        if (playerInput != null) playerInput.enabled = true;
        health.Heal(100000);
        enabled = true;
        gameObject.SetActive(true);
        CharacterEnabled?.Invoke(transform);
    }

    public void Disable()
    {
        if (controller != null) controller.enabled = false;
        if (playerInput != null) playerInput.enabled = false;
        enabled = false;
        gameObject.SetActive(false);
        CharacterDisabled?.Invoke();
    }

    private void OnDestroy()
    {
        health.Death -= NotifyOfPlayerDeath;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (!enabled || !playerInput.enabled) return;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, data.RotationSpeed * Time.deltaTime);
        }

        controller.Move(moveDirection.normalized * (data.MoveSpeed * Time.deltaTime));
        controller.SimpleMove(gravityDirection * (Physics.gravity.y * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other) => effectReceiver.OnTriggerEnter(other);

    private void OnTriggerExit(Collider other) => effectReceiver.OnTriggerExit(other);

    private void OnEnable()
    {
        playerInput.ActivateInput();
        var moveAction = playerInput.actions.FindAction("Move");
        
        if (moveAction != null)
        {
            moveAction.performed += OnMove;
            moveAction.canceled += OnMove;
        }
        else
        {
            Debug.LogError("Move action not found in Input Actions!");
        }
    }

    private void OnDisable()
    {
        effectsHandler.ResetAllEffects();
        playerInput.DeactivateInput();
        var moveAction = playerInput.actions.FindAction("Move");
        
        if (moveAction != null)
        {
            moveAction.performed -= OnMove;
            moveAction.canceled -= OnMove;
        }
    }
}