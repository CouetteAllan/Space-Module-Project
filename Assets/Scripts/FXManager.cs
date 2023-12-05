using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine.Rendering;
using System.Linq;

public class FXManager : Singleton<FXManager>
{
    [SerializeField] private GameObject _explosionFX;
    [SerializeField] private GameObject _blowFX;
    [SerializeField] private VolumeProfile _baseProfile;
    [SerializeField] private VolumeProfile _replaceModuleProfile;
    [SerializeField] private CinemachineVolumeSettings _volumeSettings;

    private void Start()
    {
        Module.OnModuleDestroyed += Module_OnModuleDestroyed;
        UIManager.OnToggleReplaceModule += UIManager_OnToggleReplaceModule;
    }

    private void UIManager_OnToggleReplaceModule(bool toggleOn)
    {
        _volumeSettings.m_Profile = toggleOn ? _replaceModuleProfile : _baseProfile;
    }

    private void Module_OnModuleDestroyed(Module mod)
    {
        Instantiate(_explosionFX,mod.transform.position,Quaternion.identity);
    }

    public void PlayEffect(string effect, Vector2 pos)
    {
        switch(effect)
        {
            case "rocketBlow":
                Instantiate(_blowFX,pos,Quaternion.identity);
                SoundManager.Instance.Play("Rocket");
                break;
        }
    }

    private void OnDisable()
    {
        Module.OnModuleDestroyed -= Module_OnModuleDestroyed;
        UIManager.OnToggleReplaceModule -= UIManager_OnToggleReplaceModule;

    }
}
