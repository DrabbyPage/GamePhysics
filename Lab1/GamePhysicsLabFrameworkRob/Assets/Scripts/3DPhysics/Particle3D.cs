using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.Reflection;



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
    SolidSphere = 0,
    HollowSphere = 1,
    SolidBox = 2,
    HollowBox = 3,
    SolidCylinder = 4,
    SolidCone = 5
}

#region mat 4
public class MadeMatrix4x4
{
    public List<float> matrix = new List<float>();
    public List<float> invMatrix = new List<float>();

    public void ListMatrix()
    {
        string printMsg = "";
        for(int i = 0; i < matrix.Count;i++)
        {
            if (i % 4 == 0 && i != 0)
            {
                printMsg += "\n";
            }

            printMsg += matrix[i].ToString() + " ";

        }
        Debug.Log(printMsg);
    }

    public void ListInverseMatrix()
    {
        string printMsg = "";
        for (int i = 0; i < invMatrix.Count; i++)
        {

            printMsg += invMatrix[i].ToString() + " ";


        }
        Debug.Log(printMsg);
    }


    public MadeMatrix4x4 zero()
    {
        MadeMatrix4x4 zero = new MadeMatrix4x4(
                                                0, 0, 0, 0,
                                                0, 0, 0, 0,
                                                0, 0, 0, 0,
                                                0, 0, 0, 0
                                                );
        return zero;
    }

    public MadeMatrix4x4()
    {
        for(int i = 0; i < 16; i++)
        {
            matrix.Add(0);
        }

        for (int i = 0; i < 16; i++)
        {
            invMatrix.Add(0);
        }
    }

    public MadeMatrix4x4(float a, float b, float c, float d,
                         float e, float f, float g, float h,
                         float i, float j, float k, float l,
                         float m, float n, float o, float p)
    {
        matrix.Add(a);
        matrix.Add(b);
        matrix.Add(c);
        matrix.Add(d);
        matrix.Add(e);
        matrix.Add(f);
        matrix.Add(g);
        matrix.Add(h);
        matrix.Add(i);
        matrix.Add(j);
        matrix.Add(k);
        matrix.Add(l);
        matrix.Add(m);
        matrix.Add(n);
        matrix.Add(o);
        matrix.Add(p);

        for (int q = 0; q < 16; q++)
        {
            invMatrix.Add(0);
        }
    }

    public void SetColumn(int colIndex, Vector4 col)
    {
        switch (colIndex)
        {
            case 0:
                matrix[0] = col.x;
                matrix[1] = col.y;
                matrix[2] = col.z;
                matrix[3] = col.w;
                break;
            case 1:
                matrix[4] = col.x;
                matrix[5] = col.y;
                matrix[6] = col.z;
                matrix[7] = col.w;
                break;
            case 2:
                matrix[8] = col.x;
                matrix[9] = col.y;
                matrix[10] = col.z;
                matrix[11] = col.w;
                break;
            case 3:
                matrix[12] = col.x;
                matrix[13] = col.y;
                matrix[14] = col.z;
                matrix[15] = col.w;
                break;
        }
    }

    public float calculateDeterminate()
    {
        float newDet = matrix[8] * matrix[5] * matrix[2] +
                       matrix[4] * matrix[9] * matrix[2] +
                       matrix[8] * matrix[1] * matrix[6] -
                       matrix[0] * matrix[9] * matrix[6] -
                       matrix[4] * matrix[1] * matrix[10] +
                       matrix[0] * matrix[5] * matrix[10];

        return newDet;
    }

    public void calculateInv()
    {
        float mat11 = matrix[0];
        float mat12 = matrix[1];
        float mat13 = matrix[2];
        float mat14 = matrix[3];
        float mat21 = matrix[4];
        float mat22 = matrix[5];
        float mat23 = matrix[6];
        float mat24 = matrix[7];
        float mat31 = matrix[8];
        float mat32 = matrix[9];
        float mat33 = matrix[10];
        float mat34 = matrix[11];
        float mat41 = matrix[12];
        float mat42 = matrix[13];
        float mat43 = matrix[14];
        float mat44 = matrix[15];

        invMatrix[0] = mat22 * mat33 * mat44 + mat23 * mat34 * mat42 + mat24 * mat32 * mat43 - mat24 * mat33 * mat42 - mat23 * mat32 * mat44 - mat22 * mat34 * mat43;
        invMatrix[1] = -mat12 * mat33 * mat44 - mat13 * mat34 * mat42 - mat14 * mat32 * mat43 + mat14 * mat33 * mat42 + mat13 * mat32 * mat44 + mat12 * mat34 * mat43;
        invMatrix[2] = mat12 * mat23 * mat44 + mat13 * mat24 * mat42 + mat14 * mat22 * mat43 - mat14 * mat23 * mat42 - mat13 * mat22 * mat44 - mat12 * mat24 * mat43;
        invMatrix[3] = -mat12 * mat23 * mat34 - mat13 * mat24 * mat32 - mat14 * mat22 * mat33 + mat14 * mat23 * mat32 + mat13 * mat22 * mat34 + mat12 * mat24 * mat33;

        invMatrix[4] = -mat21 * mat33 * mat44 - mat23 * mat34 * mat41 - mat24 * mat31 * mat43 + mat24 * mat33 * mat41 + mat23 * mat31 * mat44 + mat21 * mat34 * mat43;
        invMatrix[5] = mat11 * mat33 * mat44 + mat13 * mat34 * mat41 + mat14 * mat31 * mat43 - mat14 * mat33 * mat41 - mat13 * mat31 * mat44 - mat11 * mat34 * mat43;
        invMatrix[6] = -mat11 * mat23 * mat44 - mat13 * mat24 * mat41 - mat14 * mat21 * mat43 + mat14 * mat23 * mat41 + mat13 * mat21 * mat44 + mat11 * mat24 * mat43;
        invMatrix[7] = mat11 * mat23 * mat34 + mat13 * mat24 * mat31 + mat14 * mat21 * mat33 - mat14 * mat23 * mat31 - mat13 * mat21 * mat34 - mat11 * mat24 * mat33;

        invMatrix[8] = mat21 * mat32 * mat44 + mat22 * mat34 * mat41 + mat24 * mat31 * mat42 - mat24 * mat32 * mat41 - mat22 * mat31 * mat44 - mat21 * mat34 * mat42;
        invMatrix[9] = -mat11 * mat32 * mat44 - mat12 * mat34 * mat41 - mat14 * mat31 * mat42 + mat14 * mat32 * mat41 + mat12 * mat31 * mat44 + mat11 * mat34 * mat42;
        invMatrix[10] = mat11 * mat22 * mat44 + mat12 * mat24 * mat41 + mat14 * mat21 * mat42 - mat14 * mat22 * mat41 - mat12 * mat21 * mat44 - mat11 * mat24 * mat42;
        invMatrix[11] = -mat11 * mat22 * mat34 - mat12 * mat24 * mat31 - mat14 * mat21 * mat32 + mat14 * mat22 * mat31 + mat12 * mat21 * mat34 + mat11 * mat24 * mat32;

        invMatrix[12] = -mat21 * mat32 * mat43 - mat22 * mat33 * mat41 - mat23 * mat31 * mat42 + mat23 * mat32 * mat41 + mat22 * mat31 * mat43 + mat21 * mat33 * mat42;
        invMatrix[13] = mat11 * mat32 * mat43 + mat12 * mat33 * mat41 + mat13 * mat31 * mat42 - mat13 * mat32 * mat41 - mat12 * mat31 * mat43 - mat11 * mat33 * mat43;
        invMatrix[14] = -mat11 * mat22 * mat43 - mat12 * mat23 * mat41 - mat13 * mat21 * mat42 + mat13 * mat22 * mat41 + mat12 * mat21 * mat43 + mat11 * mat23 * mat42;
        invMatrix[15] = mat11 * mat22 * mat33 + mat12 * mat23 * mat31 + mat13 * mat21 * mat32 - mat13 * mat22 * mat31 - mat12 * mat21 * mat33 - mat11 * mat23 * mat32;

        float det = calculateDeterminate();
        for (int i = 0; i < invMatrix.Count; i++)
        {
            invMatrix[i] = (1/det) * invMatrix[i];
        }
    }

    public static MadeMatrix4x4 operator *(MadeMatrix4x4 myQuad, MadeMatrix4x4 otherQuad)
    {
        MadeMatrix4x4 newMat = new MadeMatrix4x4();
        /*
          0, 1, 2, 3,
          4, 5, 6, 7,
          8, 9,10,11,
         12,13,14,15
         */

        /*
         00,01,02,03
         10,11,12,13
         20,21,22,23
         30,31,32,33
         */

        #region left quad vals
        float leftQuad00 = myQuad.matrix[0];
        float leftQuad01 = myQuad.matrix[1];
        float leftQuad02 = myQuad.matrix[2];
        float leftQuad03 = myQuad.matrix[3];
                              
        float leftQuad10 = myQuad.matrix[4];
        float leftQuad11 = myQuad.matrix[5];
        float leftQuad12 = myQuad.matrix[6];
        float leftQuad13 = myQuad.matrix[7];
                               
        float leftQuad20 = myQuad.matrix[8];
        float leftQuad21 = myQuad.matrix[9];
        float leftQuad22 = myQuad.matrix[10];
        float leftQuad23 = myQuad.matrix[11];
                               
        float leftQuad30 = myQuad.matrix[12];
        float leftQuad31 = myQuad.matrix[13];
        float leftQuad32 = myQuad.matrix[14];
        float leftQuad33 = myQuad.matrix[15];

        #endregion

        #region right quad vals
        float rightQuad00 = otherQuad.matrix[0];
        float rightQuad01 = otherQuad.matrix[1];
        float rightQuad02 = otherQuad.matrix[2];
        float rightQuad03 = otherQuad.matrix[3];

        float rightQuad10 = otherQuad.matrix[4];
        float rightQuad11 = otherQuad.matrix[5];
        float rightQuad12 = otherQuad.matrix[6];
        float rightQuad13 = otherQuad.matrix[7];

        float rightQuad20 = otherQuad.matrix[8];
        float rightQuad21 = otherQuad.matrix[9];
        float rightQuad22 = otherQuad.matrix[10];
        float rightQuad23 = otherQuad.matrix[11];

        float rightQuad30 = otherQuad.matrix[12];
        float rightQuad31 = otherQuad.matrix[13];
        float rightQuad32 = otherQuad.matrix[14];
        float rightQuad33 = otherQuad.matrix[15];
        #endregion

        newMat.matrix[0] = leftQuad00 * rightQuad00 + leftQuad01 * rightQuad10 + leftQuad02 * rightQuad20 + leftQuad03 * rightQuad30;
        newMat.matrix[1] = leftQuad00 * rightQuad01 + leftQuad01 * rightQuad11 + leftQuad02 * rightQuad21 + leftQuad03 * rightQuad31;
        newMat.matrix[2] = leftQuad00 * rightQuad02 + leftQuad01 * rightQuad12 + leftQuad02 * rightQuad22 + leftQuad03 * rightQuad32;
        newMat.matrix[3] = leftQuad00 * rightQuad03 + leftQuad01 * rightQuad13 + leftQuad02 * rightQuad23 + leftQuad03 * rightQuad33;

        newMat.matrix[4] = leftQuad10 * rightQuad00 + leftQuad11 * rightQuad10 + leftQuad12 * rightQuad20 + leftQuad13 * rightQuad30;
        newMat.matrix[5] = leftQuad10 * rightQuad01 + leftQuad11 * rightQuad11 + leftQuad12 * rightQuad21 + leftQuad13 * rightQuad31;
        newMat.matrix[6] = leftQuad10 * rightQuad02 + leftQuad11 * rightQuad12 + leftQuad12 * rightQuad22 + leftQuad13 * rightQuad32;
        newMat.matrix[7] = leftQuad10 * rightQuad03 + leftQuad11 * rightQuad13 + leftQuad12 * rightQuad23 + leftQuad13 * rightQuad33;

        newMat.matrix[8] = leftQuad20 * rightQuad00 + leftQuad21 * rightQuad10 + leftQuad22 * rightQuad20 + leftQuad23 * rightQuad30;
        newMat.matrix[9] = leftQuad20 * rightQuad01 + leftQuad21 * rightQuad11 + leftQuad22 * rightQuad21 + leftQuad23 * rightQuad31;
        newMat.matrix[10] = leftQuad20 * rightQuad02 + leftQuad21 * rightQuad12 + leftQuad22 * rightQuad22 + leftQuad23 * rightQuad32;
        newMat.matrix[11] = leftQuad20 * rightQuad03 + leftQuad21 * rightQuad13 + leftQuad22 * rightQuad23 + leftQuad23 * rightQuad33;

        newMat.matrix[12] = leftQuad30 * rightQuad00 + leftQuad31 * rightQuad10 + leftQuad32 * rightQuad20 + leftQuad33 * rightQuad30;
        newMat.matrix[13] = leftQuad30 * rightQuad01 + leftQuad31 * rightQuad11 + leftQuad32 * rightQuad21 + leftQuad33 * rightQuad31;
        newMat.matrix[14] = leftQuad30 * rightQuad02 + leftQuad31 * rightQuad12 + leftQuad32 * rightQuad22 + leftQuad33 * rightQuad32;
        newMat.matrix[15] = leftQuad30 * rightQuad03 + leftQuad31 * rightQuad13 + leftQuad32 * rightQuad23 + leftQuad33 * rightQuad33;

        return newMat;
    }

    public static Vector4 operator *(Vector4 myVec4, MadeMatrix4x4 otherQuad)
    {
        /*
         0, 1, 2, 3,
         4, 5, 6, 7,
         8, 9,10,11,
         12,13,14,15
        */

        /*
         00,01,02,03
         10,11,12,13
         20,21,22,23
         30,31,32,33
         */

        #region left quad vals
        float otherQuad00 = otherQuad.matrix[0];
        float otherQuad01 = otherQuad.matrix[1];
        float otherQuad02 = otherQuad.matrix[2];
        float otherQuad03 = otherQuad.matrix[3];
                       
        float otherQuad10 = otherQuad.matrix[4];
        float otherQuad11 = otherQuad.matrix[5];
        float otherQuad12 = otherQuad.matrix[6];
        float otherQuad13 = otherQuad.matrix[7];
                       
        float otherQuad20 = otherQuad.matrix[8];
        float otherQuad21 = otherQuad.matrix[9];
        float otherQuad22 = otherQuad.matrix[10];
        float otherQuad23 = otherQuad.matrix[11];
                       
        float otherQuad30 = otherQuad.matrix[12];
        float otherQuad31 = otherQuad.matrix[13];
        float otherQuad32 = otherQuad.matrix[14];
        float otherQuad33 = otherQuad.matrix[15];

        #endregion

        Vector4 newVec4 = Vector4.zero;

        newVec4.x = myVec4.x * otherQuad00 + myVec4.y * otherQuad10 + myVec4.z * otherQuad20 + myVec4.w * otherQuad30;
        newVec4.y = myVec4.x * otherQuad01 + myVec4.y * otherQuad11 + myVec4.z * otherQuad21 + myVec4.w * otherQuad31;
        newVec4.z = myVec4.x * otherQuad02 + myVec4.y * otherQuad12 + myVec4.z * otherQuad22 + myVec4.w * otherQuad32;
        newVec4.w = myVec4.x * otherQuad03 + myVec4.y * otherQuad13 + myVec4.z * otherQuad23 + myVec4.w * otherQuad33;

        return newVec4;
    }

}
#endregion

#region Quaternions
[System.Serializable]
public class MadeQuaternion
{
    public Vector4 quat;

    public MadeQuaternion()
    {
        quat = new Vector4(0, 0, 0, 1);
    }

    public MadeQuaternion(float eulerX, float eulerY, float eulerZ)
    {
        /*
             (yaw, pitch, roll) = (r[0], r[1], r[2])
              qx = sin(roll/2) * cos(pitch/2) * cos(yaw/2) - cos(roll/2) * sin(pitch/2) * sin(yaw/2)
              qy = cos(roll/2) * sin(pitch/2) * cos(yaw/2) + sin(roll/2) * cos(pitch/2) * sin(yaw/2)
              qz = cos(roll/2) * cos(pitch/2) * sin(yaw/2) - sin(roll/2) * sin(pitch/2) * cos(yaw/2)
              qw = cos(roll/2) * cos(pitch/2) * cos(yaw/2) + sin(roll/2) * sin(pitch/2) * sin(yaw/2)
              return [qx, qy, qz, qw]
         */

        float xTheta = eulerX*0.5f * Mathf.Deg2Rad;
        float yTheta = eulerY*0.5f * Mathf.Deg2Rad;
        float zTheta = eulerZ*0.5f * Mathf.Deg2Rad;

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

        /*
             (yaw, pitch, roll) = (r[0], r[1], r[2])
              qx = sin(roll/2) * cos(pitch/2) * cos(yaw/2) - cos(roll/2) * sin(pitch/2) * sin(yaw/2)
              qy = cos(roll/2) * sin(pitch/2) * cos(yaw/2) + sin(roll/2) * cos(pitch/2) * sin(yaw/2)
              qz = cos(roll/2) * cos(pitch/2) * sin(yaw/2) - sin(roll/2) * sin(pitch/2) * cos(yaw/2)
              qw = cos(roll/2) * cos(pitch/2) * cos(yaw/2) + sin(roll/2) * sin(pitch/2) * sin(yaw/2)
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

        /*
             (yaw, pitch, roll) = (r[0], r[1], r[2])
              qx = sin(roll/2) * cos(pitch/2) * cos(yaw/2) - cos(roll/2) * sin(pitch/2) * sin(yaw/2)
              qy = cos(roll/2) * sin(pitch/2) * cos(yaw/2) + sin(roll/2) * cos(pitch/2) * sin(yaw/2)
              qz = cos(roll/2) * cos(pitch/2) * sin(yaw/2) - sin(roll/2) * sin(pitch/2) * cos(yaw/2)
              qw = cos(roll/2) * cos(pitch/2) * cos(yaw/2) + sin(roll/2) * sin(pitch/2) * sin(yaw/2)
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

        float eulerX = Mathf.Atan2(t0, t1) * Mathf.Rad2Deg;

        float t2 = 2 * (quat.w * quat.y - quat.z * quat.x);

        if(t2 > 1)
        {
            t2 = 1;
        }

        if( t2 < -1)
        {
            t2 = -1;
        }

        float eulerY = Mathf.Asin(t2) * Mathf.Rad2Deg;

        float t3 = 2 * (quat.w * quat.z + quat.x * quat.y);
        float t4 = 1 - 2 * (quat.y * quat.y + quat.z * quat.z);

        float eulerZ = Mathf.Atan2(t3, t4) * Mathf.Rad2Deg;

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

    public static Vector3 operator*(MadeQuaternion myQuat, Vector3 otherVector)
    {
        Vector3 newVector = Vector3.zero;

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
#endregion

#region Particle3d transform
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
#endregion

#region 3d forces
[System.Serializable]
public class Particle3DForces
{
    public float startingMass = 1.0f;
    public float mass = 1.0f;
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
#endregion

#region 3d torque
[System.Serializable]
public class Torque
{
    // Lab 3 step 2
    public Vector3 torque;

    public Vector3 pointOfAppliedForce = Vector3.up;
    public Vector3 angForce;
    public Vector3 force;
    
    // new stuff
    public Vector3 localCenterOfMass;
    public Vector3 worldCenterOfMass;

    public MadeMatrix4x4 momentOfInertia = new MadeMatrix4x4();
    public MadeMatrix4x4 invInertia = new MadeMatrix4x4();
    public TorqueType objType;

    public SolidSphereTorque solidSphereTorque;
    public HollowSphereTorque  hollowSphereTorque;
    public SolidBoxTorque solidBoxTorque;
    public HollowBoxTorque hollowBoxTorque;
    public SolidCylinderTorque solidCylinderTorque;
    public SolidConeTorque solidConeTorque;
}

#endregion

#region Torque objects
[System.Serializable]
public struct SolidSphereTorque
{
    public float radius;
}

[System.Serializable]
public struct HollowSphereTorque
{
    public float radius;
}

[System.Serializable]
public struct SolidBoxTorque
{
    public float length;
    public float width;
    public float height;
}

[System.Serializable]
public struct HollowBoxTorque
{
    public float length;
    public float width;
    public float height;
}

[System.Serializable]
public struct SolidCylinderTorque
{
    public float radius;
    public float height;
}

[System.Serializable]
public struct SolidConeTorque
{
    public float radius;
    public float height;
}

#endregion

// slide number 54 
// normalize the reulst at the end of quaternion

public class Particle3D : MonoBehaviour
{
    public CollisionHull3D colHull;
    //public List<Particle2D> otherColParticleList;

    [SerializeField]
    public Particle3DTransform particle3DTransform;

    // lab2 step 1
    [SerializeField]
    public Particle3DForces forces;

    [SerializeField]
    Torque torqueContainer;

    MadeMatrix4x4 worldTransformMatrix;

    MadeMatrix4x4 worldTranformInverseMatrix;

    #region mass
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

    #endregion

    #region add torque forces functions
    public void AddForce(Vector3 newForce)
    {
        // D'Alembert
        torqueContainer.force += newForce;
    }

    public void AddRotationForce(Vector3 newRotForce)
    {
        torqueContainer.angForce += newRotForce;
    }
    #endregion

    void UpdateAcceleration()
    {
        // Newton 2
        particle3DTransform.acceleration = forces.massInv * torqueContainer.force;
        //Debug.Log("Force: " + force);

        torqueContainer.force.Set(0.0f, 0.0f, 0.0f);
    }

    #region position kin and euler
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
    #endregion

    #region Rotation Euler and Kin
    void UpdateRotationEulerExplicit(float dt)
    {
        particle3DTransform.eulerAngle = particle3DTransform.eulerAngle + (dt * particle3DTransform.angularVelocity);
        particle3DTransform.rotation = particle3DTransform.rotation.EulerToQuat(particle3DTransform.eulerAngle);
        particle3DTransform.angularVelocity += particle3DTransform.angularAcceleration * dt;

        particle3DTransform.eulerAngle.x = particle3DTransform.eulerAngle.x % 360;
        particle3DTransform.eulerAngle.y = particle3DTransform.eulerAngle.y % 360;
        particle3DTransform.eulerAngle.z = particle3DTransform.eulerAngle.z % 360;
    }
    
    void UpdateRotationKinematic(float dt)
    {
        particle3DTransform.eulerAngle += particle3DTransform.angularVelocity * dt + 0.5f * particle3DTransform.angularAcceleration * dt * dt;
        particle3DTransform.rotation = particle3DTransform.rotation.EulerToQuat(particle3DTransform.eulerAngle);
        particle3DTransform.angularVelocity += particle3DTransform.angularAcceleration * dt;

        particle3DTransform.eulerAngle.x = particle3DTransform.eulerAngle.x % 360;
        particle3DTransform.eulerAngle.y = particle3DTransform.eulerAngle.y % 360;
        particle3DTransform.eulerAngle.z = particle3DTransform.eulerAngle.z % 360;
    }
    #endregion region

    // Start is called before the first frame update
    void Start()
    {
        worldTransformMatrix = new MadeMatrix4x4(
                                                    1.0f, 0.0f, 0.0f, transform.position.x,
                                                    0.0f, 1.0f, 0.0f, transform.position.y,
                                                    0.0f, 0.0f, 1.0f, transform.position.z,
                                                    0.0f, 0.0f, 0.0f, 1.0f
                                                 );

        worldTransformMatrix.calculateInv();

        worldTranformInverseMatrix = new MadeMatrix4x4();
        worldTranformInverseMatrix.matrix = worldTransformMatrix.invMatrix;

        torqueContainer.worldCenterOfMass = transform.position;
        torqueContainer.localCenterOfMass = Vector3.zero;

        SetMass(forces.startingMass);

        particle3DTransform.position = transform.position;

        Quaternion rot = gameObject.transform.rotation;
        Vector4 quatVals = new Vector4(rot.x, rot.y, rot.z, 1);
        particle3DTransform.rotation = new MadeQuaternion(quatVals.x,quatVals.y,quatVals.z);

        UpdateInertia();

        colHull = this.gameObject.GetComponent<CollisionHull3D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateWorldMatrix();

        torqueContainer.worldCenterOfMass = transform.position;


        switch (particle3DTransform.typeOfPositioning)
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


        float rotX = particle3DTransform.rotation.quat.x;
        float rotY = particle3DTransform.rotation.quat.y;
        float rotZ = particle3DTransform.rotation.quat.z;
        float rotW = particle3DTransform.rotation.quat.w;
        transform.rotation = new Quaternion(rotX, rotY, rotZ, rotW);

        UpdateInertia();

        ApplyTorque(torqueContainer.pointOfAppliedForce, torqueContainer.angForce);

        UpdateAngAcc();


    }

    void UpdateWorldMatrix()
    {
        float cosX = Mathf.Cos(particle3DTransform.eulerAngle.x * Mathf.Deg2Rad);
        float sinX = Mathf.Sin(particle3DTransform.eulerAngle.x * Mathf.Deg2Rad);
        float cosY = Mathf.Cos(particle3DTransform.eulerAngle.y * Mathf.Deg2Rad);
        float sinY = Mathf.Sin(particle3DTransform.eulerAngle.y * Mathf.Deg2Rad);
        float cosZ = Mathf.Cos(particle3DTransform.eulerAngle.z * Mathf.Deg2Rad);
        float sinZ = Mathf.Sin(particle3DTransform.eulerAngle.z * Mathf.Deg2Rad);

        worldTransformMatrix = new MadeMatrix4x4(
                                                    cosY * cosZ, -cosY*sinZ, sinY, transform.position.x,
                                                    sinX*sinY*cosZ+cosX*sinZ, -sinX*sinY*sinZ+cosX*cosZ, -sinX*cosY, transform.position.y,
                                                    -cosX*sinY*cosZ+sinX*sinZ, cosX*sinY*sinZ+sinX*cosZ, cosX*cosY, transform.position.z,
                                                    0.0f, 0.0f, 0.0f, 1.0f
                                                 );

        worldTransformMatrix.calculateInv();

        worldTranformInverseMatrix.matrix = worldTransformMatrix.invMatrix;
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
            case TorqueType.SolidSphere: // 
                torqueContainer.momentOfInertia = SolidSphereTensor(torqueContainer.solidSphereTorque.radius, GetMass());
                //torqueContainer.invInertia = torqueContainer.momentOfInertia.invMatrix;
                break;
            case TorqueType.HollowSphere: // 
                torqueContainer.momentOfInertia = HollowSphereTensor(torqueContainer.hollowSphereTorque.radius, GetMass());
                break;
            case TorqueType.SolidBox: // 
                torqueContainer.momentOfInertia = SolidBoxTensor(torqueContainer.solidBoxTorque.height, torqueContainer.solidBoxTorque.width, torqueContainer.solidBoxTorque.length, GetMass());
                break;
            case TorqueType.HollowBox: // 
                torqueContainer.momentOfInertia = HollowBoxTensor(torqueContainer.hollowBoxTorque.length, torqueContainer.hollowBoxTorque.width, torqueContainer.hollowBoxTorque.height, GetMass());
                break;
            case TorqueType.SolidCylinder:
                torqueContainer.momentOfInertia = SolidCylinderTensor(torqueContainer.solidCylinderTorque.radius, torqueContainer.solidCylinderTorque.height, GetMass());
                break;
            case TorqueType.SolidCone:
                torqueContainer.momentOfInertia = SolidConeTensor(torqueContainer.solidConeTorque.radius, torqueContainer.solidConeTorque.height, GetMass());
                break;
        }
        //torqueContainer.momentOfInertia.ListMatrix();
        torqueContainer.momentOfInertia.calculateInv();


        torqueContainer.invInertia.matrix = torqueContainer.momentOfInertia.invMatrix;

        //torqueContainer.invInertia = 1 / torqueContainer.momentOfInertia;
    }

    void UpdateAngAcc()
    {
        //Debug.Log(torqueContainer.momentOfInertia);
        particle3DTransform.angularAcceleration = torqueContainer.torque * (worldTransformMatrix * torqueContainer.invInertia * worldTranformInverseMatrix);
        torqueContainer.torque = Vector3.zero;
    }

    void ApplyTorque(Vector3 forcePos, Vector3 newForce)
    {
        // The torque is calculated according to the formula, τ= rxF. 
        // This means that the cross product of the distance vector and the force vector gives the resultant.
        Vector3 momentArm = forcePos - particle3DTransform.position;

        torqueContainer.torque = Vector3.Cross(momentArm, newForce);
    }

    #region Torque Tensors

    MadeMatrix4x4 SolidSphereTensor(float radius, float mass)
    {
        float inputVal = 0.4f * mass * radius * radius;

        MadeMatrix4x4 newMat = new MadeMatrix4x4(
                                                    inputVal, 0, 0, 0,
                                                    0, inputVal, 0, 0,
                                                    0, 0, inputVal, 0,
                                                    0, 0, 0, 1
                                                 );

        return newMat;
    }

    // cube: 𝐼 = 1/6 * mass * size^2
    MadeMatrix4x4 HollowSphereTensor(float radius, float mass)
    {
        float inputVal = 0.66f * mass * radius * radius;

        MadeMatrix4x4 newMat = new MadeMatrix4x4(
                                            inputVal, 0, 0, 0,
                                            0, inputVal, 0, 0,
                                            0, 0, inputVal, 0,
                                            0, 0, 0, 1
                                         );

        return newMat;
    }

    MadeMatrix4x4 SolidBoxTensor(float height, float width, float length, float mass)
    {
        float col1Input = 0.083f * mass * (height * height + length * length);
        float col2Input = 0.083f * mass * (length * length + width * width);
        float col3Input = 0.083f * mass * (width * width + height * height);

        MadeMatrix4x4 newMat = new MadeMatrix4x4(
                                            col1Input, 0, 0, 0,
                                            0, col2Input, 0, 0,
                                            0, 0, col3Input, 0,
                                            0, 0, 0, 1
                                         );

        return newMat;
    }

    MadeMatrix4x4 HollowBoxTensor(float length, float width, float height, float mass)
    {

        float col1Input = 1.66f * mass * (height * height + length * length);
        float col2Input = 1.66f * mass * (length * length + width * width);
        float col3Input = 1.66f * mass * (width * width + height * height);

        MadeMatrix4x4 newMat = new MadeMatrix4x4(
                                    col1Input, 0, 0, 0,
                                    0, col2Input, 0, 0,
                                    0, 0, col3Input, 0,
                                    0, 0, 0, 1
                                 );


        return newMat;
    }

    MadeMatrix4x4 SolidCylinderTensor(float radius, float height, float mass)
    {
        float col1Input = 0.083f * mass * (3 * radius * radius + height * height);
        float col2Input = col1Input;
        float col3Input = 0.5f * mass * radius * radius;

        MadeMatrix4x4 newMat = new MadeMatrix4x4(
                                    col1Input, 0, 0, 0,
                                    0, col2Input, 0, 0,
                                    0, 0, col3Input, 0,
                                    0, 0, 0, 1
                                 );


        return newMat;
    }

    // axis parallel to third lacal basis ***FOUND IN THE SLIDES #18***
    MadeMatrix4x4 SolidConeTensor(float radius, float height, float mass)
    {
        float col1Input = 0.6f * mass * height * height + 0.15f * mass * radius * radius;
        float col2Input = col1Input;
        float col3Input = 0.3f * mass * radius * radius;

        MadeMatrix4x4 newMat = new MadeMatrix4x4(
                                    col1Input, 0, 0, 0,
                                    0, col2Input, 0, 0,
                                    0, 0, col3Input, 0,
                                    0, 0, 0, 1
                                 );


        return newMat;
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

    public void SetPositionZ(float newZ)
    {
        particle3DTransform.position.z = newZ;
    }

    public void SetVelocityX(float newVel)
    {
        particle3DTransform.velocity.x = newVel;
    }

    public void SetVelocityY(float newVel)
    {
        particle3DTransform.velocity.y = newVel;
    }

    public void SetVelocityZ(float newVel)
    {
        particle3DTransform.velocity.z = newVel;
    }

    public void SetAccelerationX(float newAcc)
    {
        particle3DTransform.acceleration.x = newAcc;
    }

    public void SetAccelerationY(float newAcc)
    {
        particle3DTransform.acceleration.y = newAcc;
    }

    public void SetAccelerationZ(float newAcc)
    {
        particle3DTransform.acceleration.z = newAcc;
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
