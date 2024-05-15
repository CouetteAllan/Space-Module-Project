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
        DoubleCanon,
        Shockwave
    }
    public BaseOffensiveScript OffensiveStrategy;
    public float BaseModuleDamage = 1.0f;
    public float AttackSpeedMultiplier = 1.0f;
    public OffensiveType Type;
    public GameObject[] LevelUpGraph;


    public IStrategyModule GetOffensiveStrategy(StatClass statClass, ModuleDatas datas, Transform moduleTransform, Module.CurrentModuleStats currentModuleStats)
    {
        OffensiveStrategy.Init(statClass, datas, moduleTransform, currentModuleStats);
        return OffensiveStrategy;
    }

}
