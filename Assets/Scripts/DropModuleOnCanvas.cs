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
    [SerializeField] private PreviewLvlUp _preview;
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
        if (eventData.pointerDrag == null)
            return;

        ModuleImageScript moduleDragged = eventData.pointerDrag.GetComponent<ModuleImageScript>();

        if (_attachPointScript.IsActive)
        {
            PlaceModule(moduleDragged);
        }
        else if(moduleDragged.GetModuleDatas().OffensiveModuleDatas.Type == _currentModule.OffensiveType)
        {
            //lvl up module
            moduleDragged.ResetPos();

            SoundManager.Instance.Play("Reload"); //instead play lvl up module

            _currentModule.LevelUpModule();
            if (_currentModule.GetModuleClass() != Module.ModuleClass.Placement && _currentModule.GetModuleClass() != Module.ModuleClass.StatBuff)
                GameManager.Instance.CloseShop();
            if (GraphPreview?.gameObject != null)
                Destroy(GraphPreview?.gameObject);

            OnDropModule?.Invoke(_attachPointScript);

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Afficher preview du prefab
        if (eventData.pointerDrag == null)
            return;

        ModuleImageScript moduleDragged = eventData.pointerDrag.GetComponent<ModuleImageScript>();

        if (_attachPointScript.IsActive)
        {

            GraphPreview = Module.CreateModPreview(
                _transformParent.position,
                moduleDragged.GetModuleDatas(),
                _transformParent);
        }

        if (_currentModule == null)
            return;

        if (moduleDragged.GetModuleDatas().OffensiveModuleDatas.Type == _currentModule?.OffensiveType)
        {
            //Show if we can fuse modules
            var currentPreview = PreviewLvlUp.InstantiateTextObject(_currentModule.transform.position + _currentModule.transform.up, _preview);
            GraphPreview = currentPreview.transform;
            currentPreview.SetText("Lvl " + _currentModule.CurrentLevel);
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

    private bool PlaceModule(ModuleImageScript moduleDragged)
    {
        //See if we have enough scrap to place some modules
        if (!(bool)ScrapManagerDataHandler.SellScrap(moduleDragged.GetModuleDatas().ScrapCost))
        {
            Destroy(GraphPreview?.gameObject);
            SoundManager.Instance.Play("Cancel");
            return false;
        }

        var modulePlaced = _playerModule.PlaceModule(
            Module.CreateMod(
                _transformParent.position,
                moduleDragged.GetModuleDatas(),
                _transformParent)
            );

        moduleDragged.ResetPos();

        OnDropModule?.Invoke(_attachPointScript);
        OnModuleAttached?.Invoke(modulePlaced);

        SoundManager.Instance.Play("Reload");

        if (modulePlaced.GetModuleClass() != Module.ModuleClass.Placement && modulePlaced.GetModuleClass() != Module.ModuleClass.StatBuff)
            GameManager.Instance.CloseShop();

        _currentModule = modulePlaced;
        if(GraphPreview?.gameObject != null)
            Destroy(GraphPreview?.gameObject);
        return true;
    }
}
