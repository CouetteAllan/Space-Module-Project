using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseOffensiveScript 
{
    protected StatClass _statClass;
    protected ModuleDatas _datas;
    protected float _baseDamage;


    public BaseOffensiveScript(StatClass statClass, ModuleDatas datas, float baseDamage)
    {
        _statClass = statClass;
        _datas = datas;
        _baseDamage = baseDamage;
    }
}
