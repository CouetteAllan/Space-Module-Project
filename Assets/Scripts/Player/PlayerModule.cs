using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerModule : MonoBehaviour
{
    public static List<GameObject> AttachPoints = new List<GameObject>();
    private static List<Module> _modules = new List<Module>();

    public Module PlaceModule(Module newMod)
    {
        _modules.Add(newMod);
        return newMod;
    }


    private Vector3 GetMousePos()
    {
        var mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousPos.z = 0;
        return mousPos;
    }

   
    public static List<Module> GetModules()
    {
        return _modules;
    }

    public static void RemoveModule(Module mod) {
        _modules.Remove(mod);
    }
}
