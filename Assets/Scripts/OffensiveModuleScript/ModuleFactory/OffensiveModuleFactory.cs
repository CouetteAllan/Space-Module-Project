using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OffensiveFactoryModule",menuName ="Module/Factory/OffensiveFactory")]
public class OffensiveModuleFactory : ModuleFactory
{
    public ModuleDatas OffensiveModuleDatas;

    public override IModule MakeModule(ModuleDatas moduleDatas)
    {
        IModule newOffensiveModule = Instantiate(OffensiveModuleDatas.ModulePrefab,Vector3.zero,Quaternion.identity).GetComponent<IModule>();
        newOffensiveModule.Init(moduleDatas, null);
        return newOffensiveModule;
    }
}
