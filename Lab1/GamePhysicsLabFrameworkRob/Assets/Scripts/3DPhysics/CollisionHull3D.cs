using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull3D : MonoBehaviour
{
    public class Collision
    {
        public struct Contact
        {
            public Vector3 point;
            public Vector3 normal;
            public float restitutionCoefficient;
            public float penetration;
        }
        public CollisionHull3D a = null, b = null;
        public Contact[] contact = new Contact[4];
        public int contactCount = 0;
        public bool status = false;

        // Vc = -(Va - Vb) * (Direction vector a - direction vector b)
        public Vector3 closingVelocity;

        public Collision()
        {

        }
        
        public void ContactResolver(Contact con)
        {
            //Vector2 velDiff = a.particle.velocity - b.particle.velocity;

            float xVelDiff = a.particle.particle3DTransform.velocity.x - b.particle.particle3DTransform.velocity.x;
            float yVelDiff = a.particle.particle3DTransform.velocity.y - b.particle.particle3DTransform.velocity.y;
            float zVelDiff = a.particle.particle3DTransform.velocity.z - b.particle.particle3DTransform.velocity.z;

            float distX = b.particle.particle3DTransform.position.x - a.particle.particle3DTransform.position.x;
            float distY = b.particle.particle3DTransform.position.y - a.particle.particle3DTransform.position.y;
            float distZ = b.particle.particle3DTransform.position.z - b.particle.particle3DTransform.position.z;

            //Debug.Log(xVelDiff * distX + yVelDiff * distY);

            if (xVelDiff * distX + yVelDiff * distY + zVelDiff * distZ >= 0)
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

                Vector3 rotatedVectorA = Quaternion.Euler(0, 0, angle) * a.particle.particle3DTransform.velocity;
                Vector3 rotatedVectorB = Quaternion.Euler(0, 0, angle) * b.particle.particle3DTransform.velocity;

                Vector3 newVelA = new Vector3(rotatedVectorA.x * (massA - massB) / (massA + massB) + rotatedVectorB.x * 2 * massB / (massA + massB), rotatedVectorA.y);
                Vector3 newVelB = new Vector3(rotatedVectorB.x * (massA - massB) / (massA + massB) + rotatedVectorA.x * 2 * massB / (massA + massB), rotatedVectorB.y);

                Vector3 finalVelA = Quaternion.Euler(0, 0, -angle) * newVelA;
                Vector3 finalVelB = Quaternion.Euler(0, 0, -angle) * newVelB;

                a.particle.SetVelocityX(finalVelA.x);
                a.particle.SetVelocityY(finalVelA.y);
                a.particle.SetVelocityZ(finalVelA.z);

                b.particle.SetVelocityX(finalVelB.x);
                b.particle.SetVelocityY(finalVelB.y);
                b.particle.SetVelocityZ(finalVelB.z);
            }

            resolveInterpenetration(con);
        }

        public void resolveInterpenetration(Contact con)
        {
            Vector3[] particleMovement = new Vector3[2];

            if (con.penetration <= 0)
            {
                return;
            }

            float totalInverseMass = a.particle.GetInvMass() + b.particle.GetInvMass();
            if (totalInverseMass <= 0)
            {
                return;
            }

            Vector3 movementPerIMass = con.normal * (con.penetration / totalInverseMass);
            particleMovement[0] = movementPerIMass * a.particle.GetInvMass();
            particleMovement[1] = movementPerIMass * -b.particle.GetInvMass();

            a.particle.SetPositionX(a.transform.position.x + particleMovement[0].x);
            a.particle.SetPositionY(a.transform.position.y + particleMovement[0].y);
            a.particle.SetPositionZ(a.transform.position.z + particleMovement[0].z);
            b.particle.SetPositionX(b.transform.position.x + particleMovement[1].x);
            b.particle.SetPositionY(b.transform.position.y + particleMovement[1].y);
            b.particle.SetPositionZ(b.transform.position.z + particleMovement[1].z);
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
                    Vector3 currentSV = Vector3.Scale((a.particle.particle3DTransform.velocity - b.particle.particle3DTransform.velocity), contact[i].normal);

                    Vector3 nextSV = Vector3.Scale((a.particle.particle3DTransform.velocity - b.particle.particle3DTransform.velocity), contact[i + 1].normal);
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
    }

    public enum HULLTYPE
    {
        hull_sphere,
        hull_aabb,
        hull_obb,
    }
    protected CollisionHull3D(HULLTYPE type_set)
    {
        type = type_set;
    }

    public HULLTYPE type { get; }

    protected Particle3D particle;
    public Collision c;
    public bool colliding = false;
    [SerializeField]
    public float restitution = 0f;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle3D>();
        c = new Collision();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract bool TestCollisionVSSphere(SphereCollisionHull3D other, ref Collision c);
    public abstract bool TestCollisionVSAABB3D(AABBCollisionHull3D other, ref Collision c);
    public abstract bool TestCollisionVSOBB3D(OBBCollisionHull3D other, ref Collision c);
}
