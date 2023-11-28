using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneModuleScript : BaseOffensiveScript
{
    private Transform _moduleTransform;
    private bool _doOnce = false;

    public DroneModuleScript(StatClass statClass, ModuleDatas datas, float baseDamage, Transform moduleTransform) : base(statClass, datas, baseDamage)
    {
        _moduleTransform = moduleTransform;
    }

    public override void Fire(bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success)
    {
        success = true;
        if (_doOnce)
            return;
        foreach (Transform t in projectilePositions)
        {
            Vector3 position = firstProjectile ? t.position : t.position + UtilsClass.GetRandomDir() * Random.Range(0.1f, 0.6f);

            var projectile = Object.Instantiate(_datas.ProjectilePrefab, position, currentRotation).GetComponent<ProjectileScript>();
            float projectileDamage = _statClass.GetStatValue(StatType.Damage) * _baseDamage;
            projectile.Launch((t.position - currentModulePosition).normalized, 10.0f, projectileDamage,_moduleTransform);
            _doOnce = true;

        }
    }
}
