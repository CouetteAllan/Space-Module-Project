using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingModuleScript : IStrategyModule
{
    private HealthScript _playerHealthScript;
    private SecondaryModuleDatas _secondaryModuleDatas;

    public HealingModuleScript(HealthScript playerHealthScript, SecondaryModuleDatas _datas)
    {
        _playerHealthScript = playerHealthScript;
        _secondaryModuleDatas = _datas;
    }

    public void Fire(Module module,bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, ref bool success)
    {
        //Heal the player when it ticks
        success = true;

        _playerHealthScript.ChangeHealth((int)_secondaryModuleDatas.ModuleValue);
    }

    public void Init(StatClass statClass, ModuleDatas datas, Transform moduleTransform, Module.CurrentModuleStats currentModuleStats)
    {
        
    }
}
