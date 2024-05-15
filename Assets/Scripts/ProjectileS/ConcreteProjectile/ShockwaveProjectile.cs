using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveProjectile : ProjectileBehaviour
{
    public override void LaunchProjectile(ProjectileScript projectileGO, ProjectileScript.ProjectileParameter projectileParameters)
    {
        
    }

    public override void ProjectileEnd(ProjectileScript projectileGO, ProjectileScript.ProjectileParameter projectileParameter)
    {
        throw new System.NotImplementedException();
    }
}
