using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using DG.Tweening;

[CreateAssetMenu(fileName = "Drone", menuName = "Projectiles/Drone Behaviour")]
public class DroneProjectile : ProjectileBehaviour
{
    public override void LaunchProjectile(ProjectileScript projectile, ProjectileScript.ProjectileParameter projectileParameter)
    {
        //revolve the drone around the parent module
        projectile.RevolveAroundModule(projectileParameter);
        FunctionTimer.Create(() => ProjectileEnd(projectile, projectileParameter), projectileParameter.duration);
    }

    public override void ProjectileEnd(ProjectileScript projectile, ProjectileScript.ProjectileParameter projectileParameter)
    {
        //Destroy the drone;
        if (projectile == null)
            return;

        projectile.transform.DOPunchPosition(projectile.transform.up,1.0f).OnComplete(() => Destroy(projectile.gameObject));
        projectile.transform.DOScale(0.1f, 1.1f);

    }
}
