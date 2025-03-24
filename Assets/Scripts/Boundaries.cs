using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    float halfWidth, halfHeight;
    float repelForce = 5f;
    float minX=-15f, maxX=15f;
    float minY=-9f, maxY=9f;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        halfWidth = col.bounds.extents.x;
        halfHeight = col.bounds.extents.y;
    }
    void FixedUpdate()
    {
        if (rb == null || col == null) return; 

        maxX = LevelManager.instance.MaxX;
        Vector2 force = Vector2.zero;

        float halfWidth = col.bounds.extents.x;
        float halfHeight = col.bounds.extents.y;

        bool isOutOfBounds = false;

        if (transform.position.x - halfWidth <= minX)
        {
            force += Vector2.right * repelForce;
            isOutOfBounds = true;
        }
        else if (transform.position.x + halfWidth >= maxX)
        {
            force += Vector2.left * repelForce;
            isOutOfBounds = true;
        }

        if (transform.position.y - halfHeight <= minY)
        {
            force += Vector2.up * repelForce;
            isOutOfBounds = true;
        }
        else if (transform.position.y + halfHeight >= maxY)
        {
            force += Vector2.down * repelForce;
            isOutOfBounds = true;
        }

        rb.AddForce(force, ForceMode2D.Force);

        // Останавливаем объект, если он уже внутри границ
        if (!isOutOfBounds)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
