using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "Laser Strategy", menuName = "Module/Strategy/Laser")]
public class LaserModuleScript : BaseOffensiveScript, IDamageSource
{
    public float LaserLenght = 17.0f;
    private Transform _moduleTransform;
    public Transform Transform => _moduleTransform;

    public float RecoilMultiplier { get; set; } = 2.0f;

    public override void Fire(Module module, bool firstProjectile, Quaternion currentModuleRotation, Vector3 currentModulePosition, Transform[] projectilePositions, ref bool success)
    {
        success = true;
        foreach (Transform projectileTransform in projectilePositions)
        {
            Vector3 position = firstProjectile ? projectileTransform.position : projectileTransform.position + UtilsClass.GetRandomDir() * Random.Range(0.1f, 0.6f);

            //Instantiate a physic cast in a straight line.
            float hitboxWidth = 3.5f;
            var laser = Physics2D.BoxCastAll(position, Vector2.one * hitboxWidth, angle: 0, projectileTransform.up, LaserLenght);
            foreach (var hittable in laser)
            {
                if (hittable.transform.gameObject.TryGetComponent(out IHittable enemy))
                {
                    int damageToDeal = (int)(_statClass.GetStatValue(StatType.Damage) * _currentModuleStats.currentDamage);
                    enemy.TryHit(this, damageToDeal);
                }
            }
        }
    }

    public override void Init(StatClass statClass, ModuleDatas datas, Transform moduleTransform, Module.CurrentModuleStats currentModuleStats)
    {
        base.Init(statClass, datas, moduleTransform, currentModuleStats);
        _moduleTransform = moduleTransform;
    }

}
