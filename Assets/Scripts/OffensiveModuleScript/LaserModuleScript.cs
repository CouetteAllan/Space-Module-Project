using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
public class LaserModuleScript : BaseOffensiveScript ,IOffensiveModule, IDamageSource
{
    private Transform _moduleTransform;
    public LaserModuleScript(StatClass statClass, ModuleDatas datas, float baseDamage , Transform moduleTransform) : base(statClass, datas, baseDamage)
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
            var laser = Physics2D.BoxCastAll(position, Vector2.one, 0, t.up, 9.0f);
            foreach(var l in laser) 
            {
                if (l.transform.gameObject.TryGetComponent<IHittable>(out IHittable enemy))
                    enemy.TryHit(this, (int)(_statClass.GetStatValue(StatType.Damage) * _baseDamage));

            }


        }
    }

}
