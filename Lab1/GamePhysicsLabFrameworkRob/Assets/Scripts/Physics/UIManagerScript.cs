using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    public bool kinRot, kinPos;

    public float inputVelX, inputVelY, inputAccX, inputAccY, inputAngVel, inputAngAcc;

    GameObject cube;

    #region InfoPanelStuff

    public Text currentPosText, currentVelText, currentAccText, currentRotText, currentRotVelText, currentRotAccText;

    #endregion

    #region InputPanelStuff

    public Text inputVelXText, inputVelYText, inputAccXText, inputAccYText, inputAngVelText, inputAngAccText;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cube = GameObject.Find("Cube");
    }

    void Update()
    {
        UpdateInputField();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateInfoPanel();
    }

    void UpdateInfoPanel()
    {
        currentPosText.text = "Current Pos: (" + (cube.transform.position.x).ToString("0.00") + ", " + (cube.transform.position.y).ToString("0.00") + ", " + (cube.transform.position.z).ToString("0.00") + ")";
        currentVelText.text = "Current Vel: (" + (cube.GetComponent<Particle2D>().velocity.x).ToString("0.00") + ", "+ (cube.GetComponent<Particle2D>().velocity.y).ToString("0.00") + ")";
        currentAccText.text = "Current Acc: (" + (cube.GetComponent<Particle2D>().acceleration.x).ToString("0.00") + ", "+ (cube.GetComponent<Particle2D>().acceleration.y).ToString("0.00") + ")";

        currentRotText.text = "Current Rot: " + (Mathf.Repeat(cube.GetComponent<Particle2D>().rotation, 360)).ToString("0.00");
        //currentRotText.text = "Current Rot: " + (cube.GetComponent<Particle2D>().rotation.ToString("0.00"));
        currentRotVelText.text = "Current RotVel: " + (cube.GetComponent<Particle2D>().angularVelocity).ToString("0.00");
        currentRotAccText.text = "Current RotAcc: " + (cube.GetComponent<Particle2D>().angularAcceleration).ToString("0.00");
    }

    void UpdateInputField()
    {
        if (inputVelXText.text != "")
            ChangeVelX(float.Parse(inputVelXText.text));
        else
            ChangeVelX(0);

        if (inputVelYText.text != "")
            ChangeVelY(float.Parse(inputVelYText.text));
        else
            ChangeVelY(0);

        if (inputAccXText.text != "")
            ChangeAccX(float.Parse(inputAccXText.text));
        else
            ChangeAccX(0);

        if (inputAccYText.text != "")
            ChangeAccY(float.Parse(inputAccYText.text));
        else
            ChangeAccY(0);

        if (inputAngVelText.text != "")
            ChangeAngVel(float.Parse(inputAngVelText.text));
        else
            ChangeAngVel(0);

        if (inputAngAccText.text != "")
            ChangeAngAcc(float.Parse(inputAngAccText.text));
        else
            ChangeAngAcc(0);
    }

    #region Wrapper functions

    public void ChangeKinRot()
    {
        kinRot = !kinRot;
    }

    public void ChangeKinPos()
    {
        kinPos = !kinPos;
    }

    public void ChangeVelX(float newVel)
    {
        inputVelX = newVel;
        cube.GetComponent<Particle2D>().SetVelocityX(inputVelX);
    }

    public void ChangeVelY(float newVel)
    {
        inputVelY = newVel;
        cube.GetComponent<Particle2D>().SetVelocityY(inputVelY);
    }

    public void ChangeAccX(float newAcc)
    {
        inputAccX = newAcc;
        cube.GetComponent<Particle2D>().SetAccelerationX(inputAccX);
    }

    public void ChangeAccY(float newAcc)
    {
        inputAccY = newAcc;
        cube.GetComponent<Particle2D>().SetAccelerationY(inputAccY);
    }

    public void ChangeAngVel(float newAngVel)
    {
        inputAngVel = newAngVel;
        cube.GetComponent<Particle2D>().SetAngularVelocity(inputAngVel);
    }

    public void ChangeAngAcc(float newAngAcc)
    {
        Debug.Log("Got Here");
        Debug.Log(newAngAcc);
        inputAngAcc = newAngAcc;
        cube.GetComponent<Particle2D>().SetAngularAcceleration(inputAngAcc);
    }

    #endregion

}
