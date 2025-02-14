using UnityEngine;
using DG.Tweening;

public class DropBounceEffect : MonoBehaviour
{
    public float bounceHeight = 2f;
    public int bounceCount = 3;
    public float bounceDuration = 1f;
    public float horizontalRange = 1f;
    public float rotationSpeed = 360f;
    public Ease bounceEase = Ease.OutQuad;

    private Vector3 startPosition;

    public void StartBounce(Vector3 startWorldPosition, System.Action onComplete)
    {
        startPosition = startWorldPosition;
        transform.position = startPosition;

        Vector3 randomHorizontalOffset = new Vector3(
            Random.Range(-horizontalRange, horizontalRange),
            0,
            Random.Range(-horizontalRange, horizontalRange)
        );

        Sequence bounceSequence = DOTween.Sequence();

        for (int i = 0; i < bounceCount; i++)
        {
            float height = bounceHeight / (i + 1);
            Vector3 jumpTarget = startPosition + randomHorizontalOffset / (3 - i);

            bounceSequence.Append(transform.DOJump(jumpTarget, height, 1, bounceDuration / bounceCount)
                .SetEase(bounceEase));
        }

        transform.DORotate(new Vector3(-90, 360, 0), bounceDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear).OnComplete(() => onComplete?.Invoke());
    }
}