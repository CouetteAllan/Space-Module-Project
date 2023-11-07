using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Stat", menuName = "New Base Stat")]
public class BaseStatDatas : ScriptableObject
{
    public List<SingleStat> singleStats = new List<SingleStat>();

    public Dictionary<StatType, SingleStat> GetDictionary(StatClass statClass)
    {
        Dictionary<StatType, SingleStat> statDictionary = new Dictionary<StatType, SingleStat>();
        foreach (var stat in singleStats)
        {
            SingleStat newStat = new SingleStat(stat.BaseValue, stat.Type, statClass);
            statDictionary.Add(stat.Type, newStat);
            Debug.Log(newStat.ToString());
        }
        return statDictionary;
    }
}
