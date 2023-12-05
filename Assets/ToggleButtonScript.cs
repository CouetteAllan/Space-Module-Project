using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleButtonScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Image _button;
    private bool _toggleOn = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        _toggleOn = UIManager._toggleReplaceModule;
        //Change Text
        if (_toggleOn)
        {
            _buttonText.text = "Replace Module: On";
            var newColor = _button.color;
            newColor = Color.green;
            _button.color = newColor;
        }
        else
        {
            _buttonText.text = "Replace Module: Off";
            var color = _button.color;
            color = Color.cyan;
            _button.color = color;

        }
    }
}
