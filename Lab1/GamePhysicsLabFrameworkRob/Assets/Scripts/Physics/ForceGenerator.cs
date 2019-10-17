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
        Vector2 f_normal = Vector3.Project(-f_gravity, surfaceNormal_unit);
       
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
        Vector2 f_friction_s = new Vector2(0, 0);

        float max = frictionCoefficient_static * f_normal.magnitude;

        float opposing = f_opposing.magnitude;

        if (f_opposing.magnitude < max)
        {
            f_friction_s = -f_opposing;
        }
        else
        {
            f_friction_s = -f_opposing * max;
        }

        return f_friction_s;
    }

    public static Vector2 GenerateForce_Friction_Kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic)
    {
        // f_friction_k = -coeff*|f_normal| * unit(vel)
        Vector2 f_friction_k;

        f_friction_k = -frictionCoefficient_kinetic * f_normal.magnitude * particleVelocity.normalized;

        return f_friction_k;
    }

    public static Vector2 GenerateForce_Drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        // f_drag = (p * u^2 * area * coeff)/2
        Vector2 f_drag = new Vector2(0, 0);

        Vector2 velDiff = particleVelocity - fluidVelocity;
        float velDiffMag = velDiff.magnitude;

        f_drag = objectDragCoefficient * (fluidDensity * (velDiff) * (velDiffMag) * 0.5f) * objectArea_crossSection;

        //Debug.Log("Drag Force: " + f_drag);
        return f_drag;
    }

    public static Vector2 GenerateForce_Spring(Vector2 particlePosition, Vector2 anchorPosition, float springrestingLength, float springStiffnessCoefficent)
    {
        // f_spring = -coeff*(spring length - spring resting length)
        Vector2 springLengthDirection = particlePosition - anchorPosition;

        float actualSpringLength = springLengthDirection.magnitude;

        // Vector2 f_spring = -springStiffnessCoefficent * (springCurrentLength - springrestingLength);
        Vector2 f_spring =  springLengthDirection * springStiffnessCoefficent * (springrestingLength  - actualSpringLength) / actualSpringLength;

        return f_spring;
    }

}
