using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PositionType
{
    kinematic = 0,
    euler = 1
}

public enum RotationType
{
    kinematic = 0,
    euler = 1
}

public enum TorqueType
{
    disk = 0,
    ring = 1,
    rect = 2,
    rod = 3
}

[System.Serializable]
public class Particle3DTransform
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public float rotation;

    // need this to be vector 3
    public float angularVelocity;
    public float angularAcceleration;

    public PositionType typeOfPositioning;
    public RotationType typeOfRotation;
}

[System.Serializable]
public class Particle3DForces
{
    public float startingMass;
    public float mass;
    public float massInv;

    [SerializeField]
    public bool generateGravity = false;
    public bool generateNormal = false;
    public bool generateSliding = false;
    public bool generateStaticsFriction = false;
    public bool generateKineticFriction = false;
    public bool generateDrag = false;
    public bool generateSpring = false;

    public Vector3 surfaceNormal_unit = new Vector3(0, 1, 0);

    public Vector3 frictionOpposingForce = new Vector3(0, 0, 0);
    public float frictionCoeff_static = 0.5f;

    public float frictionCoeff_kinetic = 0.5f;

    public Vector3 fluidVelocity = new Vector3(0, 0, 0);
    public float fluidDensity = 1.0f; // for water it's 1.0 (this changes based on the temperature)
    public float objArea_CrossSection = 3.0f;
    public float objDragCoeff = 0.5f;

    public Vector3 anchorPos = new Vector3(0, 0, 0);
    public float springRestingLength = 2.0f;
    public float springStiffnesCoeff = 5.0f;

    public Vector3 basicForce;
}

[System.Serializable]
public class Torque
{
    // Lab 3 step 2
    // need this to be vector 3
    public float torque;

    public Vector3 pointOfAppliedForce = (Vector3)Vector3.up;
    public Vector3 angForce;
    public Vector3 force;

    public float momentOfInertia;
    public float invInertia;
    public TorqueType objType;


    public float diskRadius;
    public float ringOuterRadius;
    public float ringInnerRadius;
    public float rectHeight;
    public float rectWidth;
    public float rodLength;
}

// slide number 54 
// normalize the reulst at the end of quaternion

public class Particle3D : MonoBehaviour
{
    public CollisionHull2D colHull;
    //public List<Particle2D> otherColParticleList;

    [SerializeField]
    Particle3DTransform particle3DTransform;

    // lab2 step 1
    [SerializeField]
    Particle3DForces forces;

    [SerializeField]
    Torque torqueContainer;



    public void SetMass(float newMass)
    {
        //mass = newMass > 0.0f ? newMass: 0.0f;
        forces.mass = Mathf.Max(0.0f, newMass);
        forces.massInv = forces.mass > 0 ? 1.0f / forces.mass : 0.0f;
    }

    public float GetMass()
    {
        return forces.mass;
    }

    public float GetInvMass()
    {
        return forces.massInv;
    }

    // lab 2 setp 2

    public void AddForce(Vector3 newForce)
    {
        // D'Alembert
        torqueContainer.force += newForce;
    }

    public void AddRotationForce(Vector3 newRotForce)
    {
        torqueContainer.angForce += newRotForce;
    }

    void UpdateAcceleration()
    {
        // Newton 2
        particle3DTransform.acceleration = forces.massInv * torqueContainer.force;
        //Debug.Log("Force: " + force);

        torqueContainer.force.Set(0.0f, 0.0f, 0.0f);
    }

    // Lab 1 Step 2
    void UpdatePositionEulerExplicit(float dt)
    {
        //x(t+dt) = x(t) + v(t)dt
        //Euler's Method
        //F(t+dt) = F(t) + f(t)dt
        //               + (dF/dt)dt
        particle3DTransform.position += particle3DTransform.velocity * dt;

        //v(t+dt) = v(t) + a(t)dt
        particle3DTransform.velocity += particle3DTransform.acceleration * dt;
    }

    void UpdatePositionKinematic(float dt)
    {
        particle3DTransform.position += particle3DTransform.velocity * dt + 0.5f * particle3DTransform.acceleration * dt * dt;
        particle3DTransform.velocity += particle3DTransform.acceleration * dt;
    }

    void UpdateRotationEulerExplicit(float dt)
    {
        particle3DTransform.rotation += particle3DTransform.angularVelocity * dt;
        particle3DTransform.angularVelocity += particle3DTransform.angularAcceleration * dt;
    }

    void UpdateRotationKinematic(float dt)
    {
        particle3DTransform.rotation += particle3DTransform.angularVelocity * dt + 0.5f * particle3DTransform.angularAcceleration * dt * dt;
        particle3DTransform.angularVelocity += particle3DTransform.angularAcceleration * dt;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMass(forces.startingMass);

        particle3DTransform.position = transform.position;

        UpdateInertia();

        colHull = this.gameObject.GetComponent<CollisionHull2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(particle3DTransform.typeOfPositioning)
        {
            case PositionType.euler:
                UpdatePositionEulerExplicit(Time.deltaTime);
                break;
            case PositionType.kinematic:
                UpdatePositionKinematic(Time.deltaTime);
                break;
        }

        switch(particle3DTransform.typeOfRotation)
        {
            case RotationType.euler:
                UpdateRotationEulerExplicit(Time.deltaTime);
                break;
            case RotationType.kinematic:
                UpdateRotationKinematic(Time.deltaTime);
                break;
        }

        UpdateForce();

        UpdateAcceleration();

        //apply to transform
        transform.position = particle3DTransform.position;
        //transform.eulerAngles = new Vector3(0, 0, rotation);
        transform.rotation = Quaternion.Euler(0, 0, particle3DTransform.rotation);
        // lab 1 Step 4
        //test
        //acceleration.x = -Mathf.Sin(Time.fixedTime);
        //angularAcceleration = -Mathf.Sin(Time.fixedTime);
        // hard code all scenarios for the forces

        UpdateForce();

        UpdateAngAcc();

        torqueContainer.pointOfAppliedForce = (Vector3)(transform.position + Vector3.up);

        ApplyTorque(torqueContainer.pointOfAppliedForce, torqueContainer.angForce);

        particle3DTransform.rotation = particle3DTransform.rotation % 360;
    }

    void UpdateForce()
    {
        // Lab 2 Step 3
        //f_gravity = f = mg = ma
        Vector3 f_gravity = forces.mass * new Vector3(0.0f, -9.8f);

        Vector3 f_normal = ForceGenerator.GenerateForce_Normal(f_gravity, transform.up);

        // AddForce(f_gravity); // works
        if (forces.generateGravity)
            AddForce(ForceGenerator.GenerateForce_Gravity(forces.mass, -9.8f, Vector3.up));

        if (forces.generateNormal)
            AddForce(ForceGenerator.GenerateForce_Normal(f_gravity, forces.surfaceNormal_unit)); // works? more testing (surface normal is 0, 1)

        if (forces.generateSliding)
            AddForce(ForceGenerator.GenerateForce_Sliding(f_gravity, f_normal));  // (surface normal is 0,1)

        if (forces.generateStaticsFriction)
            AddForce(ForceGenerator.GenerateForce_Friction_Static(f_normal, forces.frictionOpposingForce, forces.frictionCoeff_static)); // works (surface normal is 1,1) FOF = (-3,0) FCS = 0.9

        if (forces.generateKineticFriction)
            AddForce(ForceGenerator.GenerateForce_Friction_Kinetic(f_normal, particle3DTransform.velocity, forces.frictionCoeff_kinetic));  // works surface = (1,1) initVel = 15 FCK = 0.3

        if (forces.generateDrag)
            AddForce(ForceGenerator.GenerateForce_Drag(particle3DTransform.velocity, forces.fluidVelocity, forces.fluidDensity, forces.objArea_CrossSection, forces.objDragCoeff));  // not sure if this works ask dan... IV = 1, FV = 1, FD = 1, OACS = 1.5, ODC=1.05

        if (forces.generateSpring && particle3DTransform.position.magnitude != 0)
            AddForce(ForceGenerator.GenerateForce_Spring(particle3DTransform.position, forces.anchorPos, forces.springRestingLength, forces.springStiffnesCoeff)); // pos = 0,100 , AP = 0,0 , SRL = 0.1, SSC = 3 , fricCoKin = 0.15 (turn on gravity and kin fric

        AddForce(forces.basicForce);
    }


    void UpdateInertia()
    {
        switch (torqueContainer.objType)
        {
            case TorqueType.disk: // disk
                torqueContainer.momentOfInertia = DiskInertia(torqueContainer.diskRadius);
                break;
            case TorqueType.ring: // ring
                torqueContainer.momentOfInertia = RingInertia(torqueContainer.ringOuterRadius, torqueContainer.ringInnerRadius);
                break;
            case TorqueType.rect: // rect
                torqueContainer.momentOfInertia = RectangleInertia(torqueContainer.rectHeight, torqueContainer.rectWidth);
                break;
            case TorqueType.rod: // rod
                torqueContainer.momentOfInertia = RodInertia(torqueContainer.rodLength);
                break;
        }

        torqueContainer.invInertia = 1 / torqueContainer.momentOfInertia;
    }

    void UpdateAngAcc()
    {
        //Debug.Log(momentOfInertia);
        particle3DTransform.angularAcceleration = torqueContainer.torque * torqueContainer.invInertia;
        torqueContainer.torque = 0;
    }

    void ApplyTorque(Vector3 forcePos, Vector3 newForce)
    {

        Vector3 momentArm = forcePos - particle3DTransform.position;

        torqueContainer.torque += (momentArm.x * newForce.y - momentArm.y * newForce.x);
    }

    #region Inertia Functions

    float DiskInertia(float diskRadius)
    {
        float inertia = 0f;
        inertia = 0.5f * forces.mass * diskRadius * diskRadius;
        return inertia;
    }

    float RingInertia(float ringOuterRadius, float ringInnerRadius)
    {
        float inertia = 0f;
        inertia = 0.5f * forces.mass * (ringOuterRadius * ringOuterRadius + ringInnerRadius * ringInnerRadius);
        return inertia;
    }

    float RectangleInertia(float rectHeight, float rectWidth)
    {
        float inertia = 0f;
        inertia = (1f / 12f) * forces.mass * (rectHeight * rectHeight + rectWidth * rectWidth);
        return inertia;
    }

    float RodInertia(float rodLength)
    {
        float inertia = 0f;
        inertia = (1f / 12f) * forces.mass * rodLength * rodLength;
        return inertia;
    }

    #endregion

    #region manipulators
    public void SetPositionX(float newX)
    {
        particle3DTransform.position.x = newX;
    }

    public void SetPositionY(float newY)
    {
        particle3DTransform.position.y = newY;
    }

    public void SetVelocityX(float newVel)
    {
        particle3DTransform.velocity.x = newVel;
    }

    public void SetVelocityY(float newVel)
    {
        particle3DTransform.velocity.y = newVel;
    }

    public void SetAccelerationX(float newAcc)
    {
        particle3DTransform.acceleration.x = newAcc;
    }

    public void SetAccelerationY(float newAcc)
    {
        particle3DTransform.acceleration.y = newAcc;
    }

    public void SetAngularVelocity(float newVel)
    {
        particle3DTransform.angularVelocity = newVel;
    }

    public void SetAngularAcceleration(float newAcc)
    {
        particle3DTransform.angularAcceleration = newAcc;
    }

    public void ResetObj()
    {
        transform.position = new Vector3(0, 0, 0);
        particle3DTransform.position = new Vector3(0, 0);
        particle3DTransform.rotation = 0;
        particle3DTransform.velocity = new Vector3(0, 0);
        particle3DTransform.acceleration = new Vector3(0, 0);
        particle3DTransform.angularVelocity = 0;
        particle3DTransform.angularAcceleration = 0;
    }
    #endregion

}
