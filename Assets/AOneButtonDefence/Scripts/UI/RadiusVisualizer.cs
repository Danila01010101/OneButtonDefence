using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SphereCollider))]
public class RadiusVisualizer : MonoBehaviour
{
    [SerializeField] private Transform radiusObject;
    private SphereCollider sphereCollider;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        UpdateScale();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sphereCollider == null)
            sphereCollider = GetComponent<SphereCollider>();
        UpdateScale();
    }
#endif

    private void UpdateScale()
    {
        if (sphereCollider == null) return;

        float radius = sphereCollider.radius;
        radiusObject.localScale = new Vector3(radius, 1f, radius);
    }
}