using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public Player player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        return;
        if (other.GetComponent<Ball>())
        {
            player.HitBall(other.GetComponent<Ball>());
        }
    }
}
