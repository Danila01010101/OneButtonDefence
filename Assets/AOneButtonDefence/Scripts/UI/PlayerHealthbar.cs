using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image flashOverlay;
    [SerializeField] private BillboardCanvas billboardCanvas;
    [SerializeField] private float minFill = 0.1f;
    [SerializeField] private float maxFill = 0.98f;
    [SerializeField] private float scaleFactor = 1f;
    [SerializeField] private float smoothDuration = 0.25f;
    [SerializeField] private float flashDuration = 0.15f;
    [SerializeField] private int flashCount = 2;

    private Camera playerHealthBarCamera;
    private Health health;
    private Coroutine fillCoroutine;
    private bool destroyed;

    public void Initialize(Health health, Camera camera = null)
    {
        playerHealthBarCamera = camera != null ? camera : Camera.main;

        if (camera != null)
            billboardCanvas.SetCamera(camera);

        this.health = health;
        SetHealthImmediate(health.Value, health.Value);

        health.HealthChanged += SetHealth;

        if (flashOverlay != null)
        {
            var c = flashOverlay.color;
            flashOverlay.color = new Color(c.r, c.g, c.b, 0f);
        }
    }

    private void LateUpdate()
    {
        if (playerHealthBarCamera == null) return;

        float distance = Vector3.Distance(transform.position, playerHealthBarCamera.transform.position);
        transform.localScale = Vector3.one * distance * scaleFactor;
    }

    private void SetHealthImmediate(float current, float max)
    {
        if (max <= 0) return;

        float normalized = Mathf.Clamp01(current / max);
        float fill = Mathf.Lerp(minFill, maxFill, normalized);
        healthBar.fillAmount = fill;
    }

    private void SetHealth(float current, float max)
    {
        if (destroyed) return;  
        if (!gameObject.activeInHierarchy) return;

        float normalized = Mathf.Clamp01(current / max);
        float targetFill = Mathf.Lerp(minFill, maxFill, normalized);

        if (fillCoroutine != null)
        {
            CoroutineStarter.Instance.StopCoroutine(fillCoroutine);
        }

        fillCoroutine = CoroutineStarter.Instance.StartCoroutine(FlashThenFill(targetFill));
    }

    private IEnumerator FlashThenFill(float targetFill)
    {
        while (gameObject != null && !gameObject.activeInHierarchy)
            yield return null;

        if (destroyed) yield break;

        if (flashOverlay != null)
        {
            for (int i = 0; i < flashCount; i++)
            {
                float half = flashDuration * 0.5f;
                float t = 0f;

                while (t < half)
                {
                    if (destroyed) yield break;
                    t += Time.deltaTime;
                    flashOverlay.color = new Color(1, 1, 1, t / half);
                    yield return null;
                }

                t = 0f;
                while (t < half)
                {
                    if (destroyed) yield break;
                    t += Time.deltaTime;
                    flashOverlay.color = new Color(1, 1, 1, 1 - t / half);
                    yield return null;
                }
            }

            flashOverlay.color = new Color(1, 1, 1, 0);
        }

        float start = healthBar.fillAmount;
        float timer = 0f;

        while (timer < smoothDuration)
        {
            if (destroyed) yield break;
            timer += Time.deltaTime;
            healthBar.fillAmount = Mathf.Lerp(start, targetFill, timer / smoothDuration);
            yield return null;
        }

        healthBar.fillAmount = targetFill;
        fillCoroutine = null;
    }

    private void OnDestroy()
    {
        destroyed = true;

        if (health != null)
            health.HealthChanged -= SetHealth;

        if (fillCoroutine != null && CoroutineStarter.Instance != null)
            CoroutineStarter.Instance.StopCoroutine(fillCoroutine);
    }
    
    private void OnEnable()
    {
        if (health != null)
            SetHealthImmediate(health.Value, health.Value);
    }
}