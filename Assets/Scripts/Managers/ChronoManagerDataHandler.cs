using System;

public static class ChronoManagerDataHandler 
{
    public static event Action<float> OnTimeElapsed;

    public static void TimeElapsed(this ChronoManager chronoManager,float timeElapsed) => OnTimeElapsed?.Invoke(timeElapsed);
}
