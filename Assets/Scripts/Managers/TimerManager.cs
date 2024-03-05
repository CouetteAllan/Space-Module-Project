using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _maxSeconds = 300.0f; //5min
    [SerializeField] private Animator _timerAnimator;
    private float _elapsedTime;
    private int _currentTimeLevel = 0;
    private float _timerAnotherEvent = 0.0f;
    private bool _didStart = false;
    private bool _didFireEvent = false;
    private bool _countDown = true;

    private float _nextWaveTime = 10000.0f;
    private List<float> _waveTimes;
    private int _waveIndex = 0;
    private void Start()
    {
        _elapsedTime = 0.0f;
        _timerAnotherEvent = 0.0f;
        _currentTimeLevel = 0;

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        WaveManagerDataHandler.OnSendWaveTimeData += OnSendWaveTimeData;
    }

    private void OnSendWaveTimeData(List<float> waveTimes)
    {
        _waveTimes = waveTimes;
        _waveIndex = 0;
        _nextWaveTime = _waveTimes[_waveIndex];
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
        int minutes = Mathf.FloorToInt((_maxSeconds - _elapsedTime) / 60);
        int seconds = Mathf.FloorToInt((_maxSeconds - _elapsedTime)% 60);

        _timerText.text = string.Format("{0:00}: {1:00}",minutes,seconds);


        if ((int)_elapsedTime % 10 == 0 && _countDown && _elapsedTime > 1.0f)
        {
            GetCurrentTimeLevel();
            _countDown = false;
            StartCoroutine(WaitOneSecond()); //dirty
        }

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

        if(_elapsedTime >= _nextWaveTime)
        {
            this.TriggerWave(_waveIndex);
            _nextWaveTime = GetNextWaveTime();
        }
       
        if(_elapsedTime % 60.0f <= 0.5f)
        {
            _timerAnimator.SetTrigger("Bounce");
        }
    }

    public void GetCurrentTimeLevel()
    {
        _currentTimeLevel++;
        this.SendTimeLevel(_currentTimeLevel);
    }

    private float GetNextWaveTime()
    {
        _waveIndex++;
        if (_waveIndex > _waveTimes.Count - 1)
            return 10000.0f;
        return _waveTimes[_waveIndex];
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
        WaveManagerDataHandler.OnSendWaveTimeData -= OnSendWaveTimeData;
    }
    private IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1);
        _countDown = true;
    }
}
