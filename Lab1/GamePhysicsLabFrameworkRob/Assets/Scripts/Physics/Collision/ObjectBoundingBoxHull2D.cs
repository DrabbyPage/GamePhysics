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

        return false;
    }
    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other)
    {
        // Same as OBB vs OBB, but only project to ABB up and right normal
        // check the points

        //pls fix
        Matrix4x4 otherTransformMat =
            new Matrix4x4(
                other.position.x, 0, 0, 0,
                0, other.position.y, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            );

        //Fix
        topLeftAxis *= otherTransformMat;
        topRightAxis *= otherTransformMat;
        botLeftAxis *= otherTransformMat;
        botRighttAxis *= otherTransformMat;

        return false;
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
