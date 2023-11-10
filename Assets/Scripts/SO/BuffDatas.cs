using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Modules/Buff")]
public class BuffDatas : ScriptableObject
{
    public SingleStat Stat;

    public SingleStat GetStat(StatClass statClass)
    {
        return new SingleStat (Stat.BaseValue, Stat.Type, statClass);
    }
}
