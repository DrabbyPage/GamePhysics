using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{
    public class Collision
    {
        public struct Contact
        {
            Vector2 point;
            Vector2 normal;
            float restitutionCoefficient;
        }
        public CollisionHull2D a = null, b = null;
        public Contact[] contact = new Contact[4];
        public int contactCount = 0;
        public bool status = false;

        public Vector2 closingVelocity;
    }

    //public List<CollisionHull2D> otherColHullList;

    public enum HULLTYPE
    {
        hull_circle,
        hull_aabb,
        hull_obb,
    }
    public HULLTYPE type { get; }

    protected CollisionHull2D(HULLTYPE type_set)
    {
        type = type_set;
    }

    protected Particle2D particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, ref Collision c)
    {
        // change return type to collision, check status for if the collision passed or not

        return false;
    }

    public abstract bool TestCollisionVSCircle(CircleCollisionHull2D other, ref Collision c);
    public abstract bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other, ref Collision c);
    public abstract bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other, ref Collision c);

}
