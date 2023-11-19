using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Tools;

public class TooltipScript : MonoBehaviour
{
    private static TooltipScript instance;

    private RectTransform _tooltip;
    private Text _tooltipText;

    private void Awake()
    {
        instance = this;
        _tooltip = this.transform.Find("Background").GetComponent<RectTransform>();
        _tooltipText = this.transform.Find("TooltipText").GetComponent<Text>();

        HideTooltip();
        
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out localPoint);
        transform.localPosition = localPoint;
    }

    private void ShowTooltip(string text)
    {
        this.gameObject.SetActive(true);

        _tooltipText.text = text;
        float paddingSize = 4.0f;
        Vector2 backgroundSize = new Vector2(_tooltipText.preferredWidth + paddingSize * 2f, _tooltipText.preferredHeight + paddingSize * 2f);
        _tooltip.sizeDelta = backgroundSize;
    }

    private void HideTooltip()
    {
        this.gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string text)
    {
        instance.ShowTooltip(text);
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }
}
