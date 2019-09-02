using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    //Step 1
    public Vector2 position, velocity, acceleration;
    public float rotation, angularVelocity, angularAcceleration;

    //Step 2
    void UpdatePositionEulerExplicit(float dt)
    {
        //x(t+dt) = x(t) + v(t)dt
        //Euler's Method
        //F(t+dt) = F(t) + f(t)dt
        //               + (dF/dt)dt
        position += velocity * dt;

        //v(t+dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
    }

    void UpdatePositionKinematic(float dt)
    {
        position += velocity * dt + 0.5f * acceleration * dt * dt;
        velocity += acceleration * dt;
    }

    void UpdateRotationEulerExplicit(float dt)
    {
        rotation += angularVelocity * dt;
        angularVelocity += angularAcceleration * dt;
    }

    void UpdateRotationKinematic(float dt)
    {
        rotation += angularVelocity * dt + 0.5f * angularAcceleration * dt * dt;
        angularVelocity += angularAcceleration * dt;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //step 3
        //integrate
        UpdatePositionKinematic(Time.fixedDeltaTime);
        UpdateRotationKinematic(Time.fixedDeltaTime);
        //apply to transform
        transform.position = position;
        transform.eulerAngles = new Vector3(rotation, rotation, rotation);

        //Step 4
        //test
        acceleration.x = -Mathf.Sin(Time.fixedTime);
        angularAcceleration = 10;//-Mathf.Sin(Time.fixedTime);
    }
}
