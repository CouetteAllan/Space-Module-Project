using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HoverTipsModule : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform _toolTipTransform;
    [SerializeField] private TextMeshProUGUI _textToolTip;
    private ModuleDatas _moduleDatas;
    private ModuleDatas Datas
    {
        get
        {
            if (_moduleDatas == null)
                _moduleDatas = this.GetComponent<ModuleImageScript>().GetModuleDatas();
            return _moduleDatas;
        }

    }


    public static event Action<string, Vector2> OnPointerHover;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Enable Tooltip
        _toolTipTransform.gameObject.SetActive(true);
        _textToolTip.text = Datas.ModuleName;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Disable Tooltip
        _toolTipTransform.gameObject.SetActive(false);
    }

}
