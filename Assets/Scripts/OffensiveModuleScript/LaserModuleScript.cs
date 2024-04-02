using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
public class LaserModuleScript : BaseOffensiveScript , IDamageSource
{
    private Transform _moduleTransform;
    public LaserModuleScript(StatClass statClass, ModuleDatas datas, Module.CurrentModuleStats currentModuleStats, Transform moduleTransform) : base(statClass, datas, currentModuleStats)
    {
        _moduleTransform = moduleTransform;
    }

    public Transform Transform => _moduleTransform;

    public float RecoilMultiplier => 2.0f;

    public override void Fire(bool firstProjectile, Quaternion currentModuleRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success)
    {
        success = true;
        MonoBehaviourOnScene.Instance.StartCoroutine(FXCoroutine());
        foreach (Transform t in projectilePositions)
        {
            Vector3 position = firstProjectile ? t.position : t.position + UtilsClass.GetRandomDir() * Random.Range(0.1f, 0.6f);

            //Instantiate a physic cast in a straight line.
            float hitboxWidth = 3.5f;
            float hitBoxLength = 17f;
            var laser = Physics2D.BoxCastAll(position, Vector2.one * hitboxWidth, 0, t.up, hitBoxLength);
            foreach (var l in laser)
            {
                if (l.transform.gameObject.TryGetComponent(out IHittable enemy))
                {
                    int damageToDeal = (int)(_statClass.GetStatValue(StatType.Damage) * _currentModuleStats.currentDamage);
                    enemy.TryHit(this, damageToDeal);
                }
            }
        }
    }

    IEnumerator FXCoroutine()
    {
        float time = 0;
        float targetTime = 0.15f;
        while(time < targetTime)
        {


            yield return null;
            time += Time.deltaTime;
        }
        yield return null;
    }

}
