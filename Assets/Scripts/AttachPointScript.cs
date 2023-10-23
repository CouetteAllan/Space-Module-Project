using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPointScript : MonoBehaviour
{
    // Start is called before the first frame update

    private bool _isActive = true;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }


    private void ModuleImageScript_OnEndDragModule()
    {
        if(_isActive && _spriteRenderer != null)
        {
            _spriteRenderer.enabled = false;
        }
    }

    private void ModuleImageScript_OnStartDragModule()
    {
        if(_isActive && _spriteRenderer != null)
        {
            _spriteRenderer.enabled = true;
        }
    }

    void Start()
    {

        PlayerModule.AttachPoints.Add(this.gameObject);
        ModuleImageScript.OnStartDragModule += ModuleImageScript_OnStartDragModule;
        ModuleImageScript.OnEndDragModule += ModuleImageScript_OnEndDragModule;
        DropModule.OnDropModule += DropModule_OnDropModule;
    }

    private void DropModule_OnDropModule(AttachPointScript attachPoint)
    {
        if(attachPoint == this)
        {
            _isActive = false;
        }
        _spriteRenderer.enabled = false;
    }

    public void EnableAttachPoint()
    {
        _isActive = true;
    }

    private void OnDisable()
    {
        ModuleImageScript.OnStartDragModule -= ModuleImageScript_OnStartDragModule;
        ModuleImageScript.OnEndDragModule -= ModuleImageScript_OnStartDragModule;
        DropModule.OnDropModule -= DropModule_OnDropModule;
    }

    private void OnDestroy()
    {
        ModuleImageScript.OnStartDragModule -= ModuleImageScript_OnStartDragModule;
        ModuleImageScript.OnEndDragModule -= ModuleImageScript_OnStartDragModule;
        DropModule.OnDropModule -= DropModule_OnDropModule;
    }
}
