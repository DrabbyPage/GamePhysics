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
        CollisionHull2D currentParticleHull, otherParticleHull;
        bool checkCollision = false;


        // Go through list to compare current particle to every particle after it in the list
        for (int i = 0; i < particles.Count; i++)
        {
            if(particles[i]!= null)
            {
                currentParticleHull = particles[i].GetComponent<Particle2D>().colHull;
                for (int j = 0; j < particles.Count; j++)
                {
                    if(particles[j] != null && particles[j] != particles[i])
                    {
                        otherParticleHull = particles[j].GetComponent<Particle2D>().colHull;
                        // Determine which type the second particle is
                        switch (otherParticleHull.type)
                        {
                            // If it's AABB, look for that specific componenet
                            case CollisionHull2D.HULLTYPE.hull_aabb:
                                checkCollision = currentParticleHull.TestCollisionVSAABB(particles[j].GetComponent<AxisAlignBoundingBoxHull2D>(), ref particles[i].GetComponent<AxisAlignBoundingBoxHull2D>().c);
                                Debug.Log("AABB Success");
                                break;
                            // If it's circle, look for that specific componenet
                            case CollisionHull2D.HULLTYPE.hull_circle:
                                
                                checkCollision = currentParticleHull.TestCollisionVSCircle(particles[j].GetComponent<CircleCollisionHull2D>(), ref particles[i].GetComponent<CircleCollisionHull2D>().c);
                                Debug.Log("cirlce Success");
                                break;
                            // If it's OBB, look for that specific componenet
                            case CollisionHull2D.HULLTYPE.hull_obb:
                                checkCollision = currentParticleHull.TestCollisionVSOBB(particles[j].GetComponent<ObjectBoundingBoxHull2D>(), ref particles[i].GetComponent<ObjectBoundingBoxHull2D>().c);
                                Debug.Log("OBB Success");
                                break;
                        }

                        // If the two objects collide, change their color to red
                        if (checkCollision)
                        {
                            currentParticleHull.colliding = true;
                            otherParticleHull.colliding = true;
                            if (currentParticleHull.gameObject.tag == "spaceship")
                            {
                                currentParticleHull.gameObject.GetComponent<SpaceshipManagerScript>().collisions += 1;
                            }
                            if (otherParticleHull.gameObject.tag == "spaceship")
                            {
                                otherParticleHull.gameObject.GetComponent<SpaceshipManagerScript>().collisions += 1;
                            }
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
                currentParticleHull = particles[i].GetComponent<Particle2D>().colHull;
                if (currentParticleHull.colliding)
                {
                    currentParticleHull.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    currentParticleHull.c.OrderContacts();
                    currentParticleHull.c.ResolveAllContacts();
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
