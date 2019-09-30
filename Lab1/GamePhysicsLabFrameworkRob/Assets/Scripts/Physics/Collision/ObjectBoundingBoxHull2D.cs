using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxHull2D : CollisionHull2D
{
    public ObjectBoundingBoxHull2D() : base(HULLTYPE.hull_obb) { }

    public float rotation;

    Vector2 position;

    Vector2 rightVector;
    Vector2 upVector;

    [SerializeField]
    float height, width;

    public Vector2 topLeftAxis;
    public Vector2 botLeftAxis;
    public Vector2 topRightAxis;
    public Vector2 botRightAxis;

    // Start is called before the first frame update
    void Start()
    {
        // usually wed have to to (cos (theta), sin (theta)) for right but unity has a transform.right
        rightVector = position + new Vector2(transform.right.x, transform.right.y);

        // usually wed have to to (-sin (theta), cos (theta)) for right but unity has a transform.up
        upVector = position + new Vector2(transform.up.x, transform.up.y);

        topLeftAxis = upVector - rightVector;
        topRightAxis = upVector + rightVector;
        botLeftAxis = -rightVector - upVector;
        botRightAxis = rightVector - upVector;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // FOR SUCCESSFUL COLLISION, CHANGE COLOR

    public override bool TestCollisionVSCircle(CircleCollisionHull2D other)
    {
        // transform circle position by multiplying by box world matrix inverse
        // Find four points on circle, pos.x + cos(a), pos.x - cos(a), pos.y + sin(a), pos.y - sin(a)
        // Project four points on box normal, project box maxes and mins on circle normal
        // Run AABB test

        float minX = Mathf.Min(topLeftAxis.x, Mathf.Min(topRightAxis.x, Mathf.Min(botLeftAxis.x, botRightAxis.x)));
        float maxX = Mathf.Max(topLeftAxis.x, Mathf.Max(topRightAxis.x, Mathf.Max(botLeftAxis.x, botRightAxis.x)));

        float minY = Mathf.Min(topLeftAxis.y, Mathf.Min(topRightAxis.y, Mathf.Min(botLeftAxis.y, botRightAxis.y)));
        float maxY = Mathf.Max(topLeftAxis.y, Mathf.Max(topRightAxis.y, Mathf.Max(botLeftAxis.y, botRightAxis.y)));

        // calculate closest point by clamping circle center on each dimension
        // Find the vector2 distance between box & circle
        Vector2 diff = other.thisCenter - position;

        // Normalize that vector
        diff.Normalize();

        // multiply the vector by the radius to get the closest point on the circumference
        diff *= other.radius;

        return IsIntersectingCircle(minX, maxX, minY, maxY, diff.x, diff.y);
    }


    public static bool IsIntersectingCircle(float r1_minX, float r1_maxX, float r1_minY, float r1_maxY, float circClosestX, float circClosestY)
    {
        //If the min of one box in one dimension is greater than the max of another box then the boxes are not intersecting
        //They have to intersect in 2 dimensions. We have to test if box 1 is to the left or box 2 and vice versa
        bool isIntersecting = false;

        if( r1_minX < circClosestX && circClosestX < r1_maxX)
        {
            if(r1_minY < circClosestY && circClosestY < r1_maxY)
            {
                isIntersecting = true;
            }
        }

        return isIntersecting;
    }

    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other)
    {
        // Same as OBB vs OBB, but only project to ABB up and right normal
        // check the points        

        return other.TestCollisionVSOBB(this);
    }

    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other)
    {
        // AABB-OBB part 2 twice
        // Call projection function four times, if even one fails, the collision fails

        // take each corner multiply by the non axis aligned box and 



        return false;
    }



    // Function to project, pass in other collision hull & normal to be projected onto, project four points onto normal, run AABB test, return bool
}
