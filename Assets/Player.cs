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
        if (Input.GetButtonDown("Fire1"))
        {
            float vertical = Input.GetAxisRaw("Vertical");
            if (vertical > 0.2f)
                animator.SetTrigger("Upward");
            else if (vertical < -0.2f)
                animator.SetTrigger("Smash");
            else
                animator.SetTrigger("Foreward");
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
}
