using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class CameraHandle : MonoBehaviour
{
    [SerializeField] private static CinemachineVirtualCamera _virtualCamera;


    // Start is called before the first frame update
    void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        DropModuleOnCanvas.OnModuleAttached += DropModuleOnCanvas_OnModuleAttached;
    }

    private void DropModuleOnCanvas_OnModuleAttached(Module mod)
    {
        if (mod.GetModuleClass() == Module.ModuleClass.Placement)
            _virtualCamera.m_Lens.OrthographicSize += 1.5f;
    }

    public static void ZoomIn()
    {
        _virtualCamera.m_Lens.OrthographicSize -= 1.5f;
    }
}
