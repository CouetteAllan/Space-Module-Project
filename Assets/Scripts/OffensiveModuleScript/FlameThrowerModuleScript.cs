using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameStrategy", menuName = "Module/Strategy/Flame")]

public class FlameThrowerModuleScript : BaseOffensiveScript
{
    public int AttackPerSecond = 6;
    public float FlameLenght = 15.0f;
    public float FlameDuration = 4.0f;


    public float RecoilMultiplier { get; set; } = 0.8f;

    public override void Fire(Module module, bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, ref bool isActive)
    {
        if (!isActive)
        {
            MonoBehaviourOnScene.Instance.StartCoroutine(Attack(projectilePositions[0],module, new BooleanHolder(ref isActive)));
            isActive = true;
        }
    }

    private IEnumerator Attack(Transform attackPosition, IDamageSource damageSource, BooleanHolder boolean)
    {
        if (attackPosition == null)
            yield break;

        float startTime = Time.time;
        while (startTime + FlameDuration > startTime)
        {
            if (attackPosition == null)
                yield break;
            DealDamageToEnemy(attackPosition,damageSource);
            yield return new WaitForSeconds(1.0f / AttackPerSecond);
        }
        boolean.ChangeBoolean(false);
    }

    private void DealDamageToEnemy(Transform transform,IDamageSource source)
    {
        var enemies = Physics2D.BoxCastAll(transform.position, Vector2.one * 3.5f, angle: 0, transform.up, FlameLenght);
        foreach (var enemy in enemies)
        {
            if (enemy.transform.TryGetComponent(out IHittable hittable))
            {
                hittable.TryHit(source, (int)((_currentModuleStats.currentDamage / AttackPerSecond) * _statClass.GetStatValue(StatType.Damage)));
            }
        }
    }

}
