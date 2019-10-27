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
public class MadeQuaternion
{
    public Vector4 quat;

    MadeQuaternion()
    {

    }

    MadeQuaternion(float w, float eulerX, float eulerY, float eulerZ)
    {
        // https://math.stackexchange.com/questions/2975109/how-to-convert-euler-angles-to-quaternions-and-get-the-same-euler-angles-back-fr
        /*
             (yaw, pitch, roll) = (r[0], r[1], r[2])
              qx = np.sin(roll/2) * np.cos(pitch/2) * np.cos(yaw/2) - np.cos(roll/2) * np.sin(pitch/2) * np.sin(yaw/2)
              qy = np.cos(roll/2) * np.sin(pitch/2) * np.cos(yaw/2) + np.sin(roll/2) * np.cos(pitch/2) * np.sin(yaw/2)
              qz = np.cos(roll/2) * np.cos(pitch/2) * np.sin(yaw/2) - np.sin(roll/2) * np.sin(pitch/2) * np.cos(yaw/2)
              qw = np.cos(roll/2) * np.cos(pitch/2) * np.cos(yaw/2) + np.sin(roll/2) * np.sin(pitch/2) * np.sin(yaw/2)
              return [qx, qy, qz, qw]
         */

        float xTheta = eulerX/2 * Mathf.Deg2Rad;
        float yTheta = eulerX/2 * Mathf.Deg2Rad;
        float zTheta = eulerX/2 * Mathf.Deg2Rad;

        float xSinTheta = Mathf.Sin(xTheta);
        float xCosTheta = Mathf.Cos(xTheta);

        float ySinTheta = Mathf.Sin(yTheta);
        float yCosTheta = Mathf.Cos(yTheta);

        float zSinTheta = Mathf.Sin(zTheta);
        float zCosTheta = Mathf.Cos(zTheta);

        quat.x = xSinTheta * yCosTheta * zCosTheta - xCosTheta * ySinTheta * zSinTheta;
        quat.y = xCosTheta * ySinTheta * zCosTheta + xSinTheta * yCosTheta * zSinTheta;
        quat.z = xCosTheta * yCosTheta * zSinTheta - xSinTheta * ySinTheta * zCosTheta;
        quat.w = xCosTheta * yCosTheta * zCosTheta + xSinTheta * ySinTheta * zSinTheta;
    }

    public MadeQuaternion EulerToQuat(float eulerX, float eulerY, float eulerZ)
    {
        MadeQuaternion newQuat = new MadeQuaternion();

        // https://math.stackexchange.com/questions/2975109/how-to-convert-euler-angles-to-quaternions-and-get-the-same-euler-angles-back-fr
        /*
             (yaw, pitch, roll) = (r[0], r[1], r[2])
              qx = np.sin(roll/2) * np.cos(pitch/2) * np.cos(yaw/2) - np.cos(roll/2) * np.sin(pitch/2) * np.sin(yaw/2)
              qy = np.cos(roll/2) * np.sin(pitch/2) * np.cos(yaw/2) + np.sin(roll/2) * np.cos(pitch/2) * np.sin(yaw/2)
              qz = np.cos(roll/2) * np.cos(pitch/2) * np.sin(yaw/2) - np.sin(roll/2) * np.sin(pitch/2) * np.cos(yaw/2)
              qw = np.cos(roll/2) * np.cos(pitch/2) * np.cos(yaw/2) + np.sin(roll/2) * np.sin(pitch/2) * np.sin(yaw/2)
              return [qx, qy, qz, qw]
         */

        float xTheta = eulerX / 2 * Mathf.Deg2Rad;
        float yTheta = eulerX / 2 * Mathf.Deg2Rad;
        float zTheta = eulerX / 2 * Mathf.Deg2Rad;

        float xSinTheta = Mathf.Sin(xTheta);
        float xCosTheta = Mathf.Cos(xTheta);

        float ySinTheta = Mathf.Sin(yTheta);
        float yCosTheta = Mathf.Cos(yTheta);

        float zSinTheta = Mathf.Sin(zTheta);
        float zCosTheta = Mathf.Cos(zTheta);

        newQuat.quat.x = xSinTheta * yCosTheta * zCosTheta - xCosTheta * ySinTheta * zSinTheta;
        newQuat.quat.y = xCosTheta * ySinTheta * zCosTheta + xSinTheta * yCosTheta * zSinTheta;
        newQuat.quat.z = xCosTheta * yCosTheta * zSinTheta - xSinTheta * ySinTheta * zCosTheta;
        newQuat.quat.w = xCosTheta * yCosTheta * zCosTheta + xSinTheta * ySinTheta * zSinTheta;

        return newQuat;
    }

    public MadeQuaternion EulerToQuat(Vector3 vec3Euler)
    {
        MadeQuaternion newQuat = new MadeQuaternion();

        // https://math.stackexchange.com/questions/2975109/how-to-convert-euler-angles-to-quaternions-and-get-the-same-euler-angles-back-fr
        /*
             (yaw, pitch, roll) = (r[0], r[1], r[2])
              qx = np.sin(roll/2) * np.cos(pitch/2) * np.cos(yaw/2) - np.cos(roll/2) * np.sin(pitch/2) * np.sin(yaw/2)
              qy = np.cos(roll/2) * np.sin(pitch/2) * np.cos(yaw/2) + np.sin(roll/2) * np.cos(pitch/2) * np.sin(yaw/2)
              qz = np.cos(roll/2) * np.cos(pitch/2) * np.sin(yaw/2) - np.sin(roll/2) * np.sin(pitch/2) * np.cos(yaw/2)
              qw = np.cos(roll/2) * np.cos(pitch/2) * np.cos(yaw/2) + np.sin(roll/2) * np.sin(pitch/2) * np.sin(yaw/2)
              return [qx, qy, qz, qw]
         */

        float xTheta = vec3Euler.x / 2 * Mathf.Deg2Rad;
        float yTheta = vec3Euler.y / 2 * Mathf.Deg2Rad;
        float zTheta = vec3Euler.z / 2 * Mathf.Deg2Rad;

        float xSinTheta = Mathf.Sin(xTheta);
        float xCosTheta = Mathf.Cos(xTheta);

        float ySinTheta = Mathf.Sin(yTheta);
        float yCosTheta = Mathf.Cos(yTheta);

        float zSinTheta = Mathf.Sin(zTheta);
        float zCosTheta = Mathf.Cos(zTheta);

        newQuat.quat.x = xSinTheta * yCosTheta * zCosTheta - xCosTheta * ySinTheta * zSinTheta;
        newQuat.quat.y = xCosTheta * ySinTheta * zCosTheta + xSinTheta * yCosTheta * zSinTheta;
        newQuat.quat.z = xCosTheta * yCosTheta * zSinTheta - xSinTheta * ySinTheta * zCosTheta;
        newQuat.quat.w = xCosTheta * yCosTheta * zCosTheta + xSinTheta * ySinTheta * zSinTheta;

        return newQuat;
    }


    public Vector3 MadeQuatToEuler()
    {
        // https://math.stackexchange.com/questions/2975109/how-to-convert-euler-angles-to-quaternions-and-get-the-same-euler-angles-back-fr

        /*
         (x, y, z, w) = (q[0], q[1], q[2], q[3])
          t0 = +2.0 * (w * x + y * z)
          t1 = +1.0 - 2.0 * (x * x + y * y)
          roll = math.atan2(t0, t1)
          t2 = +2.0 * (w * y - z * x)
          t2 = +1.0 if t2 > +1.0 else t2
          t2 = -1.0 if t2 < -1.0 else t2
          pitch = math.asin(t2)
          t3 = +2.0 * (w * z + x * y)
          t4 = +1.0 - 2.0 * (y * y + z * z)
          yaw = math.atan2(t3, t4)
          return [yaw, pitch, roll] 
        */

        Vector3 angles = Vector3.zero;

        float t0 = 2 * (quat.w * quat.x + quat.y * quat.z);
        float t1 = 1 - 2 * (quat.x * quat.x + quat.y * quat.y);

        float eulerX = Mathf.Atan2(t0, t1);

        float t2 = 2 * (quat.w * quat.y - quat.z * quat.x);

        if(t2 > 1)
        {
            t2 = 1;
        }

        if( t2 < -1)
        {
            t2 = -1;
        }

        float eulerY = Mathf.Asin(t2);

        float t3 = 2 * (quat.w * quat.z + quat.x * quat.y);
        float t4 = 1 - 2 * (quat.y * quat.y + quat.z * quat.z);

        float eulerZ = Mathf.Atan2(t3, t4);

        angles = new Vector3(eulerX, eulerY, eulerZ);

        return angles;
    }

    public Vector4 normalized()
    {
        return quat.normalized;
    }

    public float magnitude()
    {
        float mag = 0;

        mag = Mathf.Sqrt(quat.w * quat.w + quat.x * quat.x + quat.y * quat.y + quat.z * quat.z);

        return mag;
    }

    public float DotProduct(MadeQuaternion otherQuat)
    {
        float product = 0;
        product = quat.w * otherQuat.quat.w + quat.x * otherQuat.quat.x + quat.y * otherQuat.quat.y + quat.z * otherQuat.quat.z;
        return product;
    }

    // maybe need to work on????
    public Vector4 Inverse()
    {
        Vector4 inverse = Vector4.zero;

        Vector4 qStar = new Vector4(-quat.x, -quat.y, -quat.z, quat.w);

        //Vector4 qDenominator = (1 - quat.x - quat.y - quat.z) * 0.5f;

        return inverse;
    }

    public static MadeQuaternion operator*(MadeQuaternion myQuat, MadeQuaternion otherQuat)
    {
        MadeQuaternion newQuat = new MadeQuaternion();

        float myQuatW = myQuat.quat.w;
        float otherQuatW = otherQuat.quat.w;

        Vector3 myQuatXYZ = new Vector3(myQuat.quat.x, myQuat.quat.y, myQuat.quat.z);
        Vector3 otherQuatXYZ = new Vector3(otherQuat.quat.x, otherQuat.quat.y, otherQuat.quat.z);

        float newW = myQuatW * otherQuatW - Vector3.Dot(myQuatXYZ, otherQuatXYZ);

        Vector3 newQuatXYZ = myQuatW * otherQuatXYZ + otherQuatW * myQuatXYZ + Vector3.Cross(myQuatXYZ, otherQuatXYZ);

        newQuat.quat = new Vector4(newQuatXYZ.x, newQuatXYZ.y, newQuatXYZ.z, newW);

        return newQuat;
    }

    public static Vector4 operator*(MadeQuaternion myQuat, Vector3 otherVector)
    {
        Vector4 newVector = Vector4.zero;

        float myQuatW = myQuat.quat.w;

        Vector3 myQuatXYZ = new Vector3(myQuat.quat.x, myQuat.quat.y, myQuat.quat.z);
        Vector3 otherXYZ = new Vector3(otherVector.x, otherVector.y, otherVector.z);

        newVector = Vector3.Cross(otherXYZ + 2 * myQuatXYZ, Vector3.Cross(myQuatXYZ, otherXYZ) + myQuatW * otherXYZ); 

        return newVector;
    }

    public static MadeQuaternion operator*(MadeQuaternion myQuat, float scalar)
    {
        MadeQuaternion newQuat = new MadeQuaternion();

        newQuat.quat = scalar * myQuat.quat;

        return newQuat;
    }
}

[System.Serializable]
public class Particle3DTransform
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    // needs to be a quaternion
    public MadeQuaternion rotation;
    public Vector3 eulerAngle;

    // need to work on
    /*
        Rotation is a quaternion (from a single float about the Z-axis in 2D)
        Angular velocity, angular acceleration and torque are 3D vectors (from single floats about the Z axis in 2D)
     */
    public Vector3 angularVelocity;
    public Vector3 angularAcceleration;

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
    public Vector3 torque;

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
        

        particle3DTransform.eulerAngle += particle3DTransform.angularVelocity * dt;
        particle3DTransform.rotation.EulerToQuat(particle3DTransform.eulerAngle);
        particle3DTransform.angularVelocity += particle3DTransform.angularAcceleration * dt;
    }
    
    void UpdateRotationKinematic(float dt)
    {
        particle3DTransform.eulerAngle += particle3DTransform.angularVelocity * dt + 0.5f * particle3DTransform.angularAcceleration * dt * dt;
        particle3DTransform.rotation = particle3DTransform.rotation.EulerToQuat(particle3DTransform.eulerAngle);
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
                //UpdateRotationEulerExplicit(Time.deltaTime);
                break;
            case RotationType.kinematic:
                //UpdateRotationKinematic(Time.deltaTime);
                break;
        }

        UpdateForce();

        UpdateAcceleration();

        //apply to transform
        transform.position = particle3DTransform.position;
        //transform.eulerAngles = new Vector3(0, 0, rotation);
        //transform.rotation = Quaternion.Euler(0, 0, particle3DTransform.rotation);
        // lab 1 Step 4
        //test
        //acceleration.x = -Mathf.Sin(Time.fixedTime);
        //angularAcceleration = -Mathf.Sin(Time.fixedTime);
        // hard code all scenarios for the forces

        UpdateForce();

        UpdateAngAcc();

        torqueContainer.pointOfAppliedForce = (Vector3)(transform.position + Vector3.up);

        ApplyTorque(torqueContainer.pointOfAppliedForce, torqueContainer.angForce);

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
        torqueContainer.torque = Vector3.zero;
    }

    void ApplyTorque(Vector3 forcePos, Vector3 newForce)
    {
        // The torque is calculated according to the formula, τ= rxF. 
        // This means that the cross product of the distance vector and the force vector gives the resultant.
        Vector3 momentArm = forcePos - particle3DTransform.position;

        torqueContainer.torque = Vector3.Cross(momentArm, newForce);
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

    public void SetAngularVelocity(Vector3 newVel)
    {
        particle3DTransform.angularVelocity = newVel;
    }

    public void SetAngularAcceleration(Vector3 newAcc)
    {
        particle3DTransform.angularAcceleration = newAcc;
    }

    public void ResetObj()
    {
        transform.position = new Vector3(0, 0, 0);
        particle3DTransform.position = new Vector3(0, 0, 0);
        particle3DTransform.rotation.quat = new Vector4(0, 0, 0, 1);
        particle3DTransform.velocity = new Vector3(0, 0, 0);
        particle3DTransform.acceleration = new Vector3(0, 0, 0);
        particle3DTransform.angularVelocity = new Vector3(0, 0, 0);
        particle3DTransform.angularAcceleration = new Vector3(0, 0, 0);
    }
    #endregion

}
