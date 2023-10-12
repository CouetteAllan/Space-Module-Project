using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    public void TryHit(IDamageSource source);
}

public interface IDamageSource
{

}
