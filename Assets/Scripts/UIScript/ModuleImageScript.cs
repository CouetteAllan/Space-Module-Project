using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ModuleImageScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action OnStartDragModule;
    public static event Action OnEndDragModule;

    [SerializeField] private ModuleDatas _moduleDatas;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private GameObject _highlight;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Image _image;
    private LayoutElement _layoutElement;

    private Vector3 _startPosition;
    private Vector3 _startLocalPosition;

    private int _lastRegisterScrap = 0;
    private float _currentAlpha = 1.0f;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _layoutElement = GetComponent<LayoutElement>();
        _image = GetComponent<Image>();
        _startLocalPosition = _rectTransform.localPosition;
        _image.sprite = _moduleDatas.ModuleSprite;
        _description.text = _moduleDatas.ModuleDescription;

    }

    private void Start()
    {
        _startPosition = _rectTransform.anchoredPosition;
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
        _canvasGroup.alpha = 0.3f;
        OnStartDragModule?.Invoke();
        _layoutElement.ignoreLayout = true;
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
    }

    public ModuleDatas GetModuleDatas()
    {
        return _moduleDatas;
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
        return _lastRegisterScrap >= _moduleDatas.ScrapCost;
    }

    private void OnDestroy()
    {
        ScrapManagerDataHandler.OnUpdateScrap -= OnUpdateScrap;

    }
}
