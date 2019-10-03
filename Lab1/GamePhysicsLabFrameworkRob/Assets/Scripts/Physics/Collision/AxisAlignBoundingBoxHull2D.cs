using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignBoundingBoxHull2D : CollisionHull2D
{
    public AxisAlignBoundingBoxHull2D() : base(HULLTYPE.hull_aabb) { }

    public Vector2 position;

    [SerializeField]
    public float height, width;

    Vector2 upLeft;
    Vector2 upRight;
    Vector2 botLeft;
    Vector2 botRight;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;

        upLeft = new Vector2(position.x - width * 0.5f, position.y + height * 0.5f);
        upRight = new Vector2(position.x + width * 0.5f, position.y + height * 0.5f);
        botLeft = new Vector2(position.x - width * 0.5f, position.y - height * 0.5f);
        botRight = new Vector2(position.x + width * 0.5f, position.y - height * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;

    }

    // FOR SUCCESSFUL COLLISION, CHANGE COLOR
    public override bool TestCollisionVSCircle(CircleCollisionHull2D other, ref Collision c)
    {
        // cam did this part

        // calculate closest point by clamping circle center on each dimension
        // Find the vector2 distance between box & circle
        Vector2 diff = position - other.thisCenter;

        // Normalize that vector
        diff/= Mathf.Abs(diff.magnitude);

        // multiply the vector by the radius to get the closest point on the circumference
        diff *= other.radius;

        diff += other.thisCenter;

        // find the box's mins and maxes
        float xMin = position.x - width * 0.5f;
        float xMax = position.x + width * 0.5f;

        float yMin = position.y - height * 0.5f;
        float yMax = position.y + height * 0.5f;

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
    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other, ref Collision c)
    {
        // for each dimension, max extent of A greater than min extent of B

        // position - 1/2(length or width) for min point in x or y
        float thisXMin = position.x - width * 0.5f;

        // position + 1/2(length or width) for max point in x or y
        float thisXMax = position.x + width * 0.5f;

        // position - 1/2(length or width) for min point in x or y
        float thisYMin = position.y - height * 0.5f;

        // position + 1/2(length or width) for max point in x or y
        float thisYMax = position.y + height * 0.5f;

        // position - 1/2(length or width) for min point in x or y
        float otherXMin = other.position.x - width * 0.5f;

        // position + 1/2(length or width) for max point in x or y
        float otherXMax = other.position.x + width * 0.5f;

        // position - 1/2(length or width) for min point in x or y
        float otherYMin = other.position.y - height * 0.5f;

        // position + 1/2(length or width) for max point in x or y
        float otherYMax = other.position.y + height * 0.5f;


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

    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other, ref Collision c)
    {
        // Same as above twive
        // first, test AABB vs max extents of OBB
        // then, multiply by OBB inverse matrix, do test again 

        // Same as OBB vs OBB, but only project to ABB up and right normal
        // check the points

        bool isIntersecting = false;

        //Find the min/max values for the AABB algorithm
        float r1_minX = Mathf.Min(other.topLeftAxis.x, Mathf.Min(other.topRightAxis.x, Mathf.Min(other.botLeftAxis.x, other.botRightAxis.x)));
        float r1_maxX = Mathf.Max(other.topLeftAxis.x, Mathf.Max(other.topRightAxis.x, Mathf.Max(other.botLeftAxis.x, other.botRightAxis.x)));

        float r2_minX = Mathf.Min(upLeft.x, Mathf.Min(upRight.x, Mathf.Min(botLeft.x, botRight.x)));
        float r2_maxX = Mathf.Max(upLeft.x, Mathf.Max(upRight.x, Mathf.Max(botLeft.x, botRight.x)));

        float r1_minY = Mathf.Min(other.topLeftAxis.y, Mathf.Min(other.topRightAxis.y, Mathf.Min(other.botLeftAxis.y, other.botRightAxis.y)));
        float r1_maxY = Mathf.Max(other.topLeftAxis.y, Mathf.Max(other.topRightAxis.y, Mathf.Max(other.botLeftAxis.y, other.botRightAxis.y)));

        float r2_minY = Mathf.Min(upLeft.y, Mathf.Min(upRight.y, Mathf.Min(botLeft.y, botRight.y)));
        float r2_maxY = Mathf.Max(upLeft.y, Mathf.Max(upRight.y, Mathf.Max(botLeft.y, botRight.y)));

        if (IsIntersectingAABB(r1_minX, r1_maxX, r1_minY, r1_maxY, r2_minX, r2_maxX, r2_minY, r2_maxY))
        {
            isIntersecting = true;
        }

        return isIntersecting;

    }

    public static bool IsIntersectingAABB(float r1_minX, float r1_maxX, float r1_minY, float r1_maxY,float r2_minX, float r2_maxX, float r2_minY, float r2_maxY)
    {
        //If the min of one box in one dimension is greater than the max of another box then the boxes are not intersecting
        //They have to intersect in 2 dimensions. We have to test if box 1 is to the left or box 2 and vice versa
        bool isIntersecting = true;

        //X axis
        if (r1_minX > r2_maxX)
        {
            isIntersecting = false;
        }
        else if (r2_minX > r1_maxX)
        {
            isIntersecting = false;
        }
        // y Axis
        else if (r1_minY > r2_maxY)
        {
            isIntersecting = false;
        }
        else if (r2_minY > r1_maxY)
        {
            isIntersecting = false;
        }


        return isIntersecting;
    }
}
