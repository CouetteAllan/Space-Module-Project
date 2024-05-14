using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveProjectile : ProjectileBehaviour
{
    public override void LaunchProjectile(GameObject projectileGO, ProjectileScript.ProjectileParameter projectileParameters)
    {
        
    }

    public override void ProjectileEnd(GameObject projectileGO, ProjectileScript.ProjectileParameter projectileParameter)
    {
        throw new System.NotImplementedException();
    }
}
