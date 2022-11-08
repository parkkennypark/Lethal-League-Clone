using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public delegate void BallAction(int newSpeed, float ballDelay);
    public static event BallAction OnBallHit;

    public delegate void BounceAction(float currentSpeed);
    public static event BounceAction OnHitWall;


    public int currentSpeed = 8;
    public int minSpeed = 8;
    public int maxSpeed = 128;
    public AnimationCurve velocityCurve;

    public float hitAngle = 35;

    public float minBallDelay = 0.2f;
    public float maxBallDelay = 2f;

    public Transform model;

    public float minStretch = 0;
    public float maxStretch = 0.3f;


    private Rigidbody2D rigidbody;

    enum HitType
    {
        UP,
        FORWARD,
        SMASH
    }

    enum HitDirection
    {
        LEFT,
        RIGHT
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rigidbody.velocity.magnitude > 0.1f)
            model.transform.up = rigidbody.velocity;

        float ratio = (float)(currentSpeed - minSpeed) / (maxSpeed - minSpeed);
        float scale = GetValue(ratio, minStretch, maxStretch);
        print(ratio + " " + scale);

        model.transform.localScale = new Vector3(1 - scale / 2, 1 + scale, 1 - scale / 2);

    }

    float GetValue(float ratio, float min, float max)
    {
        float val = ratio * (max - min) + min;
        val = Mathf.Clamp(val, min, max);

        return val;
    }

    float GetBallDelay(HitType type, float speed)
    {
        float delay = minBallDelay;

        if (true)
        // if (type == HitType.SMASH)
        {
            float ratio = speed / maxSpeed;
            delay = GetValue(ratio, minBallDelay, maxBallDelay);
        }

        return delay;
    }

    void AddOneToSpeed()
    {
        currentSpeed++;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
    }

    void DoubleSpeed()
    {
        currentSpeed *= 2;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
    }

    void HitInDirection(HitType type, HitDirection direction)
    {
        StopAllCoroutines();
        StartCoroutine(HitCoroutine(type, direction));
    }

    void StopBall()
    {
        rigidbody.velocity = Vector3.zero;
    }

    IEnumerator HitCoroutine(HitType type, HitDirection direction)
    {
        StopBall();

        if (type == HitType.SMASH)
            DoubleSpeed();
        else
            AddOneToSpeed();

        float delay = GetBallDelay(type, currentSpeed);

        if (OnBallHit != null)
            OnBallHit(currentSpeed, delay);

        yield return new WaitForSeconds(delay);

        float angle = 0;
        if (type == HitType.UP)
            angle = hitAngle;

        else if (type == HitType.FORWARD)
            angle = 0;

        else if (type == HitType.SMASH)
            angle = -hitAngle;

        Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right * (direction == HitDirection.RIGHT ? 1 : -1);

        float velocityMagnitude = velocityCurve.Evaluate((float)currentSpeed / maxSpeed);

        rigidbody.velocity = dir * velocityMagnitude * maxSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (OnHitWall != null)
            OnHitWall(currentSpeed);
    }
}
