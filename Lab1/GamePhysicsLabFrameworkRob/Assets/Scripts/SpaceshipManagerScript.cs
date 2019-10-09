using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipManagerScript : MonoBehaviour
{
    Particle2D particle;
    public Vector2 rotationThrusterStrength;
    public Vector2 rotationDrag;
    public Vector2 linearThrusterStrength;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //particle.AddForce()
            Debug.Log("Rear Thruster");
            Vector2 angledThrust = new Vector2(linearThrusterStrength.x * Mathf.Cos(particle.rotation), linearThrusterStrength.y * Mathf.Sin(particle.rotation));
            particle.AddForce(angledThrust);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector2 angledThrust = new Vector2(-linearThrusterStrength.x * Mathf.Cos(Mathf.Rad2Deg * particle.rotation), linearThrusterStrength.y * Mathf.Sin(Mathf.Rad2Deg * particle.rotation));
            particle.AddForce(angledThrust);
            Debug.Log("Forward Thruster");
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Debug.Log("Port Thruster");
                particle.AddRotationForce(-rotationThrusterStrength);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Debug.Log("Starboard Thruster");
                particle.AddRotationForce(rotationThrusterStrength);
            }
        }
        else 
        {
            particle.AddRotationForce(-0.5f * particle.angForce);
            particle.angularVelocity *= 0.95f;
        }
    }
}
