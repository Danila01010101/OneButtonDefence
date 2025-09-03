using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private BillboardCanvas billboardCanvas;
    [SerializeField] private int healthIconsAmount = 10;
    [SerializeField] private float minFill = 0.1f;
    [SerializeField] private float maxFill = 0.98f;
    [SerializeField] private float scaleFactor = 1f;

    private Camera camera;
    private Health health;

    public void Initialize(Health health, Camera camera = null)
    {
        if (camera != null)
        {
            this.camera = camera;
            billboardCanvas.SetCamera(camera);
        }
        else
        {
            this.camera = Camera.main;
        }

        this.health = health;
        SetHealth(health.Amount, health.Amount);
        health.DamageReceived += SetHealth;
    }
    
    private void LateUpdate()
    {
        if (camera == null) return;

        float distance = Vector3.Distance(transform.position, camera.transform.position);
        transform.localScale = Vector3.one * distance * scaleFactor;
    }


    private void SetHealth(float current, float max)
    {
        if (max <= 0) return;

        float healthPerIcon = max / healthIconsAmount;
        int iconsFilled = Mathf.FloorToInt(current / healthPerIcon);
        float normalized = (float)iconsFilled / healthIconsAmount;
        float fill = Mathf.Lerp(minFill, maxFill, normalized);
        healthBar.fillAmount = fill;
    }

    private void OnDestroy()
    {
        if (health != null)
            health.DamageReceived -= SetHealth;
    }
}