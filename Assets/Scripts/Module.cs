using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Module : MonoBehaviour
{
    public static event Action OnModuleDestroyed;

    [SerializeField] private Transform[] _firePoints;
    [SerializeField] private PlayParticle _playParticle;
    public event Action OnModuleFire;

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
    private SingleStat _singleStatApplied;

    public static Module CreateMod(Vector2 position, ModuleDatas datas, Transform parentTransform)
    {
        Module module = Instantiate(datas.ModulePrefab.GetComponent<Module>(), position, parentTransform.rotation, parentTransform);
        module.SetUpModule(datas, parentTransform.GetComponent<AttachPointScript>());
        return module;
    }
    
    public static Transform CreateModPreview(Vector2 position, ModuleDatas datas, Transform parentTransform)
    {
        Transform module = Instantiate(datas.ModulePrefab.transform.Find("Graph"), position + (Vector2)parentTransform.up * 0.5f, parentTransform.rotation , parentTransform);
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

        _playerStatClass.ChangeStat(StatType.Weight, (float)_data.Weight);

    }

    private void SetUpModulePreview(ModuleDatas datas, AttachPointScript attachPointScript)
    {
        this.gameObject.name = datas.ModuleName;
        _data = datas;
        _moduleClass = datas.ModuleClass;
        _attachPoint = attachPointScript;
        _playerStatClass = StatSystem.Instance.PlayerStat;
    }

    private void RemoveModule()
    {
        switch (this._moduleClass)
        {
            case ModuleClass.Offense:
                break;
            case ModuleClass.Defense:
                break;
            case ModuleClass.StatBuff:
                //remove the buff
                RemoveBuff();
                break;
            case ModuleClass.Placement:
                //Zoom in the camera
                GameManager.Instance.ZoomIn();
                break;
        }
        _playerStatClass.ChangeStat(StatType.Weight, -(int)_data.Weight);
        OnModuleDestroyed?.Invoke();
        PlayerModule.RemoveModule(this);
    }

    private void TimeTickSystemDataHandler_OnTick(uint tick)
    {
        if (tick % GetTickNeeded() == 0)
        {
            for (int i = 0; i < _playerStatClass.GetStatValue(StatType.NbProjectile); i++)
            {
                _offensiveStrategy.Fire(i == 0,this.transform.rotation,this.transform.position,_firePoints, out bool success);
                if (success)
                    OnModuleFire?.Invoke();

            }
        }
    }


    private void OnDisable()
    {
        TimeTickSystemDataHandler.OnTickFaster -= TimeTickSystemDataHandler_OnTick;

    }
    private void OnDestroy()
    {
        TimeTickSystemDataHandler.OnTickFaster -= TimeTickSystemDataHandler_OnTick;
        this.RemoveModule();
    }

    private int GetTickNeeded()
    {
        float maxTick = 20 * _data.OffensiveModuleDatas.AttackSpeedMultiplier;
        float n = maxTick / _playerStatClass.GetStatValue(StatType.ReloadSpeed);
        int i = Mathf.Clamp(Mathf.CeilToInt(n), 1, Mathf.CeilToInt(maxTick));
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
        _singleStatApplied = statApplied;
    }

    private void RemoveBuff()
    {
        switch(_data.BuffDatas.Type)
        {
            case BuffDatas.BuffType.PercentMultiply:
                _playerStatClass.RemovePercentStat(_singleStatApplied.Type, _singleStatApplied.BaseValue);
                break;
            default:
                _playerStatClass.ChangeStat(_singleStatApplied.Type, -_singleStatApplied.BaseValue);
                break;
        }
    }

    private void SetOffensiveStrategy(IOffensiveModule offensiveStrategy)
    {
        _offensiveStrategy = offensiveStrategy;
    }
    public ModuleClass GetModuleClass()
    {
        return _moduleClass;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var t in _firePoints)
        {
            Gizmos.color = Color.yellow;
            float hitboxWidth = 1.5f;
            Gizmos.DrawLine(t.position + (-t.right * hitboxWidth), t.position + t.up * 18 + (-t.right * hitboxWidth));
            Gizmos.DrawLine(t.position + t.right * hitboxWidth, t.position + t.up * 18 + t.right * hitboxWidth);
            Gizmos.DrawLine(t.position + t.up * 18 + (-t.right * hitboxWidth), t.position + t.up * 18 + t.right * hitboxWidth);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(t.position, 1.5f);
        }
        
    }
}
