using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    public void TryHit(IDamageSource source,int damage);
}

public interface IDamageSource
{
    Transform Transform { get; }
}
