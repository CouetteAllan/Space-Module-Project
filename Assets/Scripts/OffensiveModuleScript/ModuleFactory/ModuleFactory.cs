using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModuleFactory : ScriptableObject
{
    public abstract IModule MakeModule(ModuleDatas moduleDatas);
}
