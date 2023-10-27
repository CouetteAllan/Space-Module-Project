using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager : Singleton<ModuleManager>
{
    [SerializeField] private List<ModuleDatas> _modules = new List<ModuleDatas>();
    private List<ModuleDatas> _modulesUsed = new List<ModuleDatas>();

    public ModuleDatas GetRandomModuleData()
    {
        int randomIndex = Random.Range(0, _modules.Count);
        var selectedModule = _modules[randomIndex];
        _modulesUsed.Add(selectedModule);
        _modules.RemoveAt(randomIndex);
        return selectedModule;
    }

    public void ResetModuleList()
    {
        _modules.AddRange(_modulesUsed);
        _modulesUsed.Clear();
    }
}
