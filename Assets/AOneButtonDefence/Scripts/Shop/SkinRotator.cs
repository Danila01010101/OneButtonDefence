using UnityEngine;

public class SkinRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private Transform targetTransform;

    private float currentRotationZ;
    private float targetRotationZ;
    private float rotationVelocity; 
    private IInput input;
    private bool isInitialized;

    public void Initialize(IInput input)
    {
        this.input = input;
        isInitialized = true;
    }

    private void Update()
    {
        if (targetTransform == null) return;

        currentRotationZ = Mathf.SmoothDamp(currentRotationZ, targetRotationZ, ref rotationVelocity, smoothTime);
        targetTransform.rotation = Quaternion.Euler(0, currentRotationZ, 0);
    }

    private void Rotate(Vector3 direction)
    {
        if (targetTransform == null) return;

        targetRotationZ += direction.x * rotationSpeed;
    }

    private void OnEnable()
    {
        if (isInitialized) 
            input.Moved += Rotate;
    }

    private void OnDisable()
    {
        if (isInitialized) 
            input.Moved -= Rotate;
    }
}
