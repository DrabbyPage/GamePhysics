using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollisionHull2D : CollisionHull2D
{
    public CircleCollisionHull2D() : base(HULLTYPE.hull_circle) { }

    [Range(0.0f, 100.0f)]
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // FOR SUCCESSFUL COLLISION, CHANGE COLOR

    public override bool TestCollisionVSCircle(CircleCollisionHull2D other)
    {
        // Passes if distance between centers <= sum of radii
        // optimized collision passes if (distance between centers) squared <= (sum of radii) squared
        // 1. Get the two centers
        // 2. difference between centers
        // 3. distance squared = dot(diff, diff)
        // 4. Sum of radii
        // 5. square sum
        // 6. Do the test: distSqr <= sumSqr

        return false;
    }
    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other)
    {
        // calculate closest point by clamping circle center on each dimension
        // pass if closest point vs. circle passes
        // 1. 


        return false;
    }
    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other)
    {
        // Same as above, but first...
        // transform circle position by multiplying by box world matrix inverse


        return false;
    }
}
