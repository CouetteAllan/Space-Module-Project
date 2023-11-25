using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseOffensiveScript: IOffensiveModule
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

    public abstract void Fire(bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success);
}
