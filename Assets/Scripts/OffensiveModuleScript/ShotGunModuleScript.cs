using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunStrategy", menuName = "Module/Strategy/Shotgun")]


public class ShotGunModuleScript : BaseOffensiveScript
{
    public float ShotgunMissileSpeed;
    public float ShotgunMissileDuration;

    public override void Fire(Module module, bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, ref bool success)
    {
        success = true;
        foreach (Transform t in projectilePositions)
        {
            Vector3 position = firstProjectile ? t.position : t.position + UtilsClass.GetRandomDir() * Random.Range(0.1f, 0.8f);

            var projectile = Object.Instantiate(_datas.ProjectilePrefab, position, currentRotation).GetComponent<ProjectileScript>();
            float projectileDamage = _statClass.GetStatValue(StatType.Damage) * _currentModuleStats.currentDamage;
            projectile.Launch(new ProjectileScript.ProjectileParameter
            {
                dir = (t.position - currentModulePosition).normalized,
                speed = ShotgunMissileSpeed,
                duration = ShotgunMissileDuration,
                damage = projectileDamage,
            });

        }
    }
}
