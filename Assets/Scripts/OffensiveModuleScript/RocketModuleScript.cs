using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RocketStrategy", menuName = "Module/Strategy/Rocket")]
public class RocketModuleScript : BaseOffensiveScript
{
    public float RocketMissileSpeed = 3.0f;
    public float RocketDuration = 1.0f;


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
                speed = RocketMissileSpeed,
                duration = RocketDuration,
                damage = projectileDamage,
            });

        }
    }
}
