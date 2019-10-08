using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxHull2D : CollisionHull2D
{
    public ObjectBoundingBoxHull2D() : base(HULLTYPE.hull_obb) { }

    public float rotation;

    Vector2 position;

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

        topLeftAxis = position + (Vector2)(transform.up * (height * 0.5f) - transform.right * (width * 0.5f)); ;
        topRightAxis = position + (Vector2)(transform.up * (height  * 0.5f) + transform.right * (width  * 0.5f));
        botLeftAxis = position + (Vector2)(-transform.up * (height  * 0.5f) - transform.right * (width  * 0.5f));
        botRightAxis = position + (Vector2)(transform.right * (width  * 0.5f) - transform.up * (height  * 0.5f));
        c = new Collision();
        particle = GetComponent<Particle2D>();
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;

        topLeftAxis = position + (Vector2)(transform.up * (height  * 0.5f) - transform.right * (width  * 0.5f));
        topRightAxis = position + (Vector2)(transform.up * (height  * 0.5f) + transform.right * (width  * 0.5f));
        botLeftAxis = position + (Vector2)(-transform.up * (height  * 0.5f) - transform.right * (width  * 0.5f));
        botRightAxis = position + (Vector2)(transform.right * (width  * 0.5f) - transform.up * (height  * 0.5f));
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
        Vector2 diff = (Vector2)transform.position - (Vector2)other.transform.position;

        // Normalize that vector
        // multiply the vector by the radius to get the closest point on the circumference
        diff = diff.normalized * other.radius + (Vector2)other.transform.position;


        Debug.Log(minX + ", " + maxX);
        Debug.Log(minY + ", " + maxY);
        Debug.Log(diff);

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
        bool isIntersecting = false;

        //Find out if the rectangles are intersecting by approximating them with rectangles 
        //with no rotation and then use AABB intersection
        //Will make it faster if the probability that the rectangles are intersecting is low
        if (!IsIntersectingAABB_OBB(other))
        {
            return isIntersecting;
        }

        //Find out if the rectangles are intersecting by using the Separating Axis Theorem (SAT)
        isIntersecting = SATRectangleRectangle(other);

        return isIntersecting;
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

    private bool IsIntersectingAABB_OBB(AxisAlignBoundingBoxHull2D other)
    {
        bool isIntersecting = false;

        Vector2 otherTL = other.upLeft;
        Vector2 otherTR = other.upRight;
        Vector2 otherBL = other.botLeft;
        Vector2 otherBR = other.botRight;

        Vector2 thisTL = topLeftAxis;
        Vector2 thisTR = topRightAxis;
        Vector2 thisBL = botLeftAxis;
        Vector2 thisBR = botRightAxis;

        //Find the min/max values for the AABB algorithm
        float r1_minX = Mathf.Min(otherTL.x, Mathf.Min(otherTR.x, Mathf.Min(otherBL.x, otherBR.x)));
        float r1_maxX = Mathf.Max(otherTL.x, Mathf.Max(otherTR.x, Mathf.Max(otherBL.x, otherBR.x)));

        float r2_minX = Mathf.Min(thisTL.x, Mathf.Min(thisTR.x, Mathf.Min(thisBL.x, thisBR.x)));
        float r2_maxX = Mathf.Max(thisTL.x, Mathf.Max(thisTR.x, Mathf.Max(thisBL.x, thisBR.x)));

        float r1_minY = Mathf.Min(otherTL.y, Mathf.Min(otherTR.y, Mathf.Min(otherBL.y, otherBR.y)));
        float r1_maxY = Mathf.Max(otherTL.y, Mathf.Max(otherTR.y, Mathf.Max(otherBL.y, otherBR.y)));
                    
        float r2_minY = Mathf.Min(thisTL.y, Mathf.Min(thisTR.y, Mathf.Min(thisBL.y, thisBR.y)));
        float r2_maxY = Mathf.Max(thisTL.y, Mathf.Max(thisTR.y, Mathf.Max(thisBL.y, thisBR.y)));

        if (IsIntersectingAABB(r1_minX, r1_maxX, r1_minY, r1_maxY, r2_minX, r2_maxX, r2_minY, r2_maxY))
        {
            isIntersecting = true;
        }

        return isIntersecting;
    }

    //Intersection: AABB-AABB (Axis-Aligned Bounding Box) - rectangle-rectangle in 2d space with no orientation
    public bool IsIntersectingAABB(float r1_minX, float r1_maxX, float r1_minZ, float r1_maxZ, float r2_minX, float r2_maxX, float r2_minZ, float r2_maxZ)
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
        //Z axis
        else if (r1_minZ > r2_maxZ)
        {
            isIntersecting = false;
        }
        else if (r2_minZ > r1_maxZ)
        {
            isIntersecting = false;
        }


        return isIntersecting;
    }

    //Find out if 2 rectangles with orientation are intersecting by using the SAT algorithm
    private bool SATRectangleRectangle(AxisAlignBoundingBoxHull2D other)
    {
        bool isIntersecting = false;

        //We have just 4 normals because the other 4 normals are the same but in another direction
        //So we only need a maximum of 4 tests if we have rectangles
        //It is enough if one side is not overlapping, if so we know the rectangles are not intersecting

        Vector2 otherTL = other.upLeft;
        Vector2 otherTR = other.upRight;
        Vector2 otherBL = other.botLeft;
        Vector2 otherBR = other.botRight;

        Vector2 thisTL = topLeftAxis;
        Vector2 thisTR = topRightAxis;
        Vector2 thisBL = botLeftAxis;
        Vector2 thisBR = botRightAxis;

        //Test 1
        Vector3 normal1 = GetNormal(otherBL, otherTL);

        if (!IsOverlappingABB(normal1, other, this))
        {
            //No intersection is possible!
            return isIntersecting;
        }

        //Test 2
        Vector3 normal2 = GetNormal(otherTL, otherTR);

        if (!IsOverlappingABB(normal2, other, this))
        {
            return isIntersecting;
        }

        //Test 3
        Vector3 normal3 = GetNormal(thisBL, thisTL);

        if (!IsOverlappingABB(normal3, other, this))
        {
            return isIntersecting;
        }

        //Test 4
        Vector3 normal4 = GetNormal(thisTL, thisTR);

        if (!IsOverlappingABB(normal4, other, this))
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

        //The normal, just flip x and Y and make one negative (don't need to normalize it)
        Vector2 normal = new Vector2(-dir.y, dir.x);

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

    private static bool IsOverlappingABB(Vector3 normal, AxisAlignBoundingBoxHull2D other, ObjectBoundingBoxHull2D self)
    {
        bool isOverlapping = false;

        //Project the corners of rectangle 1 onto the normal
        float dot1 = Vector3.Dot(normal, other.upLeft);
        float dot2 = Vector3.Dot(normal, other.upRight);
        float dot3 = Vector3.Dot(normal, other.botLeft);
        float dot4 = Vector3.Dot(normal, other.botRight);

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
}
