using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using DG.Tweening;

[CreateAssetMenu(fileName = "Drone", menuName = "Projectiles/Drone Behaviour")]
public class DroneProjectile : ProjectileBehaviour, IProjectileTrigger
{
    public void HitObject(IHittable hittable,IDamageSource source, ProjectileScript.ProjectileParameter projectileParameter)
    {
        hittable.TryHit(source, (int)projectileParameter.damage);
        //Hit feedback ?
        source.Transform.DOPunchRotation(Vector3.forward * 120.0f, .8f).SetTarget(source);
    }

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

        projectile.transform.DOPunchPosition(projectile.transform.up,1.0f).SetTarget(projectile).OnComplete(() => { DOTween.Kill(projectile); Destroy(projectile.gameObject); });
        projectile.transform.DOScale(0.1f, 1.1f).SetTarget(projectile);

    }
}
