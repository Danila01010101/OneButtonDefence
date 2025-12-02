using UnityEngine;

public interface IDamagable
{
    bool IsAlive();
    void TakeDamage(IDamagable damagerTransform, float damage);
    Transform GetTransform();
    string GetName();
}