using DG.Tweening;
using UnityEngine;

public class BuildAnimation : MonoBehaviour
{
    [SerializeField] private float animationTime = 1f;
    [SerializeField] private Transform target;

    public void BuildingAnimation()
    {
        target.localPosition = new Vector3(target.localPosition.x, target.localPosition.y - 2, target.localPosition.z);
        target.DOLocalMove(new Vector3(target.localPosition.x, target.localPosition.y + 2, target.localPosition.z), animationTime).SetEase(Ease.Linear);
    }
    
}
