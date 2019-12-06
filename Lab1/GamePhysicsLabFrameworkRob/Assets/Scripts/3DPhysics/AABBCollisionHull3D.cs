using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBCollisionHull3D : CollisionHull3D
{
    public AABBCollisionHull3D() : base(HULLTYPE.hull_aabb) { }
    public Vector3 rectCenter;
    [SerializeField] public float length;
    [SerializeField] public float width;
    [SerializeField] public float height;

    public Vector3 frontBotLeft;
    public Vector3 frontTopLeft;
    public Vector3 frontBotRight;
    public Vector3 frontTopRight;
     
    public Vector3 backBotLeft;
    public Vector3 backTopLeft;
    public Vector3 backBotRight;
    public Vector3 backTopRight;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePosition();
        c = new Collision();
        particle = GetComponent<Particle3D>();
    }

    void UpdatePosition()
    {
        rectCenter = transform.position;
        frontTopLeft = new Vector3(rectCenter.x - length / 2, rectCenter.y + height / 2, rectCenter.z - width / 2);
        frontBotLeft = new Vector3(rectCenter.x - length / 2, rectCenter.y - height / 2, rectCenter.z - width / 2);
        frontTopRight = new Vector3(rectCenter.x + length / 2, rectCenter.y + height / 2, rectCenter.z - width / 2);
        frontBotRight = new Vector3(rectCenter.x - length / 2, rectCenter.y - height / 2, rectCenter.z - width / 2);

        backTopLeft = new Vector3(rectCenter.x - length / 2, rectCenter.y + height / 2, rectCenter.z + width / 2);
        backBotLeft = new Vector3(rectCenter.x - length / 2, rectCenter.y - height / 2, rectCenter.z + width / 2);
        backTopRight = new Vector3(rectCenter.x + length / 2, rectCenter.y + height / 2, rectCenter.z + width / 2);
        backBotRight = new Vector3(rectCenter.x - length / 2, rectCenter.y - height / 2, rectCenter.z + width / 2);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    public override bool TestCollisionVSSphere(SphereCollisionHull3D other, ref Collision c)
    {
        Debug.Log("This " + this + " other " + other);
        return other.TestCollisionVSAABB3D(this, ref c);
    }

    public override bool TestCollisionVSAABB3D(AABBCollisionHull3D other, ref Collision c)
    {

        // for each dimension, max extent of A greater than min extent of B

        // rectCenter - 1/2(length or width) for min point in x or y
        float thisXMin = rectCenter.x - width * 0.5f;

        // rectCenter + 1/2(length or width) for max point in x or y
        float thisXMax = rectCenter.x + width * 0.5f;

        // rectCenter - 1/2(length or width) for min point in x or y
        float thisYMin = rectCenter.y - height * 0.5f;

        // rectCenter + 1/2(length or width) for max point in x or y
        float thisYMax = rectCenter.y + height * 0.5f;

        float thisZMax = rectCenter.z + width * 0.5f;

        float thisZMin = rectCenter.z - width * 0.5f;

        // rectCenter - 1/2(length or width) for min point in x or y
        float otherXMin = other.rectCenter.x - width * 0.5f;

        // rectCenter + 1/2(length or width) for max point in x or y
        float otherXMax = other.rectCenter.x + width * 0.5f;

        // rectCenter - 1/2(length or width) for min point in x or y
        float otherYMin = other.rectCenter.y - height * 0.5f;

        // rectCenter + 1/2(length or width) for max point in x or y
        float otherYMax = other.rectCenter.y + height * 0.5f;

        float otherZMax = other.rectCenter.z + width * 0.5f;

        float otherZMin = other.rectCenter.z - width * 0.5f;

        // check which min is greater, greater min becomes the one, other becomes other
        // if one max.x < other max.x && one max.x > other min.x
        if (otherXMin <= thisXMax && thisXMax <= otherXMax)
        {
            // if this passes, check same thing with y
            if (otherYMin <= thisYMax && thisYMax <= otherYMax)
            {
                // if this passes, check same thing with z
                if (otherZMin <= thisZMax && thisZMax <= otherZMax)
                {
                    return true;
                }
                else if (otherZMin <= thisZMin && thisZMin <= otherZMax)
                {
                    return true;
                }
            }
            else if (otherYMin <= thisYMin && thisYMin <= otherYMax)
            {
                // if this passes, check same thing with z
                if (otherZMin <= thisZMax && thisZMax <= otherZMax)
                {
                    return true;
                }
                else if (otherZMin <= thisZMin && thisZMin <= otherZMax)
                {
                    return true;
                }
            }
        }

        // check which min is greater, greater min becomes the one, other becomes other
        // if one min.x < other max.x && one min.x > other min.x
        if (otherXMin <= thisXMin && thisXMin <= otherXMax)
        {
            // if this passes, check same thing with y
            if (otherYMin <= thisYMax && thisYMax <= otherYMax)
            {
                // if this passes, check same thing with z
                if (otherZMin <= thisZMax && thisZMax <= otherZMax)
                {
                    return true;
                }
                else if (otherZMin <= thisZMin && thisZMin <= otherZMax)
                {
                    return true;
                }
            }
            else if (otherYMin <= thisYMin && thisYMin <= otherYMax)
            {
                // if this passes, check same thing with z
                if (otherZMin <= thisZMax && thisZMax <= otherZMax)
                {
                    return true;
                }
                else if (otherZMin <= thisZMin && thisZMin <= otherZMax)
                {
                    return true;
                }
            }
        }


        return false;
    }
    public override bool TestCollisionVSOBB3D(OBBCollisionHull3D other, ref Collision c)
    {
        return false;
    }
}
