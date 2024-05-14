using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehaviour : ScriptableObject
{
    public float RecoilMult = 1.3f;


    public abstract void LaunchProjectile(GameObject projectileGO, ProjectileScript.ProjectileParameter projectileParameters);

    public abstract void ProjectileEnd(GameObject projectileGO, ProjectileScript.ProjectileParameter projectileParameters);
}
