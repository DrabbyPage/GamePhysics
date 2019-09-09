using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator
{
    public static Vector2 GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector2 worldUp)
    {
        // f = mg = ma
        Vector2 f_gravity = particleMass * gravitationalConstant * worldUp;
        return f_gravity;
    }
}
