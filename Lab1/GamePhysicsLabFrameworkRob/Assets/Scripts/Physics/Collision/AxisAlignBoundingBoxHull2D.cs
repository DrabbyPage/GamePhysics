using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignBoundingBoxHull2D : CollisionHull2D
{
    public AxisAlignBoundingBoxHull2D() : base(HULLTYPE.hull_aabb) { }

    public Vector2 position;

    [SerializeField]
    public float height, width;

    public Vector2 upLeft;
    public Vector2 upRight;
    public Vector2 botLeft;
    public Vector2 botRight;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;

        upLeft = new Vector2(position.x - width * 0.5f, position.y + height * 0.5f);
        upRight = new Vector2(position.x + width * 0.5f, position.y + height * 0.5f);
        botLeft = new Vector2(position.x - width * 0.5f, position.y - height * 0.5f);
        botRight = new Vector2(position.x + width * 0.5f, position.y - height * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;

    }

    // FOR SUCCESSFUL COLLISION, CHANGE COLOR
    public override bool TestCollisionVSCircle(CircleCollisionHull2D other, ref Collision c)
    {
        // cam did this part

        float circleX = other.thisCenter.x;
        float circleY = other.thisCenter.y;
        float circleRadius = other.radius;

        float rectX = position.x;
        float rectY = position.y;

        float DeltaX = circleX - Mathf.Max(rectX, Mathf.Min(circleX, rectX + width * 0.5f));
        float DeltaY = circleY - Mathf.Max(rectY, Mathf.Min(circleY, rectY + height * 0.5f));
        return (DeltaX * DeltaX + DeltaY * DeltaY) < (circleRadius * circleRadius);

    }
    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other, ref Collision c)
    {
        // for each dimension, max extent of A greater than min extent of B

        // position - 1/2(length or width) for min point in x or y
        float thisXMin = position.x - width * 0.5f;

        // position + 1/2(length or width) for max point in x or y
        float thisXMax = position.x + width * 0.5f;

        // position - 1/2(length or width) for min point in x or y
        float thisYMin = position.y - height * 0.5f;

        // position + 1/2(length or width) for max point in x or y
        float thisYMax = position.y + height * 0.5f;

        // position - 1/2(length or width) for min point in x or y
        float otherXMin = other.position.x - width * 0.5f;

        // position + 1/2(length or width) for max point in x or y
        float otherXMax = other.position.x + width * 0.5f;

        // position - 1/2(length or width) for min point in x or y
        float otherYMin = other.position.y - height * 0.5f;

        // position + 1/2(length or width) for max point in x or y
        float otherYMax = other.position.y + height * 0.5f;


        // check which min is greater, greater min becomes the one, other becomes other
        // if one max.x < other max.x && one max.x > other min.x
        if (otherXMin < thisXMax && thisXMax < otherXMax)
        {
            // if this passes, check same thing with y
            if (otherYMin < thisYMax && thisYMax < otherYMax)
            {
                return true;
            }
            else if (otherYMin < thisYMin && thisYMin < otherYMax)
            {
                return true;
            }
        }

        // check which min is greater, greater min becomes the one, other becomes other
        // if one min.x < other max.x && one min.x > other min.x
        if (otherXMin < thisXMin && thisXMin < otherXMax)
        {
            // if this passes, check same thing with y
            if (otherYMin < thisYMax && thisYMax < otherYMax)
            {
                return true;
            }
            else if (otherYMin < thisYMin && thisYMin < otherYMax)
            {
                return true;
            }
        }


        return false;
    }

    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other, ref Collision c)
    {
        // Same as above twive
        // first, test AABB vs max extents of OBB
        // then, multiply by OBB inverse matrix, do test again 

        // Same as OBB vs OBB, but only project to ABB up and right normal
        // check the points


        return other.TestCollisionVSAABB(this, ref c);
    }
}
