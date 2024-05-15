using CodeMonkey.Utils;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shockwave", menuName = "Projectiles/Shockwave Behaviour")]
public class ShockwaveProjectile : ProjectileBehaviour, IProjectileTrigger
{
    public float RecoilMultiplier = 3.0f;

    public void HitObject(IHittable hittable, IDamageSource source, ProjectileScript.ProjectileParameter projectileParameter)
    {
        source.RecoilMultiplier += RecoilMultiplier;
        hittable.TryHit(source, (int)projectileParameter.damage);
    }

    public override void LaunchProjectile(ProjectileScript projectile, ProjectileScript.ProjectileParameter projectileParameters)
    {
        projectile.Oscillate(projectileParameters.dir);
        /*projectile.transform.DOMove(destination, 5.0f).SetEase(Ease.InCubic).OnComplete(() => ProjectileEnd(projectile,projectileParameters));
        projectile.transform.DOPunchPosition(projectile.transform.right * 3, 2.5f,2).SetLoops(-1, LoopType.Yoyo);*/
        
        //Execute the end projectile method after the end of the projectile's duration
        FunctionTimer.Create(() => ProjectileEnd(projectile, projectileParameters), projectileParameters.duration);
    }


    public override void ProjectileEnd(ProjectileScript projectile, ProjectileScript.ProjectileParameter projectileParameter)
    {
        //Only destroyed at the end
        if (projectile == null) return;

        Destroy(projectile.gameObject);
    }
}
