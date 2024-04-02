using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveSO[] _wavesDatas;

    private void Start()
    {
        TimerManagerDataHandler.OnTriggerWave += OnTriggerWave;
        SetUpWaveManager();
    }

    private void SetUpWaveManager()
    {
        this.SendWaveTimeData(GetWaveTimeData());
    }

    private void OnTriggerWave(int waveIndex)
    {
        SpawnWave(_wavesDatas[waveIndex]);
    }


    private void SpawnWave(WaveSO wave)
    {
        foreach(WaveComponent waveComp in wave.WaveComponents)
        {
            //Spawn at random location away from the player
            EnemyManagerDataHandler.SpawnEnemyWave(waveComp.enemy, waveComp.number);
        }
    }

    private List<float> GetWaveTimeData()
    {
        var waveTimes = new List<float>();
        foreach (var wave in _wavesDatas)
        {
            waveTimes.Add(wave.TimeInSeconds);
        }
        waveTimes.Sort();
        return waveTimes;
    }

    private void OnDisable()
    {
        TimerManagerDataHandler.OnTriggerWave -= OnTriggerWave;

    }

}
