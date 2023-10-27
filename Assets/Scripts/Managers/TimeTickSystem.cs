using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTickSystem : MonoBehaviour
{
    private const float TICK_TIMER_MAX = 0.2f;
    private const float TICK_TIMERFAST_MAX = 0.04f;

    private uint _tick = 0;
    private uint _tickFast = 0;
    private float _tickTimer;
    private float _tickTimerFast;

    private void Awake()
    {
        _tickTimer = 0.0f;
        _tickTimerFast = 0.0f;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.InGame)
            return;

        _tickTimer += Time.deltaTime;
        _tickTimerFast += Time.deltaTime;
        if (_tickTimerFast > TICK_TIMERFAST_MAX)
        {
            _tickTimerFast -= TICK_TIMERFAST_MAX;
            _tickFast++;
            this.TickFaster(_tickFast);

        }
        if (_tickTimer > TICK_TIMER_MAX)
        {
            _tickTimer -= TICK_TIMER_MAX;
            _tick++;
            this.Tick(_tick);
        }
    }
}