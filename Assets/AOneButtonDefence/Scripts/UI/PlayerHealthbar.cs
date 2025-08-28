using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private BillboardCanvas billboardCanvas;
    [SerializeField] private int healthIconsAmount = 10;
    [SerializeField] private float minFill = 0.1f;
    [SerializeField] private float maxFill = 0.98f;

    private Health health;

    public void Initialize(Health health, Camera camera = null)
    {
        if (camera != null)
            billboardCanvas.SetCamera(camera);

        this.health = health;
        SetHealth(health.Amount, health.Amount);
        health.AmountChanged += SetHealth;
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
            health.AmountChanged -= SetHealth;
    }
}