using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollisionHull3D : CollisionHull3D
{
    public SphereCollisionHull3D() : base(HULLTYPE.hull_sphere) { }
    public Vector3 centerOfSphere;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        centerOfSphere = transform.position;
        c = new Collision();
        particle = GetComponent<Particle3D>();
    }

    // Update is called once per frame
    void Update()
    {
        centerOfSphere = transform.position;
    }

    public override bool TestCollisionVSSphere(SphereCollisionHull3D other, ref Collision c)
    {
        centerOfSphere = transform.position;
        Vector3 dist = other.centerOfSphere - centerOfSphere;
        if (dist.magnitude <= radius + other.radius)
        {
            
            c.a = gameObject.GetComponent<SphereCollisionHull3D>();
            c.b = other;
            
            //Check to see if this is setting to false somewhere if this is breaking
            c.status = true;

            Vector3 midline = c.a.transform.position - c.b.transform.position;
            float size = midline.magnitude;

            //Have a check to see if it's large enough
            c.contact[0].normal = midline / size;
            c.contact[0].point = c.a.transform.position + midline * 0.5f;
            c.contact[0].penetration = (radius + other.radius - size);
            c.contact[0].restitutionCoefficient = restitution;

            c.contactCount = 1;

            return true;
        }
        else
        {
            return false;

        }
    }

    public override bool TestCollisionVSAABB3D(AABBCollisionHull3D other, ref Collision c)
    {
        /*
         // Transform the center of the sphere into box coordinates. 
         Vector3 center = sphere.getAxis(3); 
         Vector3 relCenter = box.transform.transformInverse(center);
         */
        Vector3 center = centerOfSphere;
        Vector3 relCenter = other.transform.InverseTransformPoint(center);

        /*
        Vector3 closestPt(0,0,0);
        real dist;
        // Clamp each coordinate to the box. 
        dist = relCenter.x; 
        if (dist > box.halfSize.x)
            dist = box.halfSize.x; 
        if (dist < -box.halfSize.x) 
            dist = -box.halfSize.x; 
            
        closestPt.x = dist;
        */
        Vector3 closestPoint = new Vector3(0, 0, 0);
        float dist;

        dist = relCenter.x;

        if(dist > other.length * 0.5f)
        {
            dist = other.length * 0.5f;
        }
        if(dist < -other.length * 0.5f)
        {
            dist = -other.length * 0.5f;
        }

        closestPoint.x = dist;

        /*
        dist = relCenter.y;

        if (dist > box.halfSize.y) 
            dist = box.halfSize.y; 
        if (dist < -box.halfSize.y) 
            dist = -box.halfSize.y; 
            
        closestPt.y = dist;

        */
        dist = relCenter.y;

        if (dist > other.height * 0.5f)
        {
            dist = other.height * 0.5f;
        }
        if (dist < -other.height * 0.5f)
        {
            dist = -other.height * 0.5f;
        }

        closestPoint.y = dist;
        /*
        dist = relCenter.z;
        
        if (dist > box.halfSize.z)
            dist = box.halfSize.z; 
        if (dist < -box.halfSize.z) 
            dist = -box.halfSize.z; 
            
        closestPt.z = dist;
        */
        dist = relCenter.z;

        if (dist > other.width * 0.5f)
        {
            dist = other.width *0.5f;
        }
        if (dist < -other.width * 0.5f)
        {
            dist = -other.width * 0.5f;
        }

        closestPoint.z = dist;
        /*
        // Check to see if we’re in contact.
        dist = (closestPt - relCenter).squareMagnitude();
        
        if (dist > sphere.radius * sphere.radius)
            return 0;
        else
            return 1;
         */



        dist = (closestPoint - relCenter).sqrMagnitude;

        if (dist>radius*radius)
        {
            return false;
        }
        else
        {
            c.a = gameObject.GetComponent<SphereCollisionHull3D>();
            c.b = other;
            c.contactCount = 1;
            Debug.Log("Contact Count: " + c.contactCount);
            c.status = true;
            c.contact[0].normal = closestPoint - center;
            c.contact[0].normal.Normalize();
            c.contact[0].point = closestPoint;
            c.contact[0].penetration = radius - Mathf.Sqrt(dist);
            c.contact[0].restitutionCoefficient = restitution;
            return true;
        }
    }       
    public override bool TestCollisionVSOBB3D(OBBCollisionHull3D other, ref Collision c)
    {
        /*
         // Transform the center of the sphere into box coordinates. 
         Vector3 center = sphere.getAxis(3); 
         Vector3 relCenter = box.transform.transformInverse(center);
         */
        Vector3 center = centerOfSphere;
        Vector3 relCenter = other.transform.InverseTransformPoint(center);

        /*
        Vector3 closestPt(0,0,0);
        real dist;
        // Clamp each coordinate to the box. 
        dist = relCenter.x; 
        if (dist > box.halfSize.x)
            dist = box.halfSize.x; 
        if (dist < -box.halfSize.x) 
            dist = -box.halfSize.x; 
            
        closestPt.x = dist;
        */
        Vector3 closestPoint = new Vector3(0, 0, 0);
        float dist;

        dist = relCenter.x;

        if (dist > other.length * 0.5f)
        {
            dist = other.length * 0.5f;
        }
        if (dist < -other.length * 0.5f)
        {
            dist = -other.length * 0.5f;
        }

        closestPoint.x = dist;

        /*
        dist = relCenter.y;

        if (dist > box.halfSize.y) 
            dist = box.halfSize.y; 
        if (dist < -box.halfSize.y) 
            dist = -box.halfSize.y; 
            
        closestPt.y = dist;

        */
        dist = relCenter.y;

        if (dist > other.height * 0.5f)
        {
            dist = other.height * 0.5f;
        }
        if (dist < -other.height * 0.5f)
        {
            dist = -other.height * 0.5f;
        }

        closestPoint.y = dist;
        /*
        dist = relCenter.z;
        
        if (dist > box.halfSize.z)
            dist = box.halfSize.z; 
        if (dist < -box.halfSize.z) 
            dist = -box.halfSize.z; 
            
        closestPt.z = dist;
        */
        dist = relCenter.z;

        if (dist > other.width * 0.5f)
        {
            dist = other.width * 0.5f;
        }
        if (dist < -other.width * 0.5f)
        {
            dist = -other.width * 0.5f;
        }

        closestPoint.z = dist;
        /*
        // Check to see if we’re in contact.
        dist = (closestPt - relCenter).squareMagnitude();
        
        if (dist > sphere.radius * sphere.radius)
            return 0;
        else
            return 1;
         */



        dist = (closestPoint - relCenter).sqrMagnitude;

        if (dist > radius * radius)
        {
            return false;
        }
        else
        {
            c.a = gameObject.GetComponent<SphereCollisionHull3D>();
            c.b = other;
            c.contactCount = 1;
            Debug.Log("Contact Count: " + c.contactCount);
            c.status = true;
            c.contact[0].normal = closestPoint - center;
            c.contact[0].normal.Normalize();
            c.contact[0].point = closestPoint;
            c.contact[0].penetration = radius - Mathf.Sqrt(dist);
            c.contact[0].restitutionCoefficient = restitution;
            return true;
        }
    }
}
