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
        foreach (var module in _modules)
        {
            Debug.Log(module);
        }
        return newMod;
    }


    private Vector3 GetMousePos()
    {
        var mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousPos.z = 0;
        return mousPos;
    }

    public static Vector2 GetNearestPlacementFromMouse()
    {
        var nearestPos = Vector2.zero;
        float minDist = float.MaxValue;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; 
        foreach (var attach in AttachPoints)
        {
            if(attach == null) continue;
            Vector3 directionToTarget = attach.gameObject.transform.position - mousePos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < minDist)
            {
                minDist = dSqrToTarget;
                nearestPos = attach.gameObject.transform.position;
            }
        }

        return nearestPos;
    }

    public static List<Module> GetModules()
    {
        return _modules;
    }

    public static void RemoveModule(Module mod) {
        _modules.Remove(mod);
    }
}
