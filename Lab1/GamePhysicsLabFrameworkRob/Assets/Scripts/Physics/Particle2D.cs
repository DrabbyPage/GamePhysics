using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    // Lab 1 Step 1
    public Vector2 position, velocity, acceleration;
    public float rotation, angularVelocity, angularAcceleration;

    bool particleKinPos, particleKinRot;

    // lab2 step 1
    public float startingMass;
    float mass, massInv;

    GameObject UIMan;

    public Vector2 surfaceNormal_unit = new Vector2(0, 1);

    public Vector2 frictionOpposingForce = new Vector2(0, 0);
    public float frictionCoeff_static = 0.5f;

    public float frictionCoeff_kinetic = 0.5f;

    public Vector2 fluidVelocity = new Vector2(0, 0);
    public float fluidDensity = 1.0f; // for water it's 1.0 (this changes based on the temperature)
    public float objArea_CrossSection = 3.0f;
    public float objDragCoeff = 0.5f;
    
    public Vector2 anchorPos = new Vector2(0, 0);
    public float springRestingLength = 2.0f;
    public float springStiffnesCoeff = 5.0f;

    public void SetMass(float newMass)
    {
        //mass = newMass > 0.0f ? newMass: 0.0f;
        mass = Mathf.Max(0.0f, newMass);
        massInv = mass > 0 ? 1.0f / mass : 0.0f;
    }

    public float GetMass()
    {
        return mass;
    }

    // lab 2 setp 2
    Vector2 force;
    public void AddForce(Vector2 newForce)
    {
        // D'Alembert
        force += newForce;
    }

    void UpdateAcceleration()
    {
        // Newton 2
        acceleration = massInv * force;

        force.Set(0.0f, 0.0f);
    }

    // Lab 1 Step 2
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
        SetMass(startingMass);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // particleKinPos = UIMan.GetComponent<UIManagerScript>().kinPos;
        // particleKinRot = UIMan.GetComponent<UIManagerScript>().kinRot;

        // lab 1 step 3
        //integrate
        //if (particleKinPos)
        //{
            UpdatePositionKinematic(Time.fixedDeltaTime);
        // }
        // else
        // {
        //     UpdatePositionEulerExplicit(Time.fixedDeltaTime);
        // }

        // if(particleKinRot)
        // {
            UpdateRotationKinematic(Time.fixedDeltaTime);
        // }
        // else
        // {
        //     UpdateRotationEulerExplicit(Time.fixedDeltaTime);
        // }

        UpdateAcceleration();

        //apply to transform
        transform.position = position;
        transform.eulerAngles = new Vector3(0, 0, rotation);

        // lab 1 Step 4
        //test
        //acceleration.x = -Mathf.Sin(Time.fixedTime);
        //angularAcceleration = -Mathf.Sin(Time.fixedTime);
        
        // hard code all scenarios for the forces
        UpdateForce();

    }

    void UpdateForce()
    {
        // Lab 2 Step 3
        //f_gravity = f = mg = ma
        Vector2 f_gravity = mass * new Vector2(0.0f, -9.8f);

        Vector2 f_normal = ForceGenerator.GenerateForce_Normal(f_gravity, surfaceNormal_unit);

        // AddForce(f_gravity);
        // AddForce(ForceGenerator.GenerateForce_Gravity(mass, -9.8f, Vector2.up));
        // AddForce(ForceGenerator.GenerateForce_Normal(f_gravity, new Vector2(0, 1)));
        // AddForce(ForceGenerator.GenerateForce_Sliding(f_gravity, f_normal));          
        // AddForce(ForceGenerator.GenerateForce_Friction_Static(f_normal, frictionOpposingForce, frictionCoeff_static));   
        // AddForce(ForceGenerator.GenerateForce_Friction_Kinetic(f_normal, velocity, frictionCoeff_kinetic));  
        // AddForce(ForceGenerator.GenerateForce_Drag(velocity, fluidVelocity, fluidDensity, objArea_CrossSection, objDragCoeff));     
        //AddForce(ForceGenerator.GenerateForce_Spring(position, anchorPos, springRestingLength, springStiffnesCoeff));

    }


    #region manipulators
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

    public void ResetObj()
    {
        transform.position = new Vector3(0, 0, 0);
        position = new Vector2(0,0);
        rotation = 0;
        velocity = new Vector2(0, 0);
        acceleration = new Vector2(0, 0);
        angularVelocity = 0;
        angularAcceleration = 0;
    }
    #endregion
}
