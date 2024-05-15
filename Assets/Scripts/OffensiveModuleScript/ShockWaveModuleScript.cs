using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShockwaveStrategy",menuName = "Module/Strategy/ShockwaveCanon")]
public class ShockWaveModuleScript : BaseOffensiveScript
{
    public float ShockwaveSpeed = 16.0f;
    public override void Fire(bool firstProjectile, Quaternion currentRotation, Vector3 currentModulePosition, Transform[] projectilePositions, out bool success)
    {
        success = true;
    }
}
