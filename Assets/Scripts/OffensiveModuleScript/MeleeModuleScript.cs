using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeModuleScript : BaseOffensiveScript, IOffensiveModule, IDamageSource
{
    private Transform _moduleTransform;
    private Coroutine _attackCoroutine;
    private Transform _attackpointTransform;
    private int _attackPerSecond = 6;
    private bool _isActive = false;

    public MeleeModuleScript(StatClass statClass, ModuleDatas datas, float baseDamage, Transform moduleTransform) : base(statClass, datas, baseDamage)
    {
        _moduleTransform = moduleTransform;
    }

    public Transform Transform => _moduleTransform;

    public float RecoilMultiplier => .4f;

    public void Fire(bool firstProjectile, Quaternion currentModuleRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success)
    {
        if (!_isActive)
        {
            _isActive = true;
            if (_attackCoroutine != null)
            {
                MonoBehaviourOnScene.Instance.StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
            _attackCoroutine = MonoBehaviourOnScene.Instance.StartCoroutine(Attack(projectilePositions[0]));
            success = true;
        }
        else
            success = false;
       
    }

    private IEnumerator Attack(Transform attackPosition)
    {
        if (attackPosition == null)
            yield break;
        while (true)
        {
            if (attackPosition == null)
                yield break;
            DealDamageToEnemy(attackPosition.position);
            yield return new WaitForSeconds(1.0f / _attackPerSecond);
        }
    }

    private void DealDamageToEnemy(Vector2 attackPosition)
    {
        var enemies = Physics2D.OverlapCircleAll(attackPosition, 2.5f);
        foreach (var enemy in enemies)
        {
            if(enemy.TryGetComponent(out IHittable hittable))
            {
                hittable.TryHit(this, (int)((_baseDamage/ _attackPerSecond) * _statClass.GetStatValue(StatType.Damage)));
            }
        }
    }
}
