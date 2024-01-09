using System;

public static class ChronoManagerDataHandler 
{
    public static event Action<float> OnTimeElapsed;
    public static event Action<int> OnSendTimeLevel;

    public static void TimeElapsed(this ChronoManager chronoManager,float timeElapsed) => OnTimeElapsed?.Invoke(timeElapsed);
    public static void SendTimeLevel(this ChronoManager chrono, int timeLevel) => OnSendTimeLevel?.Invoke(timeLevel);
}
