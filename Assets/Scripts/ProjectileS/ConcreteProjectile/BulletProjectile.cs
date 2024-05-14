using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet",menuName = "Projectiles/Bullet Behaviour")]
public class BulletProjectile : ProjectileBehaviour
{
    public override void LaunchProjectile(GameObject projectileGO,ProjectileScript.ProjectileParameter projectileParameter)
    {
        var rb = projectileGO.GetComponent<Rigidbody2D>();
        rb.velocity = projectileParameter.dir * projectileParameter.speed;

    }

    public override void ProjectileEnd(GameObject projectileGO, ProjectileScript.ProjectileParameter projectileParameter)
    {
        //Fade bullet
    }
}
