using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Module : MonoBehaviour
{

    [SerializeField] private Transform[] _firePoints;

    public enum ModuleClass
    {
        Offense,
        Defense,
        Buff,
        Placement
    }

    private ModuleClass _moduleClass;
    private ModuleDatas _data;
    private AttachPointScript _attachPoint;
    private StatClass _playerStatClass;
    private IBuff _buffStrategy;

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
                break;
            case ModuleClass.Defense:
                break;
            case ModuleClass.Buff:
                StatType buffType = datas.BuffDatas.GetStat().Type;
                switch (buffType)
                {
                    case StatType.Health:
                        break;
                    case StatType.Damage:
                        SetBuffStrategy(new DamageUpBuff(datas.BuffDatas));
                        break;
                    case StatType.Resist:
                        break;
                    case StatType.ReloadSpeed:
                        break;
                    case StatType.NbProjectile:
                        break;
                    case StatType.Weight:
                        break;
                    case StatType.MaxWeight:
                        break;
                }

                _buffStrategy.ApplyBuff(_playerStatClass);
                break;
            case ModuleClass.Placement:
                var healthScript = this.gameObject.AddComponent<HealthScript>();
                healthScript.SetAttachPoint(attachPointScript);
                break;
        }

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
                Fire(i == 0);
            }
        }
    }

    private void Fire(bool firstProjectile)
    {
        //Instantiate projectile prefab from module prefab
        foreach (Transform t in _firePoints)
        {
            Vector3 position = firstProjectile ? t.position : t.position + UtilsClass.GetRandomDir() * Random.Range(0.1f, 0.6f);

            var projectile = Instantiate(_data.ProjectilePrefab, position, this.transform.rotation).GetComponent<ProjectileScript>();
            projectile.Launch((t.position - this.transform.position).normalized, 6.0f, _playerStatClass.GetStatValue(StatType.Damage));

        }
    }


    private void OnDisable()
    {
        TimeTickSystemDataHandler.OnTick -= TimeTickSystemDataHandler_OnTick;

    }

    private int GetTickNeeded()
    {
        float n = 20 / _playerStatClass.GetStatValue(StatType.ReloadSpeed);
        int i = Mathf.Clamp(Mathf.CeilToInt(n), 1, 20);
        return i;
    }

    private void SetBuffStrategy(IBuff buff)
    {
        _buffStrategy = buff;
    }
}
