using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extend
{
    private const float dotThreshold = 0.5f;

    public static bool isFacingTarget(this Transform transfrom, Transform target)
    {
        var vectorToTarget = target.position - transfrom.position;
        vectorToTarget.Normalize();

        var dot = Vector3.Dot(transfrom.forward, vectorToTarget);
        return dot >= dotThreshold;
    }
}
