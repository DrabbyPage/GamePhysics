using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    //Step 1
    public Vector2 position, velocity, acceleration;
    public float rotation, angularVelocity, angularAcceleration;

    bool particleKinPos, particleKinRot;

    GameObject UIMan;

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
        UIMan = GameObject.Find("UI Manager");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        particleKinPos = UIMan.GetComponent<UIManagerScript>().kinPos;
        particleKinRot = UIMan.GetComponent<UIManagerScript>().kinRot;

        //step 3
        //integrate
        if (particleKinPos)
        {
            UpdatePositionKinematic(Time.fixedDeltaTime);
        }
        else
        {
            UpdatePositionEulerExplicit(Time.fixedDeltaTime);
        }

        if(particleKinRot)
        {
            UpdateRotationKinematic(Time.fixedDeltaTime);
        }
        else
        {
            UpdateRotationEulerExplicit(Time.fixedDeltaTime);
        }

        //apply to transform
        transform.position = position;
        transform.eulerAngles = new Vector3(rotation, rotation, rotation);

        //Step 4
        //test
        //acceleration.x = -Mathf.Sin(Time.fixedTime);
        //angularAcceleration = 10;//-Mathf.Sin(Time.fixedTime);
    }

    public void SetVelocityX(float newVel)
    {
        velocity.x = newVel;
    }

    public void SetVelocityY(float newVel)
    {
        velocity.y = newVel;
    }

    public void SetAccelerationX(float newAcc)
    {
        acceleration.x = newAcc;
    }

    public void SetAccelerationY(float newAcc)
    {
        acceleration.y = newAcc;
    }

    public void SetAngularVelocity(float newVel)
    {
        angularVelocity = newVel;
    }

    public void SetAngularAcceleration(float newAcc)
    {
        angularAcceleration = newAcc;
    }
}
