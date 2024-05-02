using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private Vector3 _startPosition;
    private void Start()
    {
        _startPosition = this.transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GrowButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnGrowButton();
    }


    public void OnSelect(BaseEventData eventData)
    {
        GrowButton();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        UnGrowButton();
    }

    private void GrowButton()
    {
        this.transform.DOScale(Vector3.one * 1.2f, .8f).SetEase(Ease.OutBack);
        this.transform.DOMoveY(transform.position.y + 20.0f, 1.0f).SetEase(Ease.OutSine);
    }

    private void UnGrowButton()
    {
        transform.DOScale(Vector3.one, .8f).SetEase(Ease.OutBack);
        this.transform.DOMoveY(_startPosition.y, 1.0f).SetEase(Ease.OutSine);
    }
}
