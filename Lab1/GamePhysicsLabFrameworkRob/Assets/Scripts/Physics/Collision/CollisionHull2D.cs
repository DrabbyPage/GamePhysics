using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{
    public class Collision
    {
        public struct Contact
        {
            public Vector2 point;
            public Vector2 normal;
            public float restitutionCoefficient;
        }
        public CollisionHull2D a = null, b = null;
        public Contact[] contact = new Contact[4];
        public int contactCount = 0;
        public bool status = false;

        // Vc = (Va - Vb) * (Direction vector a - direction vector b)
        public Vector2 closingVelocity;

        // Velocity after collision = -(restitution * closing velocity)
        // Contact normal = Direction vector a - direction vector b
        // Impulse (g) = m * v

        // Contact Resolver Algorithm
        // 1. Apply impmulses on objects to simulate bouncing apart
        // 2. Move objects apart (non-interpenetrating) so they aren't embedded in each other
        // 3. Check if the contact is resting rather than colliding

        // Resolution order
        // 1. Calculate the separating velocity of each contact, keeping track of the lowest
        // 2. If the lowest separating velocity is greater than or equal to 0, exit algorithm, they are resting
        // 3. Process the contact resolver for the contact with the lowest separating velocity
        // 4. If there are more contacts, return to step 1

        // POSSIBLE 2ND METHOD
        // 1. Let the start time be the current simulation time, and the end time be the end
        //    of the current update request.
        // 2. Perform a complete update for the entire time interval.
        // 3. Run the collision detector and collect a list of collisions.
        // 4. If there are no collisions, we are done: exit the algorithm.
        // 5. For each collision, work out the exact time of the first collision.
        // 6. Choose the first collision to have occurred.
        // 7. If thefirst collision occurs after the end time, then we’re done: exit the algorithm.
        // 8. Remove the effects of the Step 2 update, and perform a new update from the
        //    start time to the first collision time.
        // 9. Process the collision, applying the appropriate impulses (no interpenetration
        //    resolution is needed, because at the instant of collision the objects are only just
        //    touching).
        // 10. Set the start time to be the first collision time, keep the end time unchanged,
        //     and return to Step 1.
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
    public Collision c;
    public bool colliding = false;

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
