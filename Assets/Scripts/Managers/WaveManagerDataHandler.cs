using System;
using System.Collections.Generic;

public static class WaveManagerDataHandler 
{
    public static event Action OnWaveSpawned;
    public static event Action OnSpawnWave;
    public static event Action<List<float>> OnSendWaveTimeData;
    public static event Action OnSetUpWaveManager;

    public static void WaveSpawned() => OnWaveSpawned?.Invoke();
    public static void SpawnWave() => OnSpawnWave?.Invoke();
    public static void SendWaveTimeData(this WaveManager manager, List<float> data) => OnSendWaveTimeData?.Invoke(data);
    public static void SetUpWaveManager() => OnSetUpWaveManager?.Invoke();
}
