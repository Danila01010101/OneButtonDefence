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

    public void Initialize(Health health, Camera camera = null)
    {
        if (camera != null)
        {
            this.playerHealthBarCamera = camera;
            billboardCanvas.SetCamera(camera);
        }
        else
        {
            this.playerHealthBarCamera = Camera.main;
        }

        this.health = health;
        SetHealthImmediate(health.Amount, health.Amount);
        health.HealthChanged += SetHealth;

        if (flashOverlay != null)
            flashOverlay.color = new Color(1f, 1f, 1f, 0f);
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
        healthBar.fillAmount = Mathf.Clamp(fill, Mathf.Min(minFill, maxFill), Mathf.Max(minFill, maxFill));
    }

    private void SetHealth(float current, float max)
    {
        if (max <= 0) return;

        float normalized = Mathf.Clamp01(current / max);
        float targetFill = Mathf.Lerp(minFill, maxFill, normalized);

        if (fillCoroutine != null) StopCoroutine(fillCoroutine);
        fillCoroutine = StartCoroutine(FlashThenFill(targetFill));
    }

    private IEnumerator FlashThenFill(float targetFill)
    {
        if (flashOverlay != null)
        {
            for (int i = 0; i < flashCount; i++)
            {
                float time = 0f;
                while (time < flashDuration * 0.5f)
                {
                    time += Time.deltaTime;
                    float alpha = Mathf.Lerp(0f, 1f, time / (flashDuration * 0.5f));
                    flashOverlay.color = new Color(1f, 1f, 1f, alpha);
                    yield return null;
                }

                time = 0f;
                while (time < flashDuration * 0.5f)
                {
                    time += Time.deltaTime;
                    float alpha = Mathf.Lerp(1f, 0f, time / (flashDuration * 0.5f));
                    flashOverlay.color = new Color(1f, 1f, 1f, alpha);
                    yield return null;
                }
            }

            flashOverlay.color = new Color(1f, 1f, 1f, 0f);
        }

        float start = healthBar.fillAmount;
        float timeFill = 0f;

        while (timeFill < smoothDuration)
        {
            timeFill += Time.deltaTime;
            float t = timeFill / smoothDuration;
            healthBar.fillAmount = Mathf.Lerp(start, targetFill, t);
            yield return null;
        }

        flashOverlay.fillAmount = targetFill;
        healthBar.fillAmount = targetFill;
        fillCoroutine = null;
    }

    private void OnDestroy()
    {
        if (health != null)
            health.HealthChanged -= SetHealth;
    }
}