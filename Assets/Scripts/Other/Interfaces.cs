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
    float RecoilMultiplier { get; }
}

public interface IProjectile
{
    public void Launch(Vector2 dir, float speed, float damage, float duration = 1.0f, Transform modTransform = null, int currentModuleLevel = 1);
}

public interface IObstacle : IHittable
{

}

public interface IGatherScrap
{
    public void GatherScrapMetal(int value);
}

public interface IStrategyModule
{
    public void Init(StatClass statClass, ModuleDatas datas, Transform moduleTransform, Module.CurrentModuleStats currentModuleStats);
    public void Fire(bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success);
}

public interface IEnemyBehaviour
{
    public void DoMovement(PlayerController player, bool isStopped = false);
    public void DoAttack(PlayerController player);
}

public interface IPickUpObject
{
    public void PickUpObject(BuffDatas buff);
}
