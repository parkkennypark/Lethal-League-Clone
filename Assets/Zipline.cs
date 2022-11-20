using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zipline : MonoBehaviour
{
    public ZiplineCollider UpCollider, DownCollider;
    public LineRenderer lineRenderer;
    public Transform collisionBox;
    public int health = 3;

    void Update()
    {
        lineRenderer.SetPosition(0, UpCollider.transform.position);    
        lineRenderer.SetPosition(1, DownCollider.transform.position);    

        Vector3 vec = UpCollider.transform.position + DownCollider.transform.position;
        float dist = Vector3.Distance(UpCollider.transform.position, DownCollider.transform.position);
        collisionBox.transform.position = vec / 2;
        collisionBox.localScale = new Vector3(1, dist, 1);
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        print(collisionInfo.collider);
        if (collisionInfo.collider.GetComponent<Ball>())
        {
            health--;
            if (health <= 0)
                Destroy(gameObject);
        }
    }
}
