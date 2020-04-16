using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector3 GetMidpoint(this Vector3 a, Vector3 b)
    {
        return (a + b) * 0.5f;
    }
}
