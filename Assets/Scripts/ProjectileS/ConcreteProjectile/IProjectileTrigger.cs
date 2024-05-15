using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileTrigger
{
    public void HitObject(IHittable hittable, IDamageSource source, ProjectileScript.ProjectileParameter projectileParameter);
}
