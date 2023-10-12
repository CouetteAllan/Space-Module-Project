using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager : Singleton<ModuleManager>
{
    [SerializeField] private List<ModuleDatas> _modules = new List<ModuleDatas>();
}
