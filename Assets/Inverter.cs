using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : MonoBehaviour
{


    public Animator animator;

    public int speedThreshold = 512;

    private RectTransform rectTransform;

    bool isInverted;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
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

    }

    void OnBallHit(int newSpeed, int maxSpeed, float hitRatio, float delay)
    {
        if (newSpeed < speedThreshold)
            return;


        Transform ball = FindObjectOfType<Ball>().transform;
        Vector3 screenPos = Camera.main.WorldToViewportPoint(ball.position);

        rectTransform.anchorMin = screenPos;
        rectTransform.anchorMax = screenPos;

        StartCoroutine(Invert(delay));
    }

    IEnumerator Invert(float delay)
    {
        animator.SetTrigger("Invert");
        yield return new WaitForSeconds(delay);
        animator.SetTrigger("Invert");

    }
}
