using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCheckScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 20 || transform.position.y < -14 || transform.position.x > -28 || transform.position.x < -83)
        {
            GameObject.Find("CollisionManager").GetComponent<CollisionManager>().particles.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
