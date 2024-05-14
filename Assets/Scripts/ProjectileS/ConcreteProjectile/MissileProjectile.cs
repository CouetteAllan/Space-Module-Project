using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Missile", menuName = "Projectiles/Missile Behaviour")]

public class MissileProjectile : ProjectileBehaviour
{
    public float BlowRadius = 6.0f;
    public ParticleSystem BlowParticles;


    public override void LaunchProjectile(GameObject projectileGO, ProjectileScript.ProjectileParameter projectileParameter)
    {
        //Spawn the missile
        projectileGO.transform.DOPunchScale(new Vector3(.4f, .4f, .4f), .6f, 3, .8f);

        //Launch missile at a certain speed then explode after a certain duration;
        var rb = projectileGO.GetComponent<Rigidbody2D>();
        rb.velocity = projectileParameter.dir * .6f;
        Vector2 velocity = rb.velocity;


        DOTween.To(() => velocity, x => { velocity = x; rb.velocity = velocity; },projectileParameter.dir * projectileParameter.speed * 2f, .9f).SetEase(Ease.InSine);


        //Invoke method after a certain amount of time
        
        /*var sequence = DOTween.Sequence();
        sequence.AppendInterval(projectileParameter.duration);
        sequence.AppendCallback(() => ProjectileEnd(projectileGO,projectileParameter));*/
    }

    public override void ProjectileEnd(GameObject projectileGO, ProjectileScript.ProjectileParameter projectileParameters)
    {
        MissileBlow(projectileGO, projectileParameters);
    }

    private void MissileBlow(GameObject projectileGO, ProjectileScript.ProjectileParameter projectileParameter)
    {
        var colls = Physics2D.OverlapCircleAll(projectileGO.transform.position, BlowRadius) ;
        foreach (var coll in colls)
        {
            if (coll.TryGetComponent(out IHittable hittable))
            {
                hittable.TryHit(projectileGO.GetComponent<IDamageSource>(), (int)projectileParameter.damage) ;
            }
        }
        //Play particles
        FXManager.Instance.PlayEffect("rocketBlow", projectileGO.transform.position, Quaternion.identity);
        Destroy(projectileGO);
    }
}
