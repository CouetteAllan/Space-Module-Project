using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileStrategy
{
    public void ProjectileLaunched(float speed, float duration);
}
