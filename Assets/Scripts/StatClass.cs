using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class StatClass
{
    private Dictionary<StatType,SingleStat> _statDictionary = new Dictionary<StatType,SingleStat>();

    public StatClass(BaseStatDatas baseDatas)
    {
        _statDictionary = baseDatas.GetDictionary(this);
    }

    public void AddStat(SingleStat newStat)
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

    public void MultiplyStat(StatType statType, float newValue)
    {
        if (_statDictionary.TryGetValue(statType, out SingleStat stat))
        {
            stat.MultiplyValue(newValue);
        }
    }

    public void MultiplyPercentStat(StatType statType, float newValue)
    {
        if (_statDictionary.TryGetValue(statType, out SingleStat stat))
        {
            stat.MultiplyPercentValue(newValue);
        }
    }

    public void RemovePercentStat(StatType statType, float percent)
    {
        if (_statDictionary.TryGetValue(statType, out SingleStat stat))
        {
            stat.RemovePercentBuff(percent);
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


}
