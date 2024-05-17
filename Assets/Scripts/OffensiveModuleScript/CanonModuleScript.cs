using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CanonStrategy", menuName = "Module/Strategy/SimpleCanon")]
public class CanonModuleScript : BaseOffensiveScript
{
    public float BulletSpeed = 12.0f;
    public float BulletLifeTime = 5.0f;
    public override void Fire(Module module, bool firstProjectile, Quaternion currentModuleRotation, Vector3 currentModulePosition, Transform[] projectilePositions, ref bool success)
    {
        success = true;
        foreach (Transform t in projectilePositions)
        {
            Vector3 position = firstProjectile ? t.position : t.position + UtilsClass.GetRandomDir() * Random.Range(0.1f, 0.6f);

            var projectile = Object.Instantiate(_datas.ProjectilePrefab, position, currentModuleRotation).GetComponent<ProjectileScript>();
            float projectileDamage = _statClass.GetStatValue(StatType.Damage) * _currentModuleStats.currentDamage;
            projectile.Launch( new ProjectileScript.ProjectileParameter
            {
                dir = (t.position - currentModulePosition).normalized,
                damage = projectileDamage,
                speed = BulletSpeed,
                duration = BulletLifeTime,
            });

        }
    }
}
