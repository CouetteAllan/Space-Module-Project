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

public interface IObstacle : IHittable
{

}


public interface IOffensiveModule
{
    public void Fire(bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success);
}