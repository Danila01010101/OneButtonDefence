using System;
using UnityEngine;

public interface ISelfDamageable
{
    IDamagable GetSelfDamagable();
    event Action<IDamagable> DamageRecieved;
}