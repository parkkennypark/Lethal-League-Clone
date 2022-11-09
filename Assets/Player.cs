using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    public int jumpStrength = 16;
    public float reversalStrength = 4;

    public float maxSpeed = 8;
    public float accel = 8;

    private bool hitStunned;

    HitType hitType;

    void Start()
    {
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     HitInDirection(HitType.UP, HitDirection.RIGHT);
        // }
        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     HitInDirection(HitType.FORWARD, HitDirection.RIGHT);
        // }
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     HitInDirection(HitType.SMASH, HitDirection.RIGHT);
        // }

    }

    void Update()
    {
        if (hitStunned)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            float vertical = Input.GetAxisRaw("Vertical");
            if (vertical > 0.2f)
            {
                hitType = HitType.UP;
                animator.SetTrigger("Upward");
            }
            else if (vertical < -0.2f)
            {
                hitType = HitType.SMASH;
                animator.SetTrigger("Smash");
            }
            else
            {
                hitType = HitType.FORWARD;
                animator.SetTrigger("Foreward");
            }
        }


        float horizInput = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(horizInput) > 0.1f)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizInput), 1, 1);
        }

        float current = rb.velocity.x;
        current = Mathf.Lerp(current, horizInput * maxSpeed, Time.deltaTime * accel);
        rb.velocity = new Vector2(current, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.up * jumpStrength;
        }

        if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.down * reversalStrength * Time.deltaTime;
        }
    }

    public void HitBall(Ball ball)
    {
        Ball.OnBallHit += OnBallHit;

        HitDirection direction = transform.localScale.x < 0 ? HitDirection.LEFT : HitDirection.RIGHT;
        ball.HitInDirection(hitType, direction);
    }

    void OnBallHit(int newSpeed, float delay)
    {
        StartCoroutine(OnBallHitCoroutine(delay));
    }

    IEnumerator OnBallHitCoroutine(float delay)
    {
        Ball.OnBallHit -= OnBallHit;

        hitStunned = true;
        rb.simulated = false;
        animator.speed = 0;

        yield return new WaitForSeconds(delay);

        hitStunned = false;
        rb.simulated = true;
        animator.speed = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Ball>())
        {
            HitBall(other.GetComponent<Ball>());
        }
    }
}
