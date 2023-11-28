using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropModuleOnCanvas : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<AttachPointScript> OnDropModule;
    public static event Action<Module> OnModuleAttached;

    public static Transform GraphPreview = null;

    [SerializeField] private Transform _transformParent;
    [SerializeField] private AttachPointScript _attachPointScript;
    private PlayerModule _playerModule;
    private Module _currentModule;
    

    private void OnEnable()
    {
        _playerModule = transform.parent.transform.parent.transform.parent.GetComponent<PlayerModule>(); //don't pay attention pls
        if (_playerModule == null)
            _playerModule = GameManager.Instance.PlayerController.GetPlayerModule();
    }
    public void OnDrop(PointerEventData eventData)
    {   
        if(eventData.pointerDrag != null && _attachPointScript.IsActive)
        {
            PlaceModule(eventData);
            
        }
        else if(eventData.pointerDrag != null && UIManager._toggleReplaceModule)
        {
            //Delete previous module
            Destroy(_currentModule.gameObject);
            //Create the new module
            PlaceModule(eventData);

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Afficher preview du prefab
        if (eventData.pointerDrag != null && _attachPointScript.IsActive)
        {
            ModuleImageScript moduleDragged = eventData.pointerDrag.GetComponent<ModuleImageScript>();

            GraphPreview = Module.CreateModPreview(
                PlayerModule.GetNearestPlacementFromMouse(),
                moduleDragged.GetModuleDatas(),
                _transformParent);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Enlever preview
        if (eventData.pointerDrag != null && GraphPreview != null)
        {
            Destroy(GraphPreview.gameObject);
        }
    }

    private void PlaceModule(PointerEventData eventData)
    {
        ModuleImageScript moduleDragged = eventData.pointerDrag.GetComponent<ModuleImageScript>();
        if (moduleDragged.GetModuleDatas().ModuleClass == Module.ModuleClass.Placement)
        {
            if (!(bool)ScrapManagerDataHandler.SellScrap(moduleDragged.GetModuleDatas().ScrapCost))
            {
                Destroy(GraphPreview?.gameObject);
                return;
            }
        }
        var modulePlaced = _playerModule.PlaceModule(
            Module.CreateMod(
                PlayerModule.GetNearestPlacementFromMouse(),
                moduleDragged.GetModuleDatas(),
                _transformParent)
            );

        moduleDragged.ResetPos();

        OnDropModule?.Invoke(_attachPointScript);
        OnModuleAttached?.Invoke(modulePlaced);

        SoundManager.Instance.Play("Reload");

        if (modulePlaced.GetModuleClass() != Module.ModuleClass.Placement)
            GameManager.Instance.CloseShop();

        _currentModule = modulePlaced;
        Destroy(GraphPreview?.gameObject);
    }
}
