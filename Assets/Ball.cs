using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType
{
    UP,
    FORWARD,
    SMASH
}

public enum HitDirection
{
    LEFT,
    RIGHT
}

public class Ball : MonoBehaviour
{
    public delegate void BallAction(int newSpeed, int maxSpeed, float hitRatio, float ballDelay);
    public static event BallAction OnBallHit;

    public delegate void BounceAction(float currentSpeed);
    public static event BounceAction OnHitWall;

    const int trueMaxSpeed = 1000000;
    const int trueMinSpeed = 8;

    public int currentSpeed = 4;
    public int minSpeed = 8;
    public int maxSpeed = 128;
    public float maxActualVelocity = 512;
    public AnimationCurve velocityCurve;

    public float hitAngle = 35;

    public float minBallDelay = 0.2f;
    public float maxBallDelay = 2f;

    public Transform model;

    public float minStretch = 0;
    public float maxStretch = 0.3f;


    private Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rigidbody.velocity.magnitude > 0.1f)
            model.transform.up = rigidbody.velocity;

        float scale = GetValue(GetHitRatio(), minStretch, maxStretch);

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

        // float log = Mathf.Log(speed, 2);
        // return speed * minBallDelay;

        float velocityMagnitude = velocityCurve.Evaluate((GetHitRatio()));
        return velocityMagnitude * maxBallDelay;


        return delay;
    }

    float GetHitRatio()
    {
        float maxLog = Mathf.Log(trueMaxSpeed, 2);
        float minLog = Mathf.Log(trueMinSpeed, 2);
        float log = Mathf.Log(currentSpeed, 2);

        float ratio = (log - minLog) / (maxLog - minLog);

        print("Ratio: " + ratio);

        return ratio;
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

    public void HitInDirection(HitType type, HitDirection direction)
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
        print("Delay: " + delay);


        if (OnBallHit != null)
            OnBallHit(currentSpeed, maxSpeed, GetHitRatio(), delay);

        yield return new WaitForSeconds(delay);

        float angle = 0;
        if (type == HitType.UP)
            angle = hitAngle;

        else if (type == HitType.FORWARD)
            angle = 0;

        else if (type == HitType.SMASH)
            angle = -hitAngle;

        int dirMult = (direction == HitDirection.RIGHT ? 1 : -1);
        Vector3 dir = Quaternion.Euler(0, 0, angle * dirMult) * Vector3.right * dirMult;

        float velocityMagnitude = velocityCurve.Evaluate((GetHitRatio()));
        // float velocityMagnitude = velocityCurve.Evaluate((float)currentSpeed / maxSpeed);

        rigidbody.velocity = dir * velocityMagnitude * maxActualVelocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (OnHitWall != null)
            OnHitWall(currentSpeed);

        // StartCoroutine(PauseBall(0.05f));
    }

    IEnumerator PauseBall(float delay)
    {
        rigidbody.simulated = false;
        yield return new WaitForSeconds(delay);
        rigidbody.simulated = true;
    }
}
