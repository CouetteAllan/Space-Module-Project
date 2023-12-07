using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChronoManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private float _elapsedTime;
    private float _timerAnotherEvent = 0.0f;
    private bool _didStart = false;
    private bool _didFireEvent = false;
    private void Start()
    {
        _elapsedTime = 0.0f;
        _timerAnotherEvent = 0.0f;

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        _didStart = newState == GameState.InGame;
    }

    private void Update()
    {
        if (!_didStart)
            return;
        _elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);

        _timerText.text = string.Format("{0:00}: {1:00}",minutes,seconds);

        if (_elapsedTime > 90.0f && !_didFireEvent)
        {
            this.TimeElapsed(_elapsedTime);
            _didFireEvent = true;
        }

        if (_didFireEvent)
        {
            _timerAnotherEvent += Time.deltaTime;
            if (_timerAnotherEvent > 40.0f)
            {
                _didFireEvent = false;
                _timerAnotherEvent = 0.0f;
            }

        }

    }
}
