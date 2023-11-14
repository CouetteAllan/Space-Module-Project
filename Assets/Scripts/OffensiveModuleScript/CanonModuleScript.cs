using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonModuleScript : BaseOffensiveScript,IOffensiveModule
{
    public CanonModuleScript(StatClass statClass, ModuleDatas datas) : base(statClass, datas) { }

    public void Fire(bool firstProjectile, Quaternion currentModuleRotation, Vector3 currentModulePosition, Transform[] projectilePositions)
    {
        foreach (Transform t in projectilePositions)
        {
            Vector3 position = firstProjectile ? t.position : t.position + UtilsClass.GetRandomDir() * Random.Range(0.1f, 0.6f);

            var projectile = Object.Instantiate(_datas.ProjectilePrefab, position, currentModuleRotation).GetComponent<ProjectileScript>();
            float projectileDamage = _statClass.GetStatValue(StatType.Damage);
            projectile.Launch((t.position - currentModulePosition).normalized, 6.0f, projectileDamage);

        }
    }
}
