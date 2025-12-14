using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeResourceInfoWindow : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform window;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform resourceLinesContainer;

    [Header("Prefabs")]
    [SerializeField] private TradeResourceLabel resourceLinePrefab;

    [Header("Animation")]
    [SerializeField] private float distanceBetweenLines = 40;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float moveDistance = 120f;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float lifeTime = 1.5f;

    public void Animate(List<StartResourceAmount> resourceAmounts)
    {
        foreach (Transform child in resourceLinesContainer)
            Destroy(child.gameObject);

        int count = 0;

        foreach (var r in resourceAmounts)
        {
            count++;
            
            var line = Instantiate(resourceLinePrefab, resourceLinesContainer);
            RectTransform rect = line.GetComponent<RectTransform>();

            rect.anchoredPosition = Vector2.up * (distanceBetweenLines * count);
            line.Set(r);
        }

        resourceLinesContainer.anchoredPosition += Vector2.up * (distanceBetweenLines * (count - 1) / 2f);

        window.sizeDelta = new Vector2(
            window.sizeDelta.x,
            window.sizeDelta.y + distanceBetweenLines * count
        );

        StartCoroutine(AnimateWindow());
    }

    private IEnumerator AnimateWindow()
    {
        Vector2 startPos = window.anchoredPosition;
        Vector2 endPos = startPos - new Vector2(0, moveDistance);
        canvasGroup.alpha = 0f;

        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(lifeTime);

        t = 0;
        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float k = t / moveDuration;

            window.anchoredPosition = Vector2.Lerp(startPos, endPos, k);
            canvasGroup.alpha = Mathf.Lerp(1, 0, k);

            yield return null;
        }
    }
}