using UnityEngine;

public class TargetManeqein : MonoBehaviour, IDamagable
{
    public void TakeDamage(int damage)
    {
        Debug.Log("Manequen attaked!");
    }
}