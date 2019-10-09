using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawnerScript : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public CollisionManager manager;
    public Vector2 initialVelocity;
    public float spawnTimeInterval;
    float elapsedTime;
    GameObject asteroid;
    public bool randomX, randomY;
    public float rangeMin, rangeMax;
    float offsetX = 0, offsetY = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > spawnTimeInterval && manager.particles.Count < 8)
        {
            asteroid = Instantiate(asteroidPrefab, GetComponent<Transform>().position, Quaternion.Euler(0, 0, 0));

            if (randomX)
            {
                offsetX = Random.Range(rangeMin, rangeMax);
            }
            if (randomY)
            {
                offsetY = Random.Range(rangeMin, rangeMax);
            }

            asteroid.GetComponent<Particle2D>().SetVelocityX(initialVelocity.x + offsetX);
            asteroid.GetComponent<Particle2D>().SetVelocityY(initialVelocity.y + offsetY);
            manager.particles.Add(asteroid);

            offsetX = 0;
            offsetY = 0;
            elapsedTime = 0.0f;
        }
    }
}
