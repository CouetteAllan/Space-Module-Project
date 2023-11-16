using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Module : MonoBehaviour
{
    public event Action OnModuleFire;
    [SerializeField] private Transform[] _firePoints;
    [SerializeField] private PlayParticle _playParticle;

    public enum ModuleClass
    {
        Offense,
        Defense,
        StatBuff,
        Other,
        Placement
    }

    private ModuleClass _moduleClass;
    private ModuleDatas _data;
    private AttachPointScript _attachPoint;
    private StatClass _playerStatClass;
    private IOffensiveModule _offensiveStrategy;


    public static Module CreateMod(Vector2 position, ModuleDatas datas, Transform parentTransform)
    {
        GameObject go = new GameObject();
        Module module = Instantiate(datas.ModulePrefab.GetComponent<Module>(), position, parentTransform.rotation, parentTransform);
        module.SetUpModule(datas, parentTransform.GetComponent<AttachPointScript>());
        return module;
    }

    private void SetUpModule(ModuleDatas datas, AttachPointScript attachPointScript)
    {
        this.gameObject.name = datas.ModuleName;
        _data = datas;
        _moduleClass = datas.ModuleClass;
        _attachPoint = attachPointScript;
        _playerStatClass = StatSystem.Instance.PlayerStat;


        switch (_moduleClass)
        {
            case ModuleClass.Offense:
                TimeTickSystemDataHandler.OnTickFaster += TimeTickSystemDataHandler_OnTick;

                //_offensiveStrategy = new CanonModuleScript(_playerStatClass,_data); 
                _offensiveStrategy = _data.OffensiveModuleDatas.GetOffensiveStrategy(_playerStatClass, _data, this.transform); 
                break;
            case ModuleClass.Defense:
                break;
            case ModuleClass.StatBuff:

                ApplyBuff(datas.BuffDatas);

                break;
            case ModuleClass.Placement:
                var healthScript = this.gameObject.AddComponent<HealthScript>();
                healthScript.SetAttachPoint(attachPointScript);
                break;
        }

        _playParticle?.SetUpPlayParticle(this);

    }

    private void RemoveModule()
    {

    }

    private void TimeTickSystemDataHandler_OnTick(uint tick)
    {
        if (tick % GetTickNeeded() == 0)
        {
            for (int i = 0; i < _playerStatClass.GetStatValue(StatType.NbProjectile); i++)
            {
                _offensiveStrategy.Fire(i == 0,this.transform.rotation,this.transform.position,_firePoints);
                OnModuleFire?.Invoke();
            }
        }
    }


    private void OnDisable()
    {
        TimeTickSystemDataHandler.OnTick -= TimeTickSystemDataHandler_OnTick;

    }
    private void OnDestroy()
    {
        TimeTickSystemDataHandler.OnTick -= TimeTickSystemDataHandler_OnTick;
    }

    private int GetTickNeeded()
    {
        float n = (20 * _data.OffensiveModuleDatas.AttackSpeedMultiplier) / _playerStatClass.GetStatValue(StatType.ReloadSpeed);
        int i = Mathf.Clamp(Mathf.CeilToInt(n), 1, 20);
        return i;
    }

    private void ApplyBuff(BuffDatas datas)
    {
        SingleStat statApplied = datas.GetStat();
        switch (datas.Type)
        {
            case BuffDatas.BuffType.Add:
                _playerStatClass.ChangeStat(statApplied.Type, statApplied.BaseValue);

                break;
            case BuffDatas.BuffType.Multiply:
                _playerStatClass.MultiplyStat(statApplied.Type, statApplied.BaseValue);
                break;
            case BuffDatas.BuffType.PercentMultiply:
                _playerStatClass.MultiplyPercentStat(statApplied.Type, statApplied.BaseValue);
                break;
        }
    }

    private void SetOffensiveStrategy(IOffensiveModule offensiveStrategy)
    {
        _offensiveStrategy = offensiveStrategy;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var t in _firePoints)
        {
            Gizmos.DrawLine(t.position + (-t.right), t.position + t.up * 11 + (-t.right));
            Gizmos.DrawLine(t.position + t.right, t.position + t.up * 11 + t.right);
            Gizmos.DrawLine(t.position + t.up * 11 + (-t.right), t.position + t.up * 11 + t.right);
        }
        
    }
}
