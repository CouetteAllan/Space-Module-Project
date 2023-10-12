using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TimeTickSystemDataHandler
{
    public static event Action<uint> OnTick;
    /// <summary>
    /// Tick every 0.08sec
    /// </summary>
    public static event Action<uint> OnTickFaster;
    public static void Tick(this TimeTickSystem timeTickSystem, uint tick) => OnTick?.Invoke(tick);
    public static void TickFaster(this TimeTickSystem timeTickSystem, uint tick) => OnTickFaster?.Invoke(tick);
}