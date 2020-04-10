using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class ResponseCurve
{
    public static float Exponentional(float x, float power=2)
    {
        return math.clamp(math.pow(x, power), 0f, 1f);
    }

    public static float Linear(float x, float slope=1)
    {
        return math.clamp(x * slope, 0f, 1f);
    }

    public static float Decay(float t, float mag)
    {
        return math.clamp(math.pow(mag, t), 0f, 1f);
    }

    public static float Sigmoid(float t, float k)
    {
        return math.clamp(k * t / (k - t + 1), 0f, 1f);
    }

    public static float RaiseFastToSlow(float t, float k = 4)
    {
        return math.clamp(-math.pow(t-1, k) + 1, 0, 1f);
    }
}
