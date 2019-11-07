using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public List<GameObject> particles;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CollisionHull3D currentParticleHull, otherParticleHull;
        bool checkCollision = false;


        // Go through list to compare current particle to every particle after it in the list
        for (int i = 0; i < particles.Count; i++)
        {
            if(particles[i]!= null)
            {
                currentParticleHull = particles[i].GetComponent<Particle3D>().colHull;
                for (int j = i + 1; j < particles.Count; j++)
                {
                    if(particles[j] != null && particles[j] != particles[i])
                    {
                        otherParticleHull = particles[j].GetComponent<Particle3D>().colHull;
                        // Determine which type the second particle is
                        switch (otherParticleHull.type)
                        {
                            // If it's AABB, look for that specific componenet
                            case CollisionHull3D.HULLTYPE.hull_aabb:
                                checkCollision = currentParticleHull.TestCollisionVSAABB3D(particles[j].GetComponent<AABBCollisionHull3D>(), ref particles[i].GetComponent<AABBCollisionHull3D>().c);
                                //Debug.Log("AABB Success");
                                break;
                            // If it's circle, look for that specific componenet
                            case CollisionHull3D.HULLTYPE.hull_sphere:
                                
                                checkCollision = currentParticleHull.TestCollisionVSSphere(particles[j].GetComponent<SphereCollisionHull3D>(), ref particles[i].GetComponent<SphereCollisionHull3D>().c);
                                //Debug.Log("cirlce Success");
                                break;
                            // If it's OBB, look for that specific componenet
                            case CollisionHull3D.HULLTYPE.hull_obb:
                                checkCollision = currentParticleHull.TestCollisionVSOBB3D(particles[j].GetComponent<OBBCollisionHull3D>(), ref particles[i].GetComponent<OBBCollisionHull3D>().c);
                                //Debug.Log("OBB Success");
                                break;
                        }

                        // If the two objects collide, change their color to red
                        if (checkCollision)
                        {
                            Debug.Log("checking collisions");
                            currentParticleHull.colliding = true;
                            otherParticleHull.colliding = true;
                        }
                    }
                    
                }
            }
        }

        // Go through the list of particles, if it's currently colliding, change color
        for (int i = 0; i < particles.Count; i++)
        {
            if (particles[i] != null)
            {
                currentParticleHull = particles[i].GetComponent<Particle3D>().colHull;
                if (currentParticleHull.colliding)
                {
                    currentParticleHull.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    //currentParticleHull.c.OrderContacts();
                    //currentParticleHull.c.ResolveAllContacts();
                }
                else 
                {
                    currentParticleHull.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                // reset colliding check
                currentParticleHull.colliding = false;
            }
        }
    }

}
