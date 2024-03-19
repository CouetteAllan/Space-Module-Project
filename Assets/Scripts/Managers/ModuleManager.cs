using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager : Singleton<ModuleManager>
{
    [SerializeField] private List<ModuleDatas> _modules = new List<ModuleDatas>();
    private List<ModuleDatas> _modulesUsed = new List<ModuleDatas>();
    private Dictionary<ModuleDatas, int> _modulePonderationWeight = new Dictionary<ModuleDatas, int>();
    private Dictionary<ModuleDatas, int> _modulePonderationWeightUsed = new Dictionary<ModuleDatas, int>();
    private int _weightTotal = 0;

    private void Start()
    {
        DropModuleOnCanvas.OnModuleAttached += OnModuleAttached;
    }
    private void OnDisable()
    {
        DropModuleOnCanvas.OnModuleAttached -= OnModuleAttached;
    }

    private void OnModuleAttached(Module moduleAttached)
    {
        ModuleDatas datas = moduleAttached.GetModuleDatas();
        ModulePicked(datas);
    }

    public void SetModuleManager()
    {
        _modulePonderationWeight.Clear();
        foreach (var module in _modules)
        {
            _modulePonderationWeight.Add(module, module.PonderationWeight);
        }
        foreach (int weights in _modulePonderationWeight.Values)
        {
            _weightTotal += weights;
        }
    }

    public ModuleDatas GetRandomModuleData()
    {

        ModuleDatas selectedModule = _modules[0];
        int index = 0;
        int total = 0;
        int randVal = Random.Range(0, _weightTotal);
        foreach (var weight in _modulePonderationWeight)
        { 
            total += weight.Value;
            if (total > randVal)
            {
                selectedModule = weight.Key;
                break;
            }
            index++;
        }

        _modulesUsed.Add(selectedModule);
        _modules.Remove(selectedModule);
        _modulePonderationWeightUsed.Add(selectedModule, _modulePonderationWeight.GetValueOrDefault(selectedModule));
        _modulePonderationWeight.Remove(selectedModule);

        return selectedModule;
    }

    public void ResetModuleList()
    {
        _modules.AddRange(_modulesUsed);
        _modulesUsed.Clear();
        foreach (var weight in _modulePonderationWeightUsed)
        {
            _modulePonderationWeight.Add(weight.Key,weight.Value);
        }
        _modulePonderationWeightUsed.Clear();
        _weightTotal = 0;
        foreach (var weights in _modulePonderationWeight)
        {
            _weightTotal += weights.Value;
        }

    }

    public void ModulePicked(ModuleDatas module)
    {
        if(_modulePonderationWeightUsed.TryGetValue(module, out var weight))
        {
            weight /= 2;
            _modulePonderationWeightUsed[module] = weight;
        }
    }
}
