using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropModule : MonoBehaviour, IDropHandler
{
    public static event Action<AttachPointScript> OnDropModule;


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
            var modulePlaced = _playerModule.PlaceModule(Module.CreateMod(PlayerModule.GetNearestModule(), eventData.pointerDrag.GetComponent<ModuleImageScript>().GetModuleDatas(), _transformParent));
            //Destroy(eventData.pointerDrag );
            eventData.pointerDrag.GetComponent<ModuleImageScript>().ResetPos();
            OnDropModule?.Invoke(_attachPointScript);
            if(modulePlaced.GetModuleClass() != Module.ModuleClass.Placement)
                GameManager.Instance.CloseShop();
        }
    }
}
