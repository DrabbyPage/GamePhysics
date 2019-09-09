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

    public static Vector2 GenerateForce_Normal(Vector2 f_gravity, Vector2 surfaceNormal_unit)
    {
        //f_normal = proj(f_gravity, surfaceNormalUnit)
        Vector2 f_normal = Vector2.Dot(f_gravity, surfaceNormal_unit);
        return f_normal;
    }

    public static Vector2 GenerateForce_Sliding(Vector2 f_gravity, Vector2 f_normal)
    {
        //f_sliding = f_gravity = f_normal
        Vector2 f_sliding = f_gravity + f_normal;
        return f_sliding;
    }

    public static Vector2 GenerateForce_Friction_Static(Vector2 f_normal, Vector2 f_opposing, float frictionCoefficient_static)
    {
        // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)
        if (-f_opposing < frictionCoefficient_static * f_normal)
        {
        }
        return
    }

    public static Vector2 GenerateForce_Friction_Kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic)
    {
        // f_friction_k = -coeff*|f_normal| * unit(vel)

    }

    public static Vector2 GenerateForce_drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        // f_drag = (p * u^2 * area * coeff)/2

    }

    public static Vector2 generateForce_spring(Vector2 particlePosition, Vector2 anchorPosition, float springrestingLength, float springStiffnessCoefficent)
    {
        // f_spring = -coeff*(spring length - spring resting length)

    }
}
