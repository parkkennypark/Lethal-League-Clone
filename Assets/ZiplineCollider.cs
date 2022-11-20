using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineCollider : MonoBehaviour
{
    public float speed;

    private bool collided;

    void Update()
    {
        if (!collided)
            transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ZiplineCollider>())
            return;

        print(name + " collided!");
        collided = true;
    }
}
