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
            public float penetration;
        }
        public CollisionHull2D a = null, b = null;
        public Contact[] contact = new Contact[4];
        public int contactCount = 0;
        public bool status = false;

        // Vc = -(Va - Vb) * (Direction vector a - direction vector b)
        public Vector2 closingVelocity;

        public Collision()
        { 
            
        }

        // Velocity after collision = -(restitution * closing velocity)
        // Contact normal = Direction vector a - direction vector b
        // Impulse (g) = m * v


        // Contact Resolver Algorithm
        // 1. Apply impulses on objects to simulate bouncing apart
        // 2. Move objects apart (non-interpenetrating) so they aren't embedded in each other
        // 3. Check if the contact is resting rather than colliding
        public void ContactResolver(Contact con)
        {
            //Vector2 velDiff = a.particle.velocity - b.particle.velocity;

            float xVelDiff = a.particle.velocity.x - b.particle.velocity.x;
            float yVelDiff = a.particle.velocity.y - b.particle.velocity.y;

            float distX = b.particle.position.x - a.particle.position.x;
            float distY = b.particle.position.y - a.particle.position.y;

<<<<<<< HEAD
            //Debug.Log(xVelDiff * distX + yVelDiff * distY);

=======
>>>>>>> 4f16dddb454ed18c3eb2c77faae9a6a883e88d27
            if(xVelDiff * distX + yVelDiff * distY >= 0)
            {
                float angle = -Mathf.Atan2(yVelDiff, xVelDiff) * Mathf.Rad2Deg;

                float massA = a.particle.GetMass();
                float massB = b.particle.GetMass();

                // float magVelA = a.particle.velocity.magnitude;
                // float origRotA = a.particle.rotation * Mathf.Deg2Rad;
                // 
                // float magVelB = b.particle.velocity.magnitude;
                // float origRotB = b.particle.rotation * Mathf.Deg2Rad;

                // https://matthew-brett.github.io/teaching/rotation_2d.html
                // Vector2 origVelA = new Vector2(magVelA * Mathf.Cos(origRotA + angle), magVelA * Mathf.Sin(origRotA + angle));
                // Vector2 origVelB = new Vector2(magVelB * Mathf.Cos(origRotB + angle), magVelB * Mathf.Sin(origRotB + angle));

                Vector2 rotatedVectorA = Quaternion.Euler(0, 0, angle) * a.particle.velocity;
                Vector2 rotatedVectorB = Quaternion.Euler(0, 0, angle) * b.particle.velocity;

<<<<<<< HEAD
                //Debug.Log("we here");

=======
>>>>>>> 4f16dddb454ed18c3eb2c77faae9a6a883e88d27
                Vector2 newVelA = new Vector2(rotatedVectorA.x * (massA - massB) / (massA + massB) + rotatedVectorB.x * 2 * massB / (massA + massB), rotatedVectorA.y);
                Vector2 newVelB = new Vector2(rotatedVectorB.x * (massA - massB) / (massA + massB) + rotatedVectorA.x * 2 * massB / (massA + massB), rotatedVectorB.y);

                Vector2 finalVelA = Quaternion.Euler(0, 0, -angle) * newVelA;
                Vector2 finalVelB = Quaternion.Euler(0, 0, -angle) * newVelB;

                a.particle.SetVelocityX(finalVelA.x);
                a.particle.SetVelocityY(finalVelA.y);

                b.particle.SetVelocityX(finalVelB.x);
                b.particle.SetVelocityY(finalVelB.y);
            }

            resolveInterpenetration(con);
        }

        public void resolveInterpenetration(Contact con)
        {
            Vector2[] particleMovement = new Vector2[2];

            if (con.penetration <= 0)
            {
                return;
            }

            float totalInverseMass = a.particle.GetInvMass() + b.particle.GetInvMass();
            if(totalInverseMass <= 0)
            {
                return;
            }

            Vector2 movementPerIMass = con.normal * (con.penetration / totalInverseMass);
            particleMovement[0] = movementPerIMass * a.particle.GetInvMass();
            particleMovement[1] = movementPerIMass * -b.particle.GetInvMass();

            a.particle.SetPositionX(a.transform.position.x + particleMovement[0].x);
            a.particle.SetPositionY(a.transform.position.y + particleMovement[0].y);
            b.particle.SetPositionX(b.transform.position.x + particleMovement[1].x);
            b.particle.SetPositionY(b.transform.position.y + particleMovement[1].y);
        }

        public void ResolveAllContacts()
        {
            if (contactCount != 0)
            {
                for (int i = 0; i < contactCount; i++)
                {
                    ContactResolver(contact[i]);
                }
            }
        }

        // Resolution order
        // 1. Calculate the separating velocity of each contact, keeping track of the lowest
        // 2. If the lowest separating velocity is greater than or equal to 0, exit algorithm, they are resting
        // 3. Process the contact resolver for the contact with the lowest separating velocity
        // 4. If there are more contacts, return to step 1

        // Separating Velocity = (Va - Vb) * direction of normal
        public void OrderContacts()
        {
            if (contact != null)
            {
                Contact tmp;
                //Debug.Log("Contact Count " + contactCount);
                // Sort through contacts and order them from smallest to largest closing velocity
                for (int i = 0; i < contactCount - 1; i++)
                {
                    Vector2 currentSV = (a.particle.velocity - b.particle.velocity) * contact[i].normal;

                    Vector2 nextSV = (a.particle.velocity - b.particle.velocity) * contact[i + 1].normal;
                    if (currentSV.magnitude > nextSV.magnitude)
                    {
                        tmp = contact[i];
                        contact[i] = contact[i + 1];
                        contact[i + 1] = tmp;
                        i = -1;
                    }
                }

                ResolveAllContacts();
            }

        }

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
    [SerializeField]
    public float restitution = 0f;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
        c = new Collision();
        Debug.Log(c);
        
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
