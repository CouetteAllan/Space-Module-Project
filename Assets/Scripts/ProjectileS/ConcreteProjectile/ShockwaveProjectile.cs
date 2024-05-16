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
        var rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = projectileParameters.dir * projectileParameters.speed;
        projectile.transform.DOBlendableMoveBy(-projectile.transform.right * 4.0f, .1f).SetEase(Ease.OutSine);
        projectile.transform.DOBlendableMoveBy(projectile.transform.right * 9, .3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetTarget(projectile.gameObject).SetDelay(.15f);
        
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
