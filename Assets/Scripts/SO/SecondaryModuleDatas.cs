using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Secondary Module",menuName = "Module/Secondary Module")]
public class SecondaryModuleDatas : ScriptableObject
{
    public float ModuleValue;
    public int TickMultiplier = 3;
    public enum SecondaryModuleType
    {
        None,
        Heal,
        Defense
    }
    public SecondaryModuleType ModuleType;



    public IStrategyModule GetStrategyModule(PlayerController player)
    {
        switch (ModuleType)
        {
            default:
                return null;
            case SecondaryModuleType.Heal:
                return new HealingModuleScript(player.GetHealthScript(), this);
        }
    }
}
