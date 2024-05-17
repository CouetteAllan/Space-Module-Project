using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeStrategy", menuName = "Module/Strategy/Melee")]
public class MeleeModuleScript : BaseOffensiveScript, IDamageSource
{
    public int AttackPerSecond = 6;
    public float BaseRadius = 2.5f;
    public float UpgradedRadius = 3.6f;


    private Transform _moduleTransform;
    private Coroutine _attackCoroutine;
    private Transform _attackpointTransform;
    private bool _isActive = false;


    public Transform Transform => _moduleTransform;

    public float RecoilMultiplier { get; set; } = 0.9f;

    public override void Init(StatClass statClass, ModuleDatas datas, Transform moduleTransform, Module.CurrentModuleStats currentModuleStats)
    {
        base.Init(statClass, datas, moduleTransform, currentModuleStats);
        _moduleTransform = moduleTransform;
    }

    public override void Fire(Module module, bool firstProjectile, Quaternion currentModuleRotation, Vector3 currentModulePosition, Transform[] projectilePositions, ref bool success)
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
            yield return new WaitForSeconds(1.0f / AttackPerSecond);
        }
    }

    private void DealDamageToEnemy(Vector2 position)
    {
        float radius = _currentModuleStats.currentLevel > 1 ? UpgradedRadius : BaseRadius;
        var enemies = Physics2D.OverlapCircleAll(position, radius);
        foreach (var enemy in enemies)
        {
            if(enemy.TryGetComponent(out IHittable hittable))
            {
                hittable.TryHit(this, (int)((_currentModuleStats.currentDamage / AttackPerSecond) * _statClass.GetStatValue(StatType.Damage)));
            }


        }
    }
}
