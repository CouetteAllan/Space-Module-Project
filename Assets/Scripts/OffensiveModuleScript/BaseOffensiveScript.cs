using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseOffensiveScript 
{
    protected StatClass _statClass;
    protected ModuleDatas _datas;


    public BaseOffensiveScript(StatClass statClass, ModuleDatas datas)
    {
        _statClass = statClass;
        _datas = datas;
    }
}
