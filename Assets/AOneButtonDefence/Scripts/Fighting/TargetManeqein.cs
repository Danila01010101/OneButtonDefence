using UnityEngine;

public class TargetManeqein : MonoBehaviour, IDamagable
{
    public bool IsAlive() => true;

    public Transform GetTransform() => transform;
    
    public void TakeDamage(IDamagable damagerTransform, int damage)
    {
        Debug.Log("Manequen attaked!");
    }

    public string GetName()
    {
        return gameObject.name;
    }
}