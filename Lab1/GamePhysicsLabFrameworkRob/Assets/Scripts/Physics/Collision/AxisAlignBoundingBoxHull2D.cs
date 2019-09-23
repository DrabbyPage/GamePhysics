using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignBoundingBoxHull2D : CollisionHull2D
{
    public AxisAlignBoundingBoxHull2D() : base(HULLTYPE.hull_aabb) { };

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
        // pass if closest point vs. circle passes
        // other.testThis

        return false;
    }
    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other)
    {
        // for each dimension, max extent of A greater than min extent of B

        return false;
    }
    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other)
    {
        // Same as above twive
        // first, test AABB vs max extents of OBB
        // then, multiply by OBB inverse matrix, do test again

        return false;
    }
}
