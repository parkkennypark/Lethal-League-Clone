using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    void OnEnable()
    {
        Ball.OnHitWall += OnBallHitWall;
        Ball.OnBallHit += OnBallHit;
    }

    void OnDisable()
    {
        Ball.OnHitWall -= OnBallHitWall;
        Ball.OnBallHit -= OnBallHit;
    }

    void OnBallHitWall(float speed)
    {
        // GetComponent<StressReceiver>().InduceStress(0.2f);
        GetComponent<StressReceiver>().InduceStress(1);
    }

    void OnBallHit(int newSpeed, float delay)
    {
        GetComponent<StressReceiver>().InduceStress(2);
    }
}
