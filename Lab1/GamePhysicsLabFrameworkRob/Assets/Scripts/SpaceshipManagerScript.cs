using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipManagerScript : MonoBehaviour
{
    Particle2D particle;
    public Vector2 rotationThrusterStrength;
    public Vector2 rotationDrag;
    public Vector2 linearThrusterStrength;
    public int collisions = 0;
    public GameObject gameOverText;
    public TimerScript timerScript;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collisions < 3)
        {
            CheckPos();

            GetInput();
        }
        else
        {
            //Game over
            timerScript.spaceshipAlive = false;
            gameOverText.SetActive(true);
        }
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //particle.AddForce()
            Debug.Log("Rear Thruster");
            Vector2 angledThrust = (Vector2)transform.up *linearThrusterStrength.magnitude;
            particle.AddForce(angledThrust);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector2 angledThrust = -(Vector2)transform.up*linearThrusterStrength.magnitude;
            particle.AddForce(angledThrust);
            Debug.Log("Forward Thruster");
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Debug.Log("Port Thruster");
                particle.AddRotationForce(-rotationThrusterStrength);
            }
            if (Input.GetKey(KeyCode.RightArrow))
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

    void CheckPos()
    {
        if (transform.position.y > 11 || transform.position.y < -11 || transform.position.x > -31 || transform.position.x < -80)
        {
            gameOverText.SetActive(true);
            timerScript.spaceshipAlive = false;
        }

    }
}
