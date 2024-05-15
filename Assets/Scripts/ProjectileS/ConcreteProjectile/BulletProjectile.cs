using Codice.CM.Common;
using System.Collections;
using CodeMonkey.Utils;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "Bullet",menuName = "Projectiles/Bullet Behaviour")]
public class BulletProjectile : ProjectileBehaviour
{
    public override void LaunchProjectile(GameObject projectileGO,ProjectileScript.ProjectileParameter projectileParameter)
    {
        var rb = projectileGO.GetComponent<Rigidbody2D>();
        rb.velocity = projectileParameter.dir * projectileParameter.speed;
        FunctionTimer.Create(() => ProjectileEnd(projectileGO,projectileParameter), projectileParameter.duration);
    }

    public override void ProjectileEnd(GameObject projectileGO, ProjectileScript.ProjectileParameter projectileParameter)
    {
        if (projectileGO == null)
            return;

        //Fade bullet
        projectileGO.transform.DOScale(4.0f, .3f).SetTarget(projectileGO).OnComplete(() => Destroy(projectileGO));
        var spriteRenderer = projectileGO.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in spriteRenderer)
        {
            sprite.DOFade(0,.3f).SetEase(Ease.Linear);
        }

    }
}
