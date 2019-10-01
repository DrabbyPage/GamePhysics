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
    void Update()
    {
        CollisionHull2D currentParticleHull, otherParticleHull;
        bool checkCollision = false;


        // Go through list to compare current particle to every particle after it in the list
        for (int i = 0; i < particles.Count; i++)
        {
            if(particles[i]!= null)
            {
                currentParticleHull = particles[i].GetComponent<Particle2D>().colHull;
                for (int j = i + 1; j < particles.Count; j++)
                {
                    if(particles[j] != null )
                    {
                        otherParticleHull = particles[j].GetComponent<Particle2D>().colHull;

                        // Determine which type the second particle is
                        switch (otherParticleHull.type)
                        {
                            // If it's AABB, look for that specific componenet
                            case CollisionHull2D.HULLTYPE.hull_aabb:
                                checkCollision = currentParticleHull.TestCollisionVSAABB(particles[j].GetComponent<AxisAlignBoundingBoxHull2D>(), ref particles[j].GetComponent<AxisAlignBoundingBoxHull2D>().c);
                                Debug.Log("AABB Success");
                                break;
                            // If it's circle, look for that specific componenet
                            case CollisionHull2D.HULLTYPE.hull_circle:
                                checkCollision = currentParticleHull.TestCollisionVSCircle(particles[j].GetComponent<CircleCollisionHull2D>(), ref particles[j].GetComponent<CircleCollisionHull2D>().c);
                                Debug.Log("cirlce Success");
                                break;
                            // If it's OBB, look for that specific componenet
                            case CollisionHull2D.HULLTYPE.hull_obb:
                                checkCollision = currentParticleHull.TestCollisionVSOBB(particles[j].GetComponent<ObjectBoundingBoxHull2D>(), ref particles[j].GetComponent<ObjectBoundingBoxHull2D>().c);
                                Debug.Log("OBB Success");
                                break;
                        }

                        // If the two objects collide, change their color to red
                        if (checkCollision)
                        {
                            changeObjectColor(currentParticleHull.gameObject, otherParticleHull.gameObject, Color.red);
                        }
                        else
                        {
                            //changeObjectColor(currentParticleHull.gameObject, otherParticleHull.gameObject, Color.green);
                        }
                    }
                    
                }
            }
            
        }        
    }

    void changeObjectColor(GameObject currentParticle, GameObject otherParticle, Color color)
    {
        currentParticle.GetComponent<Renderer>().material.color = color;
        otherParticle.GetComponent<Renderer>().material.color = color;
    }
}
