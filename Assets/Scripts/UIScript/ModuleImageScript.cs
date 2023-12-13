using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Text;

public class ModuleImageScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action OnStartDragModule;
    public static event Action OnEndDragModule;

    [SerializeField] private ModuleDatas _moduleDatas;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _textObject;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Image _image;
    private LayoutElement _layoutElement;

    private Vector3 _startPosition;
    private Vector3 _startLocalPosition;
    private Vector3 _startLocalScale;

    private int _lastRegisterScrap = 0;
    private float _currentAlpha = 1.0f;

    private int _currentPrice = 0;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _layoutElement = GetComponent<LayoutElement>();
        _image = GetComponent<Image>();
        _startLocalPosition = _rectTransform.localPosition;
        _image.sprite = _moduleDatas.ModuleSprite;
        _description.text = _moduleDatas.ModuleDescription;
        _currentPrice = _moduleDatas.ScrapCost;
        UIManager.OnCloseScrapShop += OnCloseScrapShop;
        if(_moduleDatas.OffensiveModuleDatas == null)
            ScrapManagerDataHandler.OnSellScrapSuccess += OnSellScrapSuccess;
    }

    private void OnSellScrapSuccess(StatType buffTypeSold,int nbSold)
    {
        if (_moduleDatas.BuffDatas == null)
            return;

        if (buffTypeSold != _moduleDatas.BuffDatas.GetStat().Type)
            return;

        _currentPrice += nbSold;
        //update specific module text
        string newDesc = _moduleDatas.ModuleDescription;
        int index = newDesc.IndexOf("Cost");
        newDesc = newDesc.Remove(index);
        newDesc = newDesc.Insert(index, $"<color=orange>Cost: {_currentPrice} Fe) </color>");
        _description.text = newDesc;
        Debug.Log(newDesc);
    }

    private void OnCloseScrapShop()
    {
        ResetPos();
    }

    private void Start()
    {
        _startPosition = _rectTransform.anchoredPosition;
        _startLocalScale = _rectTransform.localScale;
        ScrapManagerDataHandler.OnUpdateScrap += OnUpdateScrap;

    }

    private void OnUpdateScrap(int scrapLeft)
    {
        //Grey image if can purchase
        _lastRegisterScrap = scrapLeft;
        _canvasGroup.alpha = CanPurchase() ? 1.0f : 0.5f;
        _currentAlpha = _canvasGroup.alpha;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        _textObject.SetActive(false);
        this._rectTransform.localScale /= 2;
        _canvasGroup.alpha = 0.7f;
        OnStartDragModule?.Invoke();
        _layoutElement.ignoreLayout = true;
        _rectTransform.position = eventData.position;
    }

    public void OnDrag(PointerEventData pointerData)
    {
        _rectTransform.anchoredPosition += pointerData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       ResetPos();
       OnEndDragModule?.Invoke();
    }

    public void ResetPos()
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = _currentAlpha;
        _rectTransform.anchoredPosition = _startPosition;
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _layoutElement.ignoreLayout = false;
        _rectTransform.localScale = _startLocalScale;
        if(_textObject != null)
            _textObject.SetActive(true);
    }

    public ModuleDatas GetModuleDatas()
    {
        return _moduleDatas;
    }

    public int GetCurrentModuleCost()
    {
        return _currentPrice;
    }

    public void SetModuleDatas(ModuleDatas datas)
    {
        _moduleDatas = datas;
        _image.sprite = _moduleDatas.ModuleSprite;
        _description.text = _moduleDatas.ModuleDescription;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipScript.ShowTooltip_Static(_moduleDatas.ModuleName);
        _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipScript.HideTooltip_Static();
        _highlight.SetActive(false);

    }

    private bool CanPurchase()
    {
        return _lastRegisterScrap >= _currentPrice;
    }

    private void OnDestroy()
    {
        ScrapManagerDataHandler.OnUpdateScrap -= OnUpdateScrap;
        UIManager.OnCloseScrapShop -= OnCloseScrapShop;

    }
}
