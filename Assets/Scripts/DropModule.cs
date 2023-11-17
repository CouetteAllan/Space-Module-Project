using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropModule : MonoBehaviour, IDropHandler
{
    public static event Action<AttachPointScript> OnDropModule;
    public static event Action<Module> OnModuleAttached;


    [SerializeField] private Transform _transformParent;
    [SerializeField] private AttachPointScript _attachPointScript;
    private PlayerModule _playerModule;
    

    private void OnEnable()
    {
        _playerModule = transform.parent.transform.parent.transform.parent.GetComponent<PlayerModule>();
        if (_playerModule == null)
            _playerModule = GameManager.Instance.PlayerController.GetPlayerModule();

    }
    public void OnDrop(PointerEventData eventData)
    {   
        if(eventData.pointerDrag != null)
        {
            ModuleImageScript moduleDragged = eventData.pointerDrag.GetComponent<ModuleImageScript>();
            var modulePlaced = _playerModule.PlaceModule(
                Module.CreateMod(
                    PlayerModule.GetNearestPlacement(),
                    moduleDragged.GetModuleDatas(),
                    _transformParent)
                );

            moduleDragged.ResetPos();
            OnDropModule?.Invoke(_attachPointScript);
            OnModuleAttached?.Invoke(modulePlaced);
            if(modulePlaced.GetModuleClass() != Module.ModuleClass.Placement)
                GameManager.Instance.CloseShop();
        }
    }
}
