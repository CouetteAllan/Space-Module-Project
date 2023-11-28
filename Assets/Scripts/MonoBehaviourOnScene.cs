using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourOnScene : Singleton<MonoBehaviourOnScene>
{
    public static event Action OnUpdate;
    void Update()
    {
        OnUpdate?.Invoke();
    }

}
