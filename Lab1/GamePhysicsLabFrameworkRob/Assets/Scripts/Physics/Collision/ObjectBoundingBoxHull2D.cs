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
        position = transform.position;

        // usually wed have to to (cos (theta), sin (theta)) for right but unity has a transform.right
        rightVector = position + new Vector2(transform.right.x, transform.right.y);

        // usually wed have to to (-sin (theta), cos (theta)) for right but unity has a transform.up
        upVector = position + new Vector2(transform.up.x, transform.up.y);

        Debug.Log(rightVector);
        Debug.Log(upVector);

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

    public override bool TestCollisionVSCircle(CircleCollisionHull2D other, ref Collision c)
    {
        
        // find the min and max components of the obb
        float minX = Mathf.Min(topLeftAxis.x, Mathf.Min(topRightAxis.x, Mathf.Min(botLeftAxis.x, botRightAxis.x)));
        float maxX = Mathf.Max(topLeftAxis.x, Mathf.Max(topRightAxis.x, Mathf.Max(botLeftAxis.x, botRightAxis.x)));

        float minY = Mathf.Min(topLeftAxis.y, Mathf.Min(topRightAxis.y, Mathf.Min(botLeftAxis.y, botRightAxis.y)));
        float maxY = Mathf.Max(topLeftAxis.y, Mathf.Max(topRightAxis.y, Mathf.Max(botLeftAxis.y, botRightAxis.y)));

        // calculate closest point by clamping circle center on each dimension
        // Find the vector2 distance between box & circle
        Vector2 diff = position - other.thisCenter;

        // Normalize that vector
        diff /= Mathf.Abs(diff.magnitude);

        // multiply the vector by the radius to get the closest point on the circumference
        diff *= other.radius;

        // check for the collision
        return IsIntersectingCircle(minX, maxX, minY, maxY, diff.x, diff.y);
    }

    public static bool IsIntersectingCircle(float r1_minX, float r1_maxX, float r1_minY, float r1_maxY, float circClosestX, float circClosestY)
    {
        bool isIntersecting = false;

        // if the closest point of the circle is within the bounds of the square return true
        if ( r1_minX < circClosestX && circClosestX < r1_maxX)
        {
            if(r1_minY < circClosestY && circClosestY < r1_maxY)
            {
                isIntersecting = true;
            }
        }

        return isIntersecting;
    }

    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other, ref Collision c)
    {
        // Same as OBB vs OBB, but only project to ABB up and right normal
        // check the points        

        return other.TestCollisionVSOBB(this, ref c);
    }

    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other, ref Collision c)
    {
        // AABB-OBB part 2 twice
        // Call projection function four times, if even one fails, the collision fails

        // take each corner multiply by the non axis aligned box and 

        bool isIntersecting = false;

        //We have just 4 normals because the other 4 normals are the same but in another direction
        //So we only need a maximum of 4 tests if we have rectangles
        //It is enough if one side is not overlapping, if so we know the rectangles are not intersecting

        //Test 1
        Vector2 normal1 = GetNormal(other.botLeftAxis, other.topLeftAxis);

        if (!IsOverlapping(normal1, other, this))
        {
            //No intersection is possible!
            return isIntersecting;
        }

        //Test 2
        Vector2 normal2 = GetNormal(other.topLeftAxis, other.topRightAxis);

        if (!IsOverlapping(normal2, other, this))
        {
            return isIntersecting;
        }

        //Test 3
        Vector2 normal3 = GetNormal(this.botLeftAxis, this.topLeftAxis);

        if (!IsOverlapping(normal3, other, this))
        {
            return isIntersecting;
        }

        //Test 4
        Vector2 normal4 = GetNormal(this.topLeftAxis, this.topRightAxis);

        if (!IsOverlapping(normal4, other, this))
        {
            return isIntersecting;
        }

        //If we have come this far, then we know all sides are overlapping
        //So the rectangles are intersecting!
        isIntersecting = true;

        return isIntersecting;

    }

    //Get the normal from 2 points. This normal is pointing left in the direction start -> end
    //But it doesn't matter in which direction the normal is pointing as long as you have the same
    //algorithm for all edges
    private static Vector2 GetNormal(Vector2 startPos, Vector2 endPos)
    {
        //The direction
        Vector2 dir = endPos - startPos;

        //The normal, just flip x and z and make one negative (don't need to normalize it)
        Vector2 normal = new Vector2(-dir.y, dir.x);

        //Draw the normal from the center of the rectangle's side
        Debug.DrawRay(startPos + (dir * 0.5f), normal.normalized * 2f, Color.red);

        return normal;
    }

    //Is this side overlapping?
    private static bool IsOverlapping(Vector3 normal, ObjectBoundingBoxHull2D other, ObjectBoundingBoxHull2D self)
    {
        bool isOverlapping = false;

        //Project the corners of rectangle 1 onto the normal
        float dot1 = Vector3.Dot(normal, other.topLeftAxis);
        float dot2 = Vector3.Dot(normal, other.topRightAxis);
        float dot3 = Vector3.Dot(normal, other.botLeftAxis);
        float dot4 = Vector3.Dot(normal, other.botRightAxis);

        //Find the range
        float min1 = Mathf.Min(dot1, Mathf.Min(dot2, Mathf.Min(dot3, dot4)));
        float max1 = Mathf.Max(dot1, Mathf.Max(dot2, Mathf.Max(dot3, dot4)));


        //Project the corners of rectangle 2 onto the normal
        float dot5 = Vector3.Dot(normal, self.topLeftAxis);
        float dot6 = Vector3.Dot(normal, self.topLeftAxis);
        float dot7 = Vector3.Dot(normal, self.botLeftAxis);
        float dot8 = Vector3.Dot(normal, self.botRightAxis);

        //Find the range
        float min2 = Mathf.Min(dot5, Mathf.Min(dot6, Mathf.Min(dot7, dot8)));
        float max2 = Mathf.Max(dot5, Mathf.Max(dot6, Mathf.Max(dot7, dot8)));


        //Are the ranges overlapping?
        if (min1 <= max2 && min2 <= max1)
        {
            isOverlapping = true;
        }

        return isOverlapping;
    }

    // Function to project, pass in other collision hull & normal to be projected onto, project four points onto normal, run AABB test, return bool
}
