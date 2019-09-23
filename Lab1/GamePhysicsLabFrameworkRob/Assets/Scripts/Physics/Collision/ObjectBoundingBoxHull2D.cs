using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxHull2D : CollisionHull2D
{
    public ObjectBoundingBoxHull2D() : base(HULLTYPE.hull_obb) { }

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
        // see circle

        return false;
    }
    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other)
    {
        // see AABB

        return false;
    }
    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other)
    {
        // AABB-OBB part 2 twice

        return false;
    }
}
