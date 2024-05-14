using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DroneStrategy", menuName = "Module/Strategy/Drone")]
public class DroneModuleScript : BaseOffensiveScript
{

    public float DroneSpeed = 10.0f;
    public float DroneDuration = 5.0f;

    private Transform _moduleTransform;


    public override void Fire(bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success)
    {
        success = true;
        foreach (Transform t in projectilePositions)
        {
            Vector3 position = firstProjectile ? t.position : t.position + UtilsClass.GetRandomDir() * Random.Range(0.1f, 0.6f);

            var projectile = Object.Instantiate(_datas.ProjectilePrefab, position, currentRotation, _moduleTransform).GetComponentInChildren<ProjectileScript>();
            float projectileDamage = _statClass.GetStatValue(StatType.Damage) * _currentModuleStats.currentDamage;
            projectile.Launch(new ProjectileScript.ProjectileParameter
            {
                dir = (t.position - currentModulePosition).normalized,
                speed = DroneSpeed,
                duration = DroneDuration,
                damage = projectileDamage,
                modTransform = t,
            });
        }
    }

    public override void Init(StatClass statClass, ModuleDatas datas, Transform moduleTransform, Module.CurrentModuleStats currentModuleStats)
    {
        base.Init(statClass, datas, moduleTransform, currentModuleStats);
        _moduleTransform = moduleTransform;
    }
}
