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
        // Early out check to see if we can exclude the contact. 
        if (real_abs(relCenter.x) - sphere.radius > box.halfSize.x || 
        real_abs(relCenter.y) - sphere.radius > box.halfSize.y || 
        real_abs(relCenter.z) - sphere.radius > box.halfSize.z) 
        { 
        return 0;
        }
        */
        if( Mathf.Abs(relCenter.x)-radius>other.length/2 ||
            Mathf.Abs(relCenter.y)-radius>other.height/2 ||
            Mathf.Abs(relCenter.z)-radius>other.width/2)
        {
            //return false;
        }
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

        if(dist > other.length/2)
        {
            dist = other.length / 2;
        }
        if(dist < -other.length / 2)
        {
            dist = -other.length / 2;
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

        if (dist > other.height / 2)
        {
            dist = other.height / 2;
        }
        if (dist < -other.height / 2)
        {
            dist = -other.height / 2;
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

        if (dist > other.width / 2)
        {
            dist = other.width / 2;
        }
        if (dist < -other.width / 2)
        {
            dist = -other.width / 2;
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

        Debug.Log(closestPoint);
        Debug.Log(relCenter);
        Debug.Log(dist);

        if (dist>radius*radius)
        {
            return false;
        }
        else
        {
            return true;
        }
    }       
    public override bool TestCollisionVSOBB3D(OBBCollisionHull3D other, ref Collision c)
    {
        return false;
    }
}
