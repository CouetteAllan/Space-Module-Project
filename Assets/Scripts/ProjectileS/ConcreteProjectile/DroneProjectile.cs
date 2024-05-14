using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drone", menuName = "Projectiles/Drone Behaviour")]
public class DroneProjectile : ProjectileBehaviour
{
    public override void LaunchProjectile(GameObject projectileGO, ProjectileScript.ProjectileParameter projectileParameter)
    {

    }

    public override void ProjectileEnd(GameObject projectileGO, ProjectileScript.ProjectileParameter projectileParameter)
    {
        throw new System.NotImplementedException();
    }
}
