using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    StressReceiver stress;

    void Start()
    {
        stress = GetComponent<StressReceiver>();
    }

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
        stress.InduceStress(1);
    }

    void OnBallHit(int newSpeed, int maxSpeed, float hitRatio, float delay)
    {
        stress.InduceStress(2);
        StartCoroutine(SetConstantTrauma(delay, 1));
    }

    IEnumerator SetConstantTrauma(float delay, float trauma)
    {
        stress.constantTrauma = 1;
        yield return new WaitForSeconds(delay);
        stress.constantTrauma = 0;
    }
}
