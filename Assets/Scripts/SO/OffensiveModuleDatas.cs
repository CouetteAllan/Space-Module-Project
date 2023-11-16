using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New OffensiveModuleStat", menuName = "Modules/OffensiveModule")]
public class OffensiveModuleDatas : ScriptableObject
{

    public enum OffensiveType
    {
        Canon,
        Laser,
        Drone,
        Breath
    }
    public float BaseModuleDamage = 1.0f;
    public float AttackSpeedMultiplier = 1.0f;
    public OffensiveType Type = OffensiveType.Canon;

    public IOffensiveModule GetOffensiveStrategy(StatClass statClass, ModuleDatas datas, Transform moduleTransform)
    {
        switch (Type)
        {
            case OffensiveType.Canon:
                return new CanonModuleScript(statClass, datas, BaseModuleDamage);
            case OffensiveType.Laser:
                return new LaserModuleScript(statClass, datas, BaseModuleDamage,moduleTransform);
            case OffensiveType.Drone:
                return new CanonModuleScript(statClass, datas, BaseModuleDamage);
            case OffensiveType.Breath:
                return new CanonModuleScript(statClass, datas, BaseModuleDamage);
            default:
                return null;
        }
    }
}
