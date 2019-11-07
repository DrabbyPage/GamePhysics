using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBBCollisionHull3D : CollisionHull3D
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool TestCollisionVSSphere(SphereCollisionHull3D other, ref Collision c)
    {
        return false;
    }

    public override bool TestCollisionVSAABB3D(AABBCollisionHull3D other, ref Collision c)
    {
        return false;
    }
    public override bool TestCollisionVSOBB3D(OBBCollisionHull3D other, ref Collision c)
    {
        return false;
    }
}
