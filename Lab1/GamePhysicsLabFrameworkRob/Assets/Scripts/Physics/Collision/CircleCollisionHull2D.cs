using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollisionHull2D : CollisionHull2D
{
    public CircleCollisionHull2D() : base(HULLTYPE.hull_circle) { }

    [Range(0.0f, 100.0f)]
    public float radius;
    public Vector2 thisCenter;

    // Start is called before the first frame update
    void Start()
    {
        thisCenter = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        thisCenter = new Vector2(transform.position.x, transform.position.y);
    }

    // FOR SUCCESSFUL COLLISION, CHANGE COLOR

    public override bool TestCollisionVSCircle(CircleCollisionHull2D other, ref Collision c)
    {
        //ROB
        // Passes if distance between centers <= sum of radii
        // optimized collision passes if (distance between centers) squared <= (sum of radii) squared
        // 1. Get the two centers
        Vector2 otherCenter = other.thisCenter;

        // 2. difference between centers
        Vector2 distanceVec = otherCenter - thisCenter;

        //float distance = (otherCenter.x - thisCenter.x) * (otherCenter.x - thisCenter.x) 
        //               + (otherCenter.y - thisCenter.y) * (otherCenter.y - thisCenter.y);
        //distance *= distance;

        // 3. distance squared = dot(diff, diff)
        float distance = Vector2.Dot(distanceVec, distanceVec);

        // 4. Sum of radii
        float totalRadii = radius + other.radius;

        // 5. square sum
        totalRadii *= totalRadii;

        // 6. Do the test: distSqr <= sumSqr
        if (distance <= totalRadii)
        {
            c.a = this;
            c.b = other;
            c.status = true;
            if (distance < totalRadii)
            {
                // find the point in the center of the overlap between the two circles
                c.contactCount = 2;
                float distanceToContactPoint = ((distance * distance - other.radius * other.radius + radius * radius) / (2 * distance));
                c.contact[0].point.x = distanceToContactPoint;
                c.contact[0].point.y = Mathf.Sqrt(radius * radius - distanceToContactPoint);
                //c.contact[0].normal;
            }
            else if (distance == totalRadii)
            {
                // Only the edges are contacting
                c.contactCount = 1;
                c.contact[0].point = thisCenter + distanceVec.normalized * radius;
            }
            return true;
        }

        return false;
    }
    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other, ref Collision c)
    {
        // calculate closest point by clamping circle center on each dimension
        // Find the vector2 distance between box & circle
        // Normalize that vector
        // multiply the vector by the radius to get the closest point on the circumference
        // Check if closest point is within box bounds
        // pass if closest point vs. circle passes

        return other.TestCollisionVSCircle(this, ref c);
    }
    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other, ref Collision c)
    {
        // Same as above, but first...
        // transform circle position by multiplying by box world matrix inverse
        // Find four points on circle, pos.x + cos(a), pos.x - cos(a), pos.y + sin(a), pos.y - sin(a)
        // Project four points on box normal, project box maxes and mins on circle normal
        // Run AABB test


        return other.TestCollisionVSCircle(this, ref c);
    }
}
