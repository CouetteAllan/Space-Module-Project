using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameStrategy", menuName = "Module/Strategy/Flame")]

public class FlameThrowerModuleScript : BaseOffensiveScript, IDamageSource
{
    public int AttackPerSecond = 6;
    public float FlameLenght = 15.0f;
    public float FlameDuration = 4.0f;

    private Transform _moduleTransform;

    private bool _isNotActive = true;
    private Coroutine _attackCoroutine;

    public Transform Transform => _moduleTransform;

    public float RecoilMultiplier { get; set; } = 0.8f;

    public override void Fire(bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success)
    {
        Debug.Log("is flaming more even more");
        Debug.Log(_isNotActive);
        if (_isNotActive == true)
        {
            Debug.Log("is flaming more even mooooooooooqgfksjre");
            _isNotActive = false;
            if (_attackCoroutine != null)
            {
                MonoBehaviourOnScene.Instance.StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
            _attackCoroutine = MonoBehaviourOnScene.Instance.StartCoroutine(Attack(projectilePositions[0]));
            success = true;
            Debug.Log("is flaming more");
        }
        else
            success = false;
    }

    private IEnumerator Attack(Transform attackPosition)
    {
        if (attackPosition == null)
            yield break;

        float startTime = Time.time;
        while (startTime + FlameDuration > startTime)
        {
            if (attackPosition == null)
                yield break;
            DealDamageToEnemy(attackPosition.position);
            Debug.Log("is flaming");
            yield return new WaitForSeconds(1.0f / AttackPerSecond);
        }
        _isNotActive = true;
    }

    private void DealDamageToEnemy(Vector2 position)
    {
        var enemies = Physics2D.BoxCastAll(position, Vector2.one * 3.5f, angle: 0, _moduleTransform.up, FlameLenght);
        foreach (var enemy in enemies)
        {
            if (enemy.transform.TryGetComponent(out IHittable hittable))
            {
                hittable.TryHit(this, (int)((_currentModuleStats.currentDamage / AttackPerSecond) * _statClass.GetStatValue(StatType.Damage)));
            }
        }
    }



    public override void Init(StatClass statClass, ModuleDatas datas, Transform moduleTransform, Module.CurrentModuleStats currentModuleStats)
    {
        base.Init(statClass, datas, moduleTransform, currentModuleStats);
        _moduleTransform = moduleTransform;
    }
}
