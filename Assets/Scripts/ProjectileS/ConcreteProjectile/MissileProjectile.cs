using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Missile", menuName = "Projectiles/Missile Behaviour")]

public class MissileProjectile : ProjectileBehaviour
{
    public float BlowRadius = 6.0f;
    public float RecoilBoost = 2.0f;
    public ParticleSystem BlowParticles;

    public AudioClip BlowSound;

    public override void LaunchProjectile(ProjectileScript projectile, ProjectileScript.ProjectileParameter projectileParameter)
    {
        //Spawn the missile
        projectile.transform.DOPunchScale(new Vector3(.4f, .4f, .4f), .6f, 3, .8f).SetTarget(projectile);

        //Launch missile at a certain speed then explode after a certain duration;
        var rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = projectileParameter.dir * .6f;
        Vector2 velocity = rb.velocity;


        DOTween.To(
            () => velocity,
            x => {
            if (rb == null)
                return;
            velocity = x;
            rb.velocity = velocity;
        }, projectileParameter.dir * projectileParameter.speed * 2f, .9f)
            .SetEase(Ease.InSine)
            .SetTarget(projectile);


        //Invoke method after a certain amount of time
        
        var sequence = DOTween.Sequence().SetTarget(projectile);
        sequence.AppendInterval(projectileParameter.duration).SetTarget(projectile);
        sequence.AppendCallback(() => ProjectileEnd(projectile,projectileParameter)).SetTarget(projectile);
    }

    public override void ProjectileEnd(ProjectileScript projectile, ProjectileScript.ProjectileParameter projectileParameters)
    {
        if(projectile == null) 
            return;
        MissileBlow(projectile, projectileParameters);
    }

    private void MissileBlow(ProjectileScript projectile, ProjectileScript.ProjectileParameter projectileParameter)
    {
        var colls = Physics2D.OverlapCircleAll(projectile.transform.position, BlowRadius) ;
        foreach (var coll in colls)
        {
            if (coll.TryGetComponent(out IHittable hittable))
            {
                projectile.RecoilMultiplier += RecoilBoost;
                hittable.TryHit(projectile, (int)projectileParameter.damage) ;
            }
        }
        //Play particles
        FXManager.Instance.PlayEffect("rocketBlow", projectile.transform.position, Quaternion.identity);
        Destroy(projectile.gameObject);
    }
}
