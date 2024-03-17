using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class CameraHandle : MonoBehaviour
{
    [SerializeField] private static CinemachineVirtualCamera _virtualCamera;
    private float _orthoLens = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        DropModuleOnCanvas.OnModuleAttached += DropModuleOnCanvas_OnModuleAttached;
        _orthoLens = _virtualCamera.m_Lens.OrthographicSize;
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

    private void OnDisable()
    {
        DropModuleOnCanvas.OnModuleAttached -= DropModuleOnCanvas_OnModuleAttached;
        _virtualCamera.m_Lens.OrthographicSize = _orthoLens;
    }
}
