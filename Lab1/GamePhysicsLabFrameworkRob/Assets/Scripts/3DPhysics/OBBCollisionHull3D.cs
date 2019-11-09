using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBBCollisionHull3D : CollisionHull3D
{
    public OBBCollisionHull3D() : base(HULLTYPE.hull_obb) { }

    public Vector3 rectCenter;

    public float length;

    public float width;

    public float height;

    // Start is called before the first frame update
    void Start()
    {
        rectCenter = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rectCenter = transform.position;
    }

    public override bool TestCollisionVSSphere(SphereCollisionHull3D other, ref Collision c)
    {
        return other.TestCollisionVSOBB3D(this, ref c);
    }

    public override bool TestCollisionVSAABB3D(AABBCollisionHull3D other, ref Collision c)
    {
        // positions of box a and box b
        Vector3 PA = transform.position;
        Vector3 PB = other.transform.position;

        // box A's axis
        Vector3 Ax = transform.right;
        Vector3 Ay = transform.up;
        Vector3 Az = transform.forward;

        // length width height of box a
        float WA = length ;
        float DA = width ;
        float HA = height ;

        // box b's axis
        Vector3 Bx = other.transform.right;
        Vector3 By = other.transform.up;
        Vector3 Bz = other.transform.forward;

        // length width height of box b
        float WB = other.length ;
        float DB = other.width ;
        float HB = other.height ;

        Vector3 T = PB - PA;

        float Rxx = Vector3.Dot(Ax, Bx);
        float Rxy = Vector3.Dot(Ax, By);
        float Rxz = Vector3.Dot(Ax, Bz);

        float Ryx = Vector3.Dot(Ay, Bx);
        float Ryy = Vector3.Dot(Ay, By);
        float Ryz = Vector3.Dot(Ay, Bz);

        float Rzx = Vector3.Dot(Az, Bx);
        float Rzy = Vector3.Dot(Az, By);
        float Rzz = Vector3.Dot(Az, Bz);

        // case 1,2,3
        if (SeparateAxis(T, Ax, WB, Rxx, HB, Rxy, DB, Rxz, WA))
        {
            Debug.Log("failed case 1");
            return false;
        }
        if (SeparateAxis(T, Ay, WB, Ryx, HB, Ryy, DB, Ryz, HA))
        {
            Debug.Log("failed case 2");
            return false;
        }
        if (SeparateAxis(T, Az, WB, Rzx, HB, Rzy, DB, Rzz, DA))
        {
            Debug.Log("failed case 3");
            return false;
        }

        // case 4,5,6
        if (SeparateAxis(T, Bx, WA, Rxx, HA, Ryx, DA, Rzx, WB))
        {
            Debug.Log("failed case 4");
            return false;
        }
        if (SeparateAxis(T, By, WA, Rxy, HA, Ryy, DA, Rzy, HB))
        {
            Debug.Log("failed case 5");
            return false;
        }
        if (SeparateAxis(T, Bz, WA, Rxz, HA, Ryz, DA, Rzz, DB))
        {
            Debug.Log("failed case 6");
            return false;
        }

        return true;
    }
    public override bool TestCollisionVSOBB3D(OBBCollisionHull3D other, ref Collision c)
    {
        
        Debug.Log("testing obb");

        // https://www.jkh.me/files/tutorials/Separating%20Axis%20Theorem%20for%20Oriented%20Bounding%20Boxes.pdf
        /*
            Let A and B be oriented bounding boxes (OBB).
            Parameters
            PA = coordinate position of the center of A
            Ax = unit vector representing the x-axis of A
            Ay = unit vector representing the y-axis of A
            Az = unit vector representing the z-axis of A
            WA = half width of A (corresponds with the local x-axis of A)
            HA = half height of A (corresponds with the local y-axis of A)
            DA = half depth of A (corresponds with the local z-axis of A)

            PB = coordinate position of the center of B
            Bx = unit vector representing the x-axis of B
            By = unit vector representing the y-axis of B
            Bz = unit vector representing the z-axis of B
            LB = half width of B (corresponds with the local x-axis of B)
            HB = half height of B (corresponds with the local y-axis of B)
            WB = half depth of B (corresponds with the local z-axis of B)
        */

        /*
            Variables
            T = PB – PA
            Rij = Ai •Bj
        */

        // positions of box a and box b
        Vector3 PA = transform.position;
        Vector3 PB = other.transform.position;

        // box A's axis
        Vector3 Ax = transform.right;
        Vector3 Ay = transform.up;
        Vector3 Az = transform.forward;

        // length width height of box a
        float WA = length * 0.5f;
        float DA = width * 0.5f;
        float HA = height * 0.5f;

        // box b's axis
        Vector3 Bx = other.transform.right;
        Vector3 By = other.transform.up;
        Vector3 Bz = other.transform.forward;

        // length width height of box b
        float WB = other.length * 0.5f;
        float DB = other.width * 0.5f;
        float HB = other.height * 0.5f;

        Vector3 T = PB - PA;

        float Rxx = Vector3.Dot(Ax, Bx);
        float Rxy = Vector3.Dot(Ax, By);
        float Rxz = Vector3.Dot(Ax, Bz);

        float Ryx = Vector3.Dot(Ay, Bx);
        float Ryy = Vector3.Dot(Ay, By);
        float Ryz = Vector3.Dot(Ay, Bz);

        float Rzx = Vector3.Dot(Az, Bx);
        float Rzy = Vector3.Dot(Az, By);
        float Rzz = Vector3.Dot(Az, Bz);

        // case 1,2,3
        if (SeparateAxis(T, Ax, WB, Rxx, HB, Rxy, DB, Rxz, WA))
        {
            Debug.Log("failed case 1");
            return false;
        }
        if (SeparateAxis(T, Ay, WB, Ryx, HB, Ryy, DB, Ryz, HA))
        {
            Debug.Log("failed case 2");
            return false;
        }
        if (SeparateAxis(T, Az, WB, Rzx, HB, Rzy, DB, Rzz, DA))
        {
            Debug.Log("failed case 3");
            return false;
        }

        // case 4,5,6
        if (SeparateAxis(T, Bx, WA, Rxx, HA, Ryx, DA, Rzx, WB))
        {
            Debug.Log("failed case 4");
            return false;
        }
        if (SeparateAxis(T, By, WA, Rxy, HA, Ryy, DA, Rzy, HB))
        {
            Debug.Log("failed case 5");
            return false;
        }
        if (SeparateAxis(T, Bz, WA, Rxz, HA, Ryz, DA, Rzz, DB))
        {
            Debug.Log("failed case 6");
            return false;
        }

        // case 7,8,9
        if (SeparateAxisCrossAB(T, Az, Ryx, Ay, Rzx, HA, Rzx, DA, Ryx, HB, Rxz, DB, Rxy))
        {
            Debug.Log("failed case 7");
            return false;
        }
        if (SeparateAxisCrossAB(T, Az, Ryy, Ay, Rzy, HA, Rzy, DA, Ryy, WB, Rxz, DB, Rxx))
        {
            Debug.Log("failed case 8");
            return false;
        }
        if (SeparateAxisCrossAB(T, Az, Ryz, Ay, Rzz, HA, Rzz, DA, Ryz, WB, Rxy, HB, Rxx))
        {
            Debug.Log("failed case 9");
            return false;
        }

        // case 10,11,12
        if (SeparateAxisCrossAB(T, Ax, Rzx, Az, Rxx, WA, Rzx, DA, Rxx, HB, Ryz, DB, Ryy))
        {
            Debug.Log("failed case 10");
            return false;
        }
        if (SeparateAxisCrossAB(T, Ax, Rzy, Az, Rxy, WA, Rzy, DA, Rxy, WB, Ryz, DB, Ryx))
        {
            Debug.Log("failed case 11");
            return false;
        }
        if (SeparateAxisCrossAB(T, Ax, Rzz, Az, Rxz, WA, Rzz, DA, Rxz, WB, Ryy, HB, Ryx))
        {
            Debug.Log("failed case 12");
            return false;
        }

        // case 13,14,15
        if (SeparateAxisCrossAB(T, Ay, Rxx, Ax, Ryx, WA, Ryx, HA, Rxx, HB, Rzz, DB, Rzy))
        {
            Debug.Log("failed case 13");
            return false;
        }
        if (SeparateAxisCrossAB(T, Ay, Rxy, Ax, Ryy, WA, Ryy, HA, Rxy, WB, Rzz, DB, Rzx))
        {
            Debug.Log("failed case 14");
            return false;
        }
        if (SeparateAxisCrossAB(T, Ay, Rxz, Ax, Ryz, WA, Ryz, HA, Rxz, WB, Rzy, HB, Rzx))
        {
            Debug.Log("failed case 15");
            return false;
        }
        return true;
    }

    bool SeparateAxis(Vector3 T, Vector3 comparedAxis, float lwh1, float Rij1, float lwh2, float Rij2, float lwh3, float Rij3, float lwhBySelf)
    {
        return Vector3.Dot(T, comparedAxis) > Mathf.Abs(lwh1 * Rij1) + Mathf.Abs(lwh2 * Rij2) + Mathf.Abs(lwh2 * Rij2) + lwhBySelf;
    }

    // T = PB-PA, compared axis (think Ax, Bx)
    bool SeparateAxisCrossAB(Vector3 T, Vector3 comparedAxis1, float Rij1, Vector3 comparedAxis2, float Rij2,
                             float lwh1, float lwhRi1, float lwh2, float lwhRi2, float lwh3, float lwhRi3, float lwh4, float lwhRi4)
    {
        /*
        Let A and B be oriented bounding boxes (OBB).
        Parameters
        PA = coordinate position of the center of A
        Ax = unit vector representing the x-axis of A
        Ay = unit vector representing the y-axis of A
        Az = unit vector representing the z-axis of A
        WA = half width of A (corresponds with the local x-axis of A)
        HA = half height of A (corresponds with the local y-axis of A)
        DA = half depth of A (corresponds with the local z-axis of A)
        PB = coordinate position of the center of B
        Bx = unit vector representing the x-axis of B
        By = unit vector representing the y-axis of B
        Bz = unit vector representing the z-axis of B
        W = half width of B (corresponds with the local x-axis of B)
        H = half height of B (corresponds with the local y-axis of B)
        D = half depth of B (corresponds with the local z-axis of B)
        */

        /*
            Variables
            T = PB – PA
            Rij = Ai •Bj
         */

        return (Mathf.Abs(Vector3.Dot(T, comparedAxis1) * Rij1 - Vector3.Dot(T, comparedAxis2) * Rij2) >
                Mathf.Abs(lwh1 * lwhRi1) + Mathf.Abs(lwh2 * lwhRi2) + Mathf.Abs(lwh3 * lwhRi3) + Mathf.Abs(lwh4 * lwhRi4));
    }
}
