using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveSO[] _wavesDatas;

    private void Start()
    {
        ChronoManagerDataHandler.OnTriggerWave += OnTriggerWave;
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
        //Spawn each wave at time T
        foreach(WaveComponent waveComp in wave.WaveComponents)
        {
            Debug.Log($"We spawn wave at {wave.TimeInSeconds} with {waveComp.number} enemies of type {waveComp.enemy.Tier}");
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
        ChronoManagerDataHandler.OnTriggerWave -= OnTriggerWave;

    }
}
