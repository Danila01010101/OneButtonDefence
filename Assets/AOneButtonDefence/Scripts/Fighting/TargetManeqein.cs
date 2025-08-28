using UnityEngine;

public class TargetManeqein : MonoBehaviour, IDamagable
{
    public bool IsAlive() => true;

    public void TakeDamage(int damage)
    {
        Debug.Log("Manequen attaked!");
    }

    public string GetName()
    {
        return gameObject.name;
    }
}