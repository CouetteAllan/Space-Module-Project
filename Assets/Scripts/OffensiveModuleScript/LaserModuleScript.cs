using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LaserModuleScript : BaseOffensiveScript ,IOffensiveModule, IDamageSource
{
    private Transform _moduleTransform;
    public LaserModuleScript(StatClass statClass, ModuleDatas datas, Transform moduleTransform) : base(statClass, datas)
    {
        _moduleTransform = moduleTransform;
    }

    public Transform Transform => _moduleTransform;

    public void Fire(bool firstProjectile, Quaternion currentModuleRotation, Vector3 currentModulePosition, Transform[] projectilePositions)
    {
        foreach (Transform t in projectilePositions)
        {
            Vector3 position = firstProjectile ? t.position : t.position + UtilsClass.GetRandomDir() * Random.Range(0.1f, 0.6f);

            //Instantiate a physic cast in a straight line.
            var laser = Physics2D.BoxCastAll(position + Vector3.left, position + Vector3.up * 1 + Vector3.right, 0, Vector2.up);
            foreach(var l in laser)
            {
                if (l.transform.gameObject.TryGetComponent<IHittable>(out IHittable enemy))
                    enemy.TryHit(this, (int)_statClass.GetStatValue(StatType.Damage));

            }

        }
    }
}
