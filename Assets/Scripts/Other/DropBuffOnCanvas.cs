using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropBuffOnCanvas : MonoBehaviour, IDropHandler
{
    public static event Action<BuffDatas> OnBuffModuleDrop;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        ModuleImageScript moduleDragged = eventData.pointerDrag.GetComponent<ModuleImageScript>();

        if (moduleDragged.GetModuleDatas().ModuleClass == Module.ModuleClass.StatBuff)
        {
            if (!PlaceModule(moduleDragged))
                return;

            var datas = moduleDragged.GetModuleDatas().BuffDatas;
            var playerStats = StatSystem.Instance.PlayerStat;

            SingleStat statApplied = datas.GetStat();
            switch (datas.TypeBuff)
            {
                case BuffDatas.BuffType.Add:
                    playerStats.ChangeStat(statApplied.Type, statApplied.BaseValue);

                    break;
                case BuffDatas.BuffType.Multiply:
                    playerStats.MultiplyStat(statApplied.Type, statApplied.BaseValue);
                    break;
                case BuffDatas.BuffType.PercentMultiply:
                    playerStats.MultiplyPercentStat(statApplied.Type, statApplied.BaseValue);
                    break;
            }

            Debug.Log("Buff dropped !: " + datas);
            string fxName = datas.Stat.Type == StatType.ReloadSpeed ? "attackSpeed" : "damageUp";
            float statValue = datas.GetStat().Type == StatType.Weight ? datas.GetStat().Value : datas.GetStat().Value * 100.0f;
            string fxValue = statValue.ToString("0") + '%';
            Transform playerPos = GameManager.Instance.PlayerController.transform;
            FXManager.Instance.PlayEffect(fxName, playerPos.position, Quaternion.identity, playerPos, fxValue);

        }

    }

    private bool PlaceModule(ModuleImageScript moduleDragged)
    {
        //See if we have enough scrap to place some modules

        StatType statGranted = moduleDragged.GetModuleDatas().BuffDatas != null ? moduleDragged.GetModuleDatas().BuffDatas.GetStat().Type : StatType.Weight;
        if (!(bool)ScrapManagerDataHandler.SellScrap(moduleDragged.GetCurrentModuleCost(), statGranted))
        {
            SoundManager.Instance.Play("Cancel");
            return false;
        }


        moduleDragged.ResetPos();

        OnBuffModuleDrop?.Invoke(moduleDragged.GetModuleDatas().BuffDatas);

        SoundManager.Instance.Play("Reload");

        return true;
    }
}
