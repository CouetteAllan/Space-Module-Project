using Codice.CM.Common;
using System.Collections;
using CodeMonkey.Utils;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "Bullet",menuName = "Projectiles/Bullet Behaviour")]
public class BulletProjectile : ProjectileBehaviour
{
    public override void LaunchProjectile(ProjectileScript projectile,ProjectileScript.ProjectileParameter projectileParameter)
    {
        var rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = projectileParameter.dir * projectileParameter.speed;

        //Execute the end projectile method after the end of the projectile's duration
        FunctionTimer.Create(() => ProjectileEnd(projectile,projectileParameter), projectileParameter.duration);
    }

    public override void ProjectileEnd(ProjectileScript projectile, ProjectileScript.ProjectileParameter projectileParameter)
    {
        if (projectile.gameObject == null)
            return;

        //Fade bullet
        projectile.transform.DOScale(4.0f, .3f).SetTarget(projectile).OnComplete(() => Destroy(projectile));
        var spriteRenderer = projectile.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in spriteRenderer)
        {
            sprite.DOFade(0,.3f).SetEase(Ease.Linear);
        }

    }
}
