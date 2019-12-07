using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PachinkoManagerScript : MonoBehaviour
{
    public float score;
    bool ballDropped;
    public Particle3D ball;
    public Vector3 leftForce;
    public Vector3 rightForce;
    public float vel;
    public Text scoreText;
    public Particle3D farLeftArea;
    public Particle3D LeftArea;
    public Particle3D MidArea;
    public Particle3D rightArea;
    public Particle3D farRightArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveBall();
        UpdateScore();
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

    void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void SutractSevenPoints()
    {
        score = score - 7;
    }

    public void AddTwoPoints()
    {
        score = score +2;
    }

    public void AddTenPoints()
    {
        score = score + 10;
    }

}
