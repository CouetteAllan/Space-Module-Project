using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_XPScript : MonoBehaviour
{
    [SerializeField] private Image _fillBar;
    [SerializeField] private TextMeshProUGUI _lvlTxt;

    public void Start()
    {
        _lvlTxt.text = $"Lvl 1";
        _fillBar.fillAmount = 0 ;
    }
    public void RefreshTextLvl(uint lvl)
    {
        _lvlTxt.text = $"Lvl {lvl}";
    }

    public void UpdateXP(uint currentXP)
    {
        _fillBar.fillAmount = (float)currentXP / (float)GameManager.Instance.NextTresholdLevelUp;
    }

}
