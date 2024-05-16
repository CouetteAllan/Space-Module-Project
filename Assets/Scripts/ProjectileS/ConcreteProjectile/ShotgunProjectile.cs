using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectileScript;

[CreateAssetMenu(fileName = "Shotgun", menuName = "Projectiles/Shotgun Behaviour")]

public class ShotgunProjectile : ProjectileBehaviour, IProjectileTrigger
{
    public void HitObject(IHittable hittable, IDamageSource source, ProjectileScript.ProjectileParameter projectileParameter)
    {
        source.RecoilMultiplier += RecoilMult;
        hittable.TryHit(source, (int)projectileParameter.damage);
    }

    public override void LaunchProjectile(ProjectileScript projectile, ProjectileScript.ProjectileParameter projectileParameters)
    {
        //Wave grow larger 
        projectile.transform.DOScaleX(4f, projectileParameters.duration - .5f).SetEase(Ease.Linear);


        var rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = projectileParameters.dir * projectileParameters.speed * 2.0f;

        Vector2 velocity = rb.velocity;

        //Wave stop speed proessively after a duration
        DOTween.To(
            () => velocity,
            x => {
                if (rb == null)
                    return;
                velocity = x;
                rb.velocity = velocity;
            }, Vector2.zero, projectileParameters.duration)
            .SetEase(Ease.OutCubic)
            .SetTarget(projectile)
            .OnComplete(() => ProjectileEnd(projectile,projectileParameters));
    }

    public override void ProjectileEnd(ProjectileScript projectile, ProjectileScript.ProjectileParameter projectileParameters)
    {
        if(projectile == null) return;


        projectile.transform.DORotate(Vector3.forward * 1.0f, 0.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).OnComplete(() => Destroy(projectile.gameObject));
        var spriteRenderer = projectile.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in spriteRenderer)
        {
            sprite.DOFade(0, .5f).SetEase(Ease.Linear);
        }
    }
}
