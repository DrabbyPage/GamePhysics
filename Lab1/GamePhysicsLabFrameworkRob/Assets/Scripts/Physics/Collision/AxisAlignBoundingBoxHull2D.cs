using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignBoundingBoxHull2D : CollisionHull2D
{
    public AxisAlignBoundingBoxHull2D() : base(HULLTYPE.hull_aabb) { }

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
        // calculate closest point by clamping circle center on each dimension
        // Find the vector2 distance between box & circle
        // Normalize that vector
        // multiply the vector by the radius to get the closest point on the circumference
        // Check if closest point is within box bounds
        // pass if closest point vs. circle passes

        return false;
    }
    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other)
    {
        // for each dimension, max extent of A greater than min extent of B
        // position + 1/2(length or width) for max point in x or y
        // position - 1/2(length or width) for min point in x or y
        // check which min is greater, greater min becomes the one, other becomes other
        // if one min.x < other max.x && one min.x > other min.x
        // if this passes, check same thing with y

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
