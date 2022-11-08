using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDelayFill : MonoBehaviour
{
    public Transform fillBar;

    void OnEnable()
    {
        Ball.OnBallHit += OnBallHit;
    }

    void OnDisable()
    {
        Ball.OnBallHit -= OnBallHit;
    }

    void OnBallHit(int newSpeed, float delay)
    {
        StartCoroutine(FillBar(delay));
    }

    IEnumerator FillBar(float delay)
    {
        float current = 0;
        while (current < delay)
        {
            float ratio = current / delay;

            fillBar.localScale = new Vector3(ratio, 1, 1);

            current += Time.deltaTime;
            yield return null;
        }
    }
}
