using System;

public static class ChronoManagerDataHandler 
{
    public static event Action<float> OnTimeElapsed;
    public static event Action<int> OnSendTimeLevel;
    public static event Action<int> OnTriggerWave;

    public static void TimeElapsed(this TimerManager chronoManager,float timeElapsed) => OnTimeElapsed?.Invoke(timeElapsed);
    public static void SendTimeLevel(this TimerManager chrono, int timeLevel) => OnSendTimeLevel?.Invoke(timeLevel);
    public static void TriggerWave(this TimerManager chrono, int waveIndex) => OnTriggerWave?.Invoke(waveIndex);
}
