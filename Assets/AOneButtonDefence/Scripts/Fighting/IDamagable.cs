using UnityEngine;

public interface IDamagable
{
    bool IsAlive();
    void TakeDamage(IDamagable damagerTransform, int damage);
    Transform GetTransform();
    string GetName();
}