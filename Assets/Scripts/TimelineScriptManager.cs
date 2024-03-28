using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineScriptManager : Singleton<TimelineScriptManager>
{
    [SerializeField] private PlayableDirector _timeline;

    protected override void Awake()
    {
        base.Awake();
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.StartGame && !GameManager.Instance.HasShownTutoOnce)
            _timeline.Play();
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;

    }
}
