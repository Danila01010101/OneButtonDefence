using System;
using AOneButtonDefence.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;

public class FightAnimation : MonoBehaviour, IAttackAnimator
{
    [SerializeField] private float rotateTime = 0.25f;
    
    private GameObject childObject;
    private Vector3 startRotation;

    public Action CharacterAttacked { get; set; }
    public Action CharacterAttackEnded { get; set; }
    
    private void Start()
    {
        childObject = transform.GetChild(0).gameObject;
        startRotation = childObject.transform.localEulerAngles;
    }
    
    public void StartAnimation() => RotateAttack();
    
    public void InterruptAnimation()
    {
        ResetRotate();
    }

    public Transform Transform
    {
        get => transform;
        set { }
    }

    private void RotateAttack()
    {
        childObject.transform.DOLocalRotate(startRotation + new Vector3(-45, -45, 0), rotateTime)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => ResetRotate());
    }

    private void ResetRotate()
    {
        CharacterAttacked?.Invoke();
        if (childObject != null)
        {
            childObject.transform.DOLocalRotate(startRotation, rotateTime)
                .SetEase(Ease.OutQuad).OnComplete(() => CharacterAttackEnded?.Invoke());
        }
    }
}