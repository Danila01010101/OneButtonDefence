using DG.Tweening;
using UnityEngine;

public class WalkingAnimation : MonoBehaviour
{
    [SerializeField] protected float jumpPower = 0.2f;
    [SerializeField] protected Vector3 endValue = new Vector3 (0,0,0);
    [SerializeField] protected int numJumps = 1;
    [SerializeField] protected float duration = 0.7f;

    public bool IsWalk = false;

    private GameObject model;

    private void Start()
    {
        model = gameObject.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if(model.transform.localPosition == endValue && IsWalk == true)
            Animate();
    }
    
    public void StartAnimation() => IsWalk = true; 
    
    public void StopAnimation() => IsWalk = false;
    
    protected virtual void Animate()
    {
        model.transform.DOLocalJump(endValue, jumpPower, numJumps, duration)
            .SetEase(Ease.OutQuad);
    }
}
