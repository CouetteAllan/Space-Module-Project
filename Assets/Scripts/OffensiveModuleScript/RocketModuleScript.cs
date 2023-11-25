using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketModuleScript : BaseOffensiveScript
{
    public RocketModuleScript(StatClass statClass, ModuleDatas datas, float baseDamage) : base(statClass, datas, baseDamage)
    {
    }

    public override void Fire(bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success)
    {
        success = true;
        foreach (Transform t in projectilePositions)
        {
            Vector3 position = firstProjectile ? t.position : t.position + UtilsClass.GetRandomDir() * Random.Range(0.1f, 0.6f);

            var projectile = Object.Instantiate(_datas.ProjectilePrefab, position, currentRotation).GetComponent<ProjectileScript>();
            float projectileDamage = _statClass.GetStatValue(StatType.Damage) * _baseDamage;
            projectile.Launch((t.position - currentModulePosition).normalized, 4.0f, projectileDamage);

        }
    }
}
