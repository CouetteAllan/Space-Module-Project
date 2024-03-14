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
    [SerializeField] private GameObject _lvlUpFX;
    [SerializeField] private VolumeProfile _baseProfile;
    [SerializeField] private VolumeProfile _replaceModuleProfile;
    [SerializeField] private CinemachineVolumeSettings _volumeSettings;
    [SerializeField] private PlayFXScript[] _pickUpFX;

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

    public void PlayEffect(string effect, Vector2 pos, Quaternion rotation, Transform transformParent = null, string value = null)
    {
        switch(effect)
        {
            case "rocketBlow":
                Instantiate(_blowFX,pos,Quaternion.identity);
                SoundManager.Instance.Play("Rocket");
                break;
            case "lvlup":
                Instantiate(_lvlUpFX, pos, rotation,transformParent);
                break;
            case "explosion":
                Instantiate(_explosionFX, pos, Quaternion.identity);
                break;

            case "attackSpeed":
                var fxAttackSpeed = Instantiate(_pickUpFX[0],pos, rotation,transformParent);
                fxAttackSpeed.PlayFX(value);
                break;
            case "damageUp":
                var fxDamage = Instantiate(_pickUpFX[1], pos, rotation, transformParent);
                fxDamage.PlayFX(value);
                break;
        }
    }

    private void OnDisable()
    {
        Module.OnModuleDestroyed -= Module_OnModuleDestroyed;
        UIManager.OnToggleReplaceModule -= UIManager_OnToggleReplaceModule;

    }
}
