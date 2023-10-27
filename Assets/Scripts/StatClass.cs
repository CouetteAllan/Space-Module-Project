using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class StatClass
{
    private Dictionary<StatType,SingleStat> _statDictionary = new Dictionary<StatType,SingleStat>();

    public StatClass()
    {
        _statDictionary = PopulateDictionary();
    }

    public void ChangeStat(SingleStat newStat)
    {
        if(_statDictionary.TryGetValue(newStat.Type, out SingleStat stat))
        {
            stat += newStat;
        }
    }

    public void ChangeStat(StatType statType, float newValue)
    {
        if (_statDictionary.TryGetValue(statType, out SingleStat stat))
        {
            stat.ChangeValue(newValue);
        }
    }

    public float GetStatValue(StatType type)
    {
        if(_statDictionary.TryGetValue(type, out SingleStat stat))
        {
            return stat.Value;
        }
        else
            throw new ArgumentException("Stat Type not found");
    }


    private Dictionary<StatType, SingleStat> PopulateDictionary()
    {
        //Need to change to control the base stat all over a manager or some base settings
        Dictionary<StatType, SingleStat> statDictionnary = new Dictionary<StatType, SingleStat>();
        statDictionnary.Add(StatType.Damage, new SingleStat(3.0f, StatType.Damage, this));
        statDictionnary.Add(StatType.Health, new SingleStat(50.0f, StatType.Health, this));
        statDictionnary.Add(StatType.NbProjectile, new SingleStat(1.0f, StatType.NbProjectile, this));
        statDictionnary.Add(StatType.Resist, new SingleStat(0.0f, StatType.Resist, this));
        statDictionnary.Add(StatType.ReloadSpeed, new SingleStat(1.0f, StatType.ReloadSpeed, this));
        statDictionnary.Add(StatType.Weight, new SingleStat(0.0f, StatType.Weight, this));
        return statDictionnary;
    }
}
