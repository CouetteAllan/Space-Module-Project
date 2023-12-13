using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DotChangeTab : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int _tab;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Change tab
        UIManager.Instance.ChangeScrapTab(_tab);
    }
}
