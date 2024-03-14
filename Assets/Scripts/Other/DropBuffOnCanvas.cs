using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropBuffOnCanvas : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        ModuleImageScript moduleDragged = eventData.pointerDrag.GetComponent<ModuleImageScript>();

        if(moduleDragged.GetModuleDatas().ModuleClass == Module.ModuleClass.StatBuff)
        {
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
        }
    }
}
