using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Module : MonoBehaviour, IGatherScrap
{
    public static event Action<Module> OnModuleDestroyed;
    public static event Action<Module> OnModuleLevelUp;

    [SerializeField] private Transform[] _firePoints;
    [SerializeField] private PlayParticle _playParticle;
    [SerializeField] private string _audioClipName = null;
    public event Action OnModuleFire;

    public enum ModuleClass
    {
        Offense,
        Defense,
        StatBuff,
        Heal,
        Placement
    }

    public class CurrentModuleStats
    {
        public float currentReloadSpeedMultplier;
        public float currentDamage;
        public float currentProjectileNumber;
        public float currentWeight;
        public int currentLevel;

        public void IncreaseStats(int currentLevel)
        {
            this.currentLevel = currentLevel;
            currentReloadSpeedMultplier /= 2.0f;
            currentDamage += 5;

        }
    }

    #region Fields
    private CurrentModuleStats _currentModuleStats = null;

    private ModuleClass _moduleClass;
    private ModuleDatas _data;
    private AttachPointScript _attachPoint;
    private StatClass _playerStatClass;
    private IStrategyModule _moduleStrategy;
    private SingleStat _singleStatApplied;
    private HealthScript _healthScript;

    private OffensiveModuleDatas.OffensiveType _offensiveType = OffensiveModuleDatas.OffensiveType.None;
    public OffensiveModuleDatas.OffensiveType OffensiveType => _offensiveType;

    private int _currentLevel = 1;
    public int CurrentLevel => _currentLevel;
    private int _maxLevel = 2;
    public int MaxLevel => _maxLevel;
    #endregion

    public static Module CreateMod(Vector2 position, ModuleDatas datas, Transform parentTransform)
    {
        Module module = Instantiate(datas.ModulePrefab.GetComponent<Module>(), position, parentTransform.rotation, parentTransform);
        module.SetUpModule(datas, parentTransform.GetComponent<AttachPointScript>());
        return module;
    }
    
    public static Transform CreateModPreview(Vector2 position, ModuleDatas datas, Transform parentTransform)
    {
        Transform module = Instantiate(datas.ModulePrefab/*.transform.Find("Graph")*/,
            position + (Vector2)parentTransform.up * 0.5f,
            parentTransform.rotation ,
            parentTransform)
            .transform;
        return module;
    }

    

    private void SetUpModule(ModuleDatas datas, AttachPointScript attachPointScript)
    {
        this.gameObject.name = datas.ModuleName;
        _data = datas;
        _moduleClass = datas.ModuleClass;
        _attachPoint = attachPointScript;
        _playerStatClass = StatSystem.Instance.PlayerStat;

        _currentLevel = 1;

        switch (_moduleClass)
        {
            case ModuleClass.Offense:
                TimeTickSystemDataHandler.OnTickFaster += TimeTickSystemDataHandler_OnTick;
                _currentModuleStats = new CurrentModuleStats
                {
                    currentDamage = _data.OffensiveModuleDatas.BaseModuleDamage,
                    currentReloadSpeedMultplier = _data.OffensiveModuleDatas.AttackSpeedMultiplier,
                    currentWeight = (int)_data.Weight,
                };

                _moduleStrategy = _data.OffensiveModuleDatas.GetOffensiveStrategy(_playerStatClass, _data, this.transform,_currentModuleStats);
                _offensiveType = _data.OffensiveModuleDatas.Type;
                
                break;
            case ModuleClass.Heal:
                TimeTickSystemDataHandler.OnTickFaster += TimeTickSystemDataHandler_OnTick;
                _currentModuleStats = new CurrentModuleStats
                {
                    currentReloadSpeedMultplier = _data.SecondaryModuleDatas.TickMultiplier,
                    currentWeight = (int)_data.Weight,
                };

                _moduleStrategy = _data.SecondaryModuleDatas.GetStrategyModule(GameManager.Instance.PlayerController);
                break;
            case ModuleClass.StatBuff:
                ApplyBuff(datas.BuffDatas);
                break;
            case ModuleClass.Placement:
                _healthScript = this.gameObject.AddComponent<HealthScript>();
                _healthScript.SetHealthScript(attachPointScript, _data.Health);
                _healthScript.OnDeath += OnModuleBranchDestroyed;
                break;
                 
        }


        _playParticle?.SetUpPlayParticle(this);

        _playerStatClass.ChangeStat(StatType.Weight, (float)_data.Weight);

    }

    private void OnModuleBranchDestroyed()
    {
        RemoveModule();
        Destroy(gameObject);
        _healthScript.OnDeath -= OnModuleBranchDestroyed;
    }

    public bool TryLevelUpModule()
    {
        if (CurrentLevel >= _maxLevel)
            return false;
        
        OnModuleLevelUp?.Invoke(this);
        _currentLevel++;
        FXManager.Instance.PlayEffect("lvlup",this.transform.position,this.transform.rotation, this.transform);
        _currentModuleStats.IncreaseStats(_currentLevel);
        if (_currentLevel >= _maxLevel)
            ReachMaxLevel();
        return true;
    }

    private void ReachMaxLevel()
    {
        //Change Color + Add feedback or change graph
        if (_data.OffensiveModuleDatas.LevelUpGraph.Length < 1)
            return;
        Destroy(this.transform.GetChild(0).gameObject);
        Instantiate(_data.OffensiveModuleDatas.LevelUpGraph[_data.OffensiveModuleDatas.LevelUpGraph.Length - 1],this.transform);
    }

    public void RemoveModule()
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
                CameraHandle.ZoomIn();
                break;
        }
        _playerStatClass.ChangeStat(StatType.Weight, -(int)_data.Weight);
        OnModuleDestroyed?.Invoke(this);
        PlayerModule.RemoveModule(this);
    }

    private void TimeTickSystemDataHandler_OnTick(uint tick)
    {
        if(this.transform == null)
            return;
        if (tick % GetTickNeeded() == 0)
        {
                for (int i = 0; i < _playerStatClass.GetStatValue(StatType.NbProjectile); i++)
                {
                    _moduleStrategy.Fire(i == 0, this.transform.rotation, this.transform.position, _firePoints, out bool success);
                    if (success)
                    {
                        OnModuleFire?.Invoke();
                        if (_audioClipName != string.Empty)
                            SoundManager.Instance.Play(_audioClipName);
                    }

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
    }

    #region HelperFunctions

    private int GetTickNeeded()
    {
        float maxTick = 20 * _currentModuleStats.currentReloadSpeedMultplier;
        float n = maxTick / _playerStatClass.GetStatValue(StatType.ReloadSpeed);
        int i = Mathf.Clamp(Mathf.CeilToInt(n), 1, Mathf.CeilToInt(maxTick));
        return i;
    }

    private void ApplyBuff(BuffDatas datas)
    {
        SingleStat statApplied = datas.GetStat();
        switch (datas.TypeBuff)
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
        switch(_data.BuffDatas.TypeBuff)
        {
            case BuffDatas.BuffType.PercentMultiply:
                _playerStatClass.RemovePercentStat(_singleStatApplied.Type, _singleStatApplied.BaseValue);
                break;
            default:
                _playerStatClass.ChangeStat(_singleStatApplied.Type, -_singleStatApplied.BaseValue);
                break;
        }
    }

    private void SetOffensiveStrategy(IStrategyModule offensiveStrategy)
    {
        _moduleStrategy = offensiveStrategy;
    }

    public ModuleDatas GetModuleDatas()
    {
        return _data;
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

    public void GatherScrapMetal(int value)
    {
        ScrapManagerDataHandler.PickUpScrap(value);
    }
    #endregion
}
