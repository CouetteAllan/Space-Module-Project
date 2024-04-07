using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineScriptManager : Singleton<TimelineScriptManager>
{
    [SerializeField] private PlayableDirector _timeline;
    [SerializeField] private PlayableDirector _timelineBossCinematic;
    private Action _endCinematicCallback;

    protected override void Awake()
    {
        base.Awake();
        GameManager.OnGameStateChanged += OnGameStateChanged;
        EnemyManagerDataHandler.OnTriggerBossCinematic += OnTriggerBossCinematic;
        _timelineBossCinematic.stopped += TimeLineBoss_stopped;
    }

    private void TimeLineBoss_stopped(PlayableDirector PlayableDirector)
    {
        _endCinematicCallback?.Invoke();
    }

    private void OnTriggerBossCinematic(Action callback)
    {
        //Play a new Timeline
        _endCinematicCallback = callback;
        _timelineBossCinematic.Play();
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.StartGame && !GameManager.Instance.HasShownTutoOnce)
            _timeline.Play();
    }

    public void EndTimeline()
    {
        _endCinematicCallback?.Invoke();
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        _timelineBossCinematic.stopped -= TimeLineBoss_stopped;
        EnemyManagerDataHandler.OnTriggerBossCinematic -= OnTriggerBossCinematic;

    }
}
