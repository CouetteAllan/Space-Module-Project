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

    public void Fire(bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success)
    {
        //Heal the player when it ticks
        success = true;

        _playerHealthScript.ChangeHealth((int)_secondaryModuleDatas.ModuleValue);
    }
}
