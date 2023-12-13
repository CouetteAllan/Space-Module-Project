using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseOffensiveScript: IStrategyModule
{
    protected StatClass _statClass;
    protected ModuleDatas _datas;
    protected Module.CurrentModuleStats _currentModuleStats;


    public BaseOffensiveScript(StatClass statClass, ModuleDatas datas, Module.CurrentModuleStats currentModuleStats)
    {
        _statClass = statClass;
        _datas = datas;
        _currentModuleStats = currentModuleStats;
    }

    public abstract void Fire(bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success);
}
