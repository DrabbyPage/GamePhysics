using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PachinkoManagerScript : MonoBehaviour
{
    public float score;
    bool ballDropped;
    public Particle3D ball;
    public Vector3 leftForce;
    public Vector3 rightForce;
    public float vel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveBall();
    }

    void MoveBall()
    {
        if (!ballDropped)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && ball.particle3DTransform.position.x > -8.4)
            {
                ball.SetVelocityX(-vel);
            }
            else if(ball.particle3DTransform.position.x < -8.4)
            {
                ball.SetPositionX(-8.4f);
                ball.SetVelocityX(0);
            }
            if (Input.GetKey(KeyCode.RightArrow) && ball.particle3DTransform.position.x < 9.4)
            {
                ball.SetVelocityX(vel);
            }
            else if(ball.particle3DTransform.position.x >9.4)
            {
                ball.SetPositionX(9.4f);
                ball.SetVelocityX(0);
            }
            if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                ball.SetVelocityX(0);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                ball.forces.generateGravity = true;
                ballDropped = true;
            }
        }
    }
}
