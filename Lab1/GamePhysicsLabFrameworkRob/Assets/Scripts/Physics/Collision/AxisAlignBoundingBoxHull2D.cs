using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignBoundingBoxHull2D : CollisionHull2D
{
    public AxisAlignBoundingBoxHull2D() : base(HULLTYPE.hull_aabb) { }

    public Vector2 position;

    [SerializeField]
    public float height, width;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;

    }

    // FOR SUCCESSFUL COLLISION, CHANGE COLOR
    public override bool TestCollisionVSCircle(CircleCollisionHull2D other)
    {
        // cam did this part

        // calculate closest point by clamping circle center on each dimension
        // Find the vector2 distance between box & circle
        Vector2 diff = other.thisCenter - position;

        // Normalize that vector
        diff.Normalize();

        // multiply the vector by the radius to get the closest point on the circumference
        diff *= other.radius;

        // find the box's mins and maxes
        float xMin = position.x - width / 2;
        float xMax = position.x + width / 2;

        float yMin = position.y - height / 2;
        float yMax = position.y + height / 2;

        // Check if closest point is within box bounds
        // pass if closest point vs. circle passes
        if (xMax > diff.x && diff.x > xMin)
        {
            if(yMax > diff.y && diff.y > yMin)
            {
                return true;
            }
        }

        return false;
    }
    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other)
    {
        // for each dimension, max extent of A greater than min extent of B

        // position - 1/2(length or width) for min point in x or y
        float thisXMin = position.x - width / 2;

        // position + 1/2(length or width) for max point in x or y
        float thisXMax = position.x + width / 2;

        // position - 1/2(length or width) for min point in x or y
        float thisYMin = position.y - height / 2;

        // position + 1/2(length or width) for max point in x or y
        float thisYMax = position.y + height / 2;

        // position - 1/2(length or width) for min point in x or y
        float otherXMin = other.position.x - width / 2;

        // position + 1/2(length or width) for max point in x or y
        float otherXMax = other.position.x + width / 2;

        // position - 1/2(length or width) for min point in x or y
        float otherYMin = other.position.y - height / 2;

        // position + 1/2(length or width) for max point in x or y
        float otherYMax = other.position.y + height / 2;


        // check which min is greater, greater min becomes the one, other becomes other
        // if one max.x < other max.x && one max.x > other min.x
        if (otherXMin < thisXMax && thisXMax < otherXMax)
        {
            // if this passes, check same thing with y
            if (otherYMin < thisYMax && thisYMax < otherYMax)
            {
                return true;
            }
            else if (otherYMin < thisYMin && thisYMin < otherYMax)
            {
                return true;
            }
        }

        // check which min is greater, greater min becomes the one, other becomes other
        // if one min.x < other max.x && one min.x > other min.x
        if (otherXMin < thisXMin && thisXMin < otherXMax)
        {
            // if this passes, check same thing with y
            if (otherYMin < thisYMax && thisYMax < otherYMax)
            {
                return true;
            }
            else if (otherYMin < thisYMin && thisYMin < otherYMax)
            {
                return true;
            }
        }


        return false;
    }

    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other)
    {
        // Same as above twive
        // first, test AABB vs max extents of OBB
        // then, multiply by OBB inverse matrix, do test again 

        // Same as OBB vs OBB, but only project to ABB up and right normal
        // check the points



        return false;
    }
}
