using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingAnimation : MonoBehaviour
{
    [SerializeField] private float jumpPower = 0.2f;
    [SerializeField] private Vector3 endValue = new Vector3 (0,0,0);
    [SerializeField] private int numJumps = 1;
    [SerializeField] private float duration = 0.7f;

    public bool IsWalk = false;

    private GameObject model;

    private void Start()
    {
        model = gameObject.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if(model.transform.localPosition == endValue && IsWalk == true)
            going();

    }
    private void going()
    {
        model.transform.DOLocalJump(endValue, jumpPower, numJumps, duration)
            .SetEase(Ease.OutQuad);
    }
}
