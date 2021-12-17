using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class ScrollingMovement : MonoBehaviour
{
    public float speed;

    public float minX;
    
    private void Update()
    {
        transform.Translate(Vector2.left * (speed * Time.deltaTime));

        if (transform.position.x <= minX*(-1))
        {
            transform.position = new Vector2(minX, transform.position.y);
        }
    }
}
