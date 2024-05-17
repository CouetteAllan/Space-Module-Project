using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShockwaveStrategy",menuName = "Module/Strategy/ShockwaveCanon")]
public class ShockWaveModuleScript : BaseOffensiveScript
{
    public float ShockwaveSpeed = 16.0f;
    public float ShockwaveLifetime = 2.0f;
    public override void Fire(Module module, bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, ref bool success)
    {
        success = true;
        foreach (Transform t in projectilePositions)
        {
            Vector3 position = firstProjectile ? t.position : t.position + UtilsClass.GetRandomDir() * Random.Range(0.1f, 0.6f);

            var projectile = Object.Instantiate(_datas.ProjectilePrefab, position, currentRotation).GetComponent<ProjectileScript>();
            float projectileDamage = _statClass.GetStatValue(StatType.Damage) * _currentModuleStats.currentDamage;
            projectile.Launch(new ProjectileScript.ProjectileParameter
            {
                dir = (t.position - currentModulePosition).normalized,
                damage = projectileDamage,
                speed = ShockwaveSpeed,
                duration = ShockwaveLifetime,
            });

        }
    }
}
