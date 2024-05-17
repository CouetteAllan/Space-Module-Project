using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseOffensiveScript: ScriptableObject,IStrategyModule
{
    protected StatClass _statClass;
    protected ModuleDatas _datas;
    protected Module.CurrentModuleStats _currentModuleStats;

    public abstract void Fire(Module module, bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, ref bool success);
    public virtual void Init(StatClass statClass, ModuleDatas datas, Transform moduleTransform, Module.CurrentModuleStats currentModuleStats)
    {
        _statClass = statClass;
        _datas = datas;
        _currentModuleStats = currentModuleStats;
    }
}

public class BooleanHolder
{
    public bool success;
    public BooleanHolder(ref bool success)
    {
        this.success = success;
    }

    public void ChangeBoolean(bool success)
    {
        this.success = success;
    }
}
