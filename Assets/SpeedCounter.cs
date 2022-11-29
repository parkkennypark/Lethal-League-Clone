using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedCounter : MonoBehaviour
{
    public TextMeshProUGUI text;

    float currentBallSpeed;

    void Start()
    {
        text.text = "0";
    }

    void OnEnable()
    {
        Ball.OnBallHit += OnBallHit;
    }

    void OnDisable()
    {
        Ball.OnBallHit -= OnBallHit;
    }

    void OnBallHit(int newSpeed, int maxSpeed, float hitRatio, float delay)
    {
        if (newSpeed == currentBallSpeed + 1)
        {
            text.text = newSpeed.ToString();
        }
        else
        {
            StartCoroutine(UpdateDisplay(newSpeed, delay));
        }
    }

    IEnumerator UpdateDisplay(int newSpeed, float delay)
    {

        float time = 0;
        float diff = newSpeed - currentBallSpeed;
        float numsPerSecond = diff / delay;

        while (time < delay)
        {
            float delta = Time.deltaTime * numsPerSecond;
            currentBallSpeed += delta;

            time += Time.deltaTime;

            text.text = Mathf.RoundToInt(currentBallSpeed).ToString();

            yield return null;
        }

        currentBallSpeed = newSpeed;
        text.text = Mathf.RoundToInt(currentBallSpeed).ToString();
    }
}
