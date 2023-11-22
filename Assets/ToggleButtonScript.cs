using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleButtonScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    private bool _toggleOn = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        _toggleOn = !_toggleOn;
        //Change Text
        if (_toggleOn)
            _buttonText.text = "Place Module";
        else
            _buttonText.text = "Replace Module";
    }
}
