using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    Text timeText;
    float time;
    public bool spaceshipAlive;
    // Start is called before the first frame update
    void Start()
    {
        timeText = GetComponent<Text>();
        time = 0.0f;
        spaceshipAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (spaceshipAlive)
        {
            time += Time.deltaTime;
            UpdateLevelTimer(time);
        }
    }

    public void UpdateLevelTimer(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.RoundToInt(totalSeconds % 60f);

        string formatedSeconds = seconds.ToString();

        if (seconds == 60)
        {
            seconds = 0;
            minutes += 1;
        }

        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
