using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpBuff : IBuff
{
    private BuffDatas _datas;
    public DamageUpBuff(BuffDatas datas)
    {
        _datas = datas;
    }

    public void ApplyBuff(StatClass statClass)
    {
        statClass.ChangeStat(StatType.Damage, _datas.GetStat().BaseValue);
    }

    public void RemoveBuff(StatClass statClass)
    {
        statClass.ChangeStat(StatType.Damage, -_datas.GetStat().BaseValue);
    }
}
