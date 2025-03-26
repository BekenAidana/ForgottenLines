using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    float halfWidth, halfHeight;
    string gameObjectName;
    float repelForce = 5f;
    float minX=-15f, maxX=15f;
    float minY=-9f, maxY=9f;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        gameObjectName = gameObject.name;
        halfWidth = col.bounds.extents.x;
        halfHeight = col.bounds.extents.y;
    }
    void FixedUpdate()
    {
        if (rb == null || col == null) return; 

        maxX = LevelManager.instance.MaxX;
        Vector2 force = Vector2.zero;
        
        bool unconstrainX = false;
        bool unconstrainY = false;
        float halfWidth = col.bounds.extents.x;
        float halfHeight = col.bounds.extents.y;

        bool isOutOfBounds = false;

        if (transform.position.x - halfWidth <= minX)
        {
            force += Vector2.right * repelForce;
            isOutOfBounds = true;
            unconstrainX = true;
        }
        else if (transform.position.x + halfWidth >= maxX)
        {
            force += Vector2.left * repelForce;
            isOutOfBounds = true;
            unconstrainX = true;
        }

        if (transform.position.y - halfHeight <= minY)
        {
            force += Vector2.up * repelForce;
            isOutOfBounds = true;
            unconstrainY = true;
        }
        else if (transform.position.y + halfHeight >= maxY)
        {
            force += Vector2.down * repelForce;
            isOutOfBounds = true;
            unconstrainY = true;
        }

        rb.AddForce(force, ForceMode2D.Force);

        if(unconstrainX && gameObjectName=="Player")
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        if(unconstrainY && gameObjectName=="Player")
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;;
        }
        if (!isOutOfBounds)
        {
            if(gameObjectName=="Player")
            {
                Debug.Log("lol");
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            rb.velocity = Vector2.zero;
        }
    }
}
