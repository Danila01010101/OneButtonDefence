using DG.Tweening;
using UnityEngine;

public class BuildAnimation : MonoBehaviour
{
    [SerializeField] private float animationTime = 1f;
    public void StartAnimation() => BuildingAnimation();

    private void BuildingAnimation()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 2, gameObject.transform.position.z);
        transform.DOMove(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z), animationTime).SetEase(Ease.Linear);
    }
    
}
