using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBCollisionHull3D : CollisionHull3D
{
    public AABBCollisionHull3D() : base(HULLTYPE.hull_aabb) { }
    public Vector3 rectCenter;
    [SerializeField] public float length;
    [SerializeField] public float width;
    [SerializeField] public float height;

    public Vector3 frontBotLeft;
    public Vector3 frontTopLeft;
    public Vector3 frontBotRight;
    public Vector3 frontTopRight;
     
    public Vector3 backBotLeft;
    public Vector3 backTopLeft;
    public Vector3 backBotRight;
    public Vector3 backTopRight;

    // Start is called before the first frame update
    void Start()
    {
        rectCenter = transform.position;
        frontTopLeft = new Vector3(rectCenter.x - length / 2, rectCenter.y + height / 2, rectCenter.z - width / 2);
        frontBotLeft = new Vector3(rectCenter.x - length / 2, rectCenter.y - height / 2, rectCenter.z - width / 2);
        frontTopRight = new Vector3(rectCenter.x + length / 2, rectCenter.y + height / 2, rectCenter.z - width / 2);
        frontBotRight = new Vector3(rectCenter.x - length / 2, rectCenter.y - height / 2, rectCenter.z - width / 2);

        backTopLeft = new Vector3(rectCenter.x - length / 2, rectCenter.y + height / 2, rectCenter.z + width / 2);
        backBotLeft = new Vector3(rectCenter.x - length / 2, rectCenter.y - height / 2, rectCenter.z + width / 2);
        backTopRight = new Vector3(rectCenter.x + length / 2, rectCenter.y + height / 2, rectCenter.z + width / 2);
        backBotRight = new Vector3(rectCenter.x - length / 2, rectCenter.y - height / 2, rectCenter.z + width / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool TestCollisionVSSphere(SphereCollisionHull3D other, ref Collision c)
    {
        Debug.Log("This " + this + " other " + other);
        return other.TestCollisionVSAABB3D(this, ref c);
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
