using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New OffensiveModuleStat", menuName = "Modules/OffensiveModule")]
public class OffensiveModuleDatas : ScriptableObject
{

    public enum OffensiveType
    {
        None,
        Canon,
        Laser,
        Drone,
        Breath,
        Melee,
        Rocket,
        DoubleCanon
    }
    public float BaseModuleDamage = 1.0f;
    public float AttackSpeedMultiplier = 1.0f;
    public OffensiveType Type = OffensiveType.Canon;
    public GameObject[] LevelUpGraph;

    public IStrategyModule GetOffensiveStrategy(StatClass statClass, ModuleDatas datas, Transform moduleTransform, Module.CurrentModuleStats currentModuleStats)
    {
        switch (Type)
        {
            case OffensiveType.Canon:
            case OffensiveType.DoubleCanon:
                return new CanonModuleScript(statClass, datas, currentModuleStats);
            case OffensiveType.Laser:
                return new LaserModuleScript(statClass, datas, currentModuleStats, moduleTransform);
            case OffensiveType.Drone:
                return new DroneModuleScript(statClass, datas, currentModuleStats, moduleTransform);
            case OffensiveType.Breath:
                return new CanonModuleScript(statClass, datas, currentModuleStats);
            case OffensiveType.Melee:
                return new MeleeModuleScript(statClass, datas, currentModuleStats, moduleTransform);
            case OffensiveType.Rocket:
                return new RocketModuleScript(statClass, datas, currentModuleStats);
            default:
                return null;
        }
    }
}
