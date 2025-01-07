using DG.Tweening;
using UnityEngine;

public class FightAnimation : MonoBehaviour
{
    private GameObject childObject;
    private float rotateTime = 0.25f;
    private void Start()
    {
        childObject = transform.GetChild(0).gameObject;
    }
    public void Attack() 
    {
        RotateAttack();
    }

    private void RotateAttack() 
    {
        childObject.transform.DOLocalRotate(new Vector3(45, 45, 0), rotateTime)
            .SetEase(Ease.OutQuad)
            .OnComplete(()=>ResetRotate());
    }

    private void ResetRotate()
    {
        childObject.transform.DOLocalRotate(new Vector3(0, 0, 0), rotateTime)
            .SetEase(Ease.OutQuad);
    }
}
