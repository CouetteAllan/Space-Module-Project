using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneModuleScript : BaseOffensiveScript
{
    private Transform _moduleTransform;

    public DroneModuleScript(StatClass statClass, ModuleDatas datas, Module.CurrentModuleStats currentModuleStats, Transform moduleTransform) : base(statClass, datas, currentModuleStats)
    {
        _moduleTransform = moduleTransform;
    }

    public override void Fire(bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success)
    {
        success = true;
        foreach (Transform t in projectilePositions)
        {
            Vector3 position = firstProjectile ? t.position : t.position + UtilsClass.GetRandomDir() * Random.Range(0.1f, 0.6f);

            var projectile = Object.Instantiate(_datas.ProjectilePrefab, position, currentRotation, _moduleTransform).GetComponentInChildren<ProjectileScript>();
            float projectileDamage = _statClass.GetStatValue(StatType.Damage) * _currentModuleStats.currentDamage;
            projectile.Launch((t.position - currentModulePosition).normalized, 10.0f, 1.0f,projectileDamage,t);
        }
    }
}
