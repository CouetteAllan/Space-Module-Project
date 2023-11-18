using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropModule : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<AttachPointScript> OnDropModule;
    public static event Action<Module> OnModuleAttached;

    public static Transform GraphPreview = null;

    [SerializeField] private Transform _transformParent;
    [SerializeField] private AttachPointScript _attachPointScript;
    private PlayerModule _playerModule;
    

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
            ModuleImageScript moduleDragged = eventData.pointerDrag.GetComponent<ModuleImageScript>();
            var modulePlaced = _playerModule.PlaceModule(
                Module.CreateMod(
                    PlayerModule.GetNearestPlacementFromMouse(),
                    moduleDragged.GetModuleDatas(),
                    _transformParent)
                );

            moduleDragged.ResetPos();
            OnDropModule?.Invoke(_attachPointScript);
            OnModuleAttached?.Invoke(modulePlaced);
            if(modulePlaced.GetModuleClass() != Module.ModuleClass.Placement)
                GameManager.Instance.CloseShop();
            Destroy(GraphPreview?.gameObject);
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
        if (eventData.pointerDrag != null)
        {
            Destroy(GraphPreview?.gameObject);
        }
    }
}
