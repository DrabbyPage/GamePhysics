using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollisionHull3D : CollisionHull3D
{
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

        //Vector3 sphereCenter = centerOfSphere;
        Vector3 relCenter = other.transform.InverseTransformVector(other.rectCenter);
        
        /*
		// Early out check to see if we can exclude the contact. 
		if (real_abs(relCenter.x) - sphere.radius > box.halfSize.x || 
		    real_abs(relCenter.y) - sphere.radius > box.halfSize.y || 
		    real_abs(relCenter.z) - sphere.radius > box.halfSize.z) 
		   {
			return 0; 
		   }
           */

        if(Mathf.Abs(relCenter.x) - radius > other.length / 2||
           Mathf.Abs(relCenter.y) - radius > other.height / 2||
           Mathf.Abs(relCenter.z) - radius > other.width / 2)
        {
            return false;
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
*/
        Vector3 closestPoint;
        float dist;

        dist = relCenter.x;

        if(dist>other.length/2)
        {
            dist = other.length / 2;
        }
        if(dist < -other.length/2)
        {
            dist = -other.length / 2;
        }
        
        /*
		closestPt.x = dist;

		dist = relCenter.y; 
	
		if (dist > box.halfSize.y) 
			dist = box.halfSize.y; 
		if (dist < -box.halfSize.y) 
			dist = -box.halfSize.y; 
        */

        closestPoint.x = dist;

        dist = relCenter.y;

        if(dist > other.height / 2)
        {
            dist = other.height / 2;
        }
        if(dist < -other.height / 2)
        {
            dist = -other.height / 2;
        }

        /*
		closestPt.y = dist;

		dist = relCenter.z; 

		if (dist > box.halfSize.z) 
			dist = box.halfSize.z; 
		if (dist < -box.halfSize.z) 
			dist = -box.halfSize.z; 
        */

        closestPoint.y = dist;

        dist = relCenter.z;

        if(dist > other.width/2)
        {
            dist = other.width / 2;
        }
        if(dist < -other.width/2)
        {
            dist = other.width / 2;
        }

        /*
		closestPt.z = dist;

		// Check to see if we’re in contact. 
		dist = (closestPt - relCenter).squareMagnitude(); 

		if (dist > sphere.radius * sphere.radius)
			return 0;
		*/

        closestPoint.z = dist;

        dist = (closestPoint - relCenter).sqrMagnitude;

        if(dist > radius * radius)
        {
            return false;
        }
        else
        {
            return true;
        }

        /*
		// compile the contact 
		Vector3 closestPtWorld = box.transform.transform(closestPt);

		Contact* contact = data->contacts; 
		contact->contactNormal = (closestPtWorld - center);

		contact->contactNormal.normalize(); 
		contact->contactPoint = closestPtWorld;
		contact->penetration = sphere.radius - real_sqrt(dist); contact->setBodyData(box.body, sphere.body, data->friction, data->restitution);
		data->addContacts(1); return 1;
        */
    }
    public override bool TestCollisionVSOBB3D(OBBCollisionHull3D other, ref Collision c)
    {
        return false;
    }
}
