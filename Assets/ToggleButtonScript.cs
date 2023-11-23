using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleButtonScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Button _button;
    private bool _toggleOn = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        _toggleOn = !_toggleOn;
        //Change Text
        if (_toggleOn)
        {
            _buttonText.text = "Replace Module: On";
            var color = _button.colors;
            color.normalColor = Color.green;
            _button.colors = color;
        }
        else
        {
            _buttonText.text = "Replace Module: Off";
            var color = _button.colors;
            color.normalColor = Color.blue;
            _button.colors = color;

        }
    }
}
