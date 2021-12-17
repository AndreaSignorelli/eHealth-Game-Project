using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{

    public float minSpeed = 1f;
    public float maxSpeed = 10f;
    public float minSize = 0.15f;
    public float maxSize = 0.35f;
    public float minVelocity = 1f;
    public float maxVelocity = 5f;
    float speed;
    float size;
    float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        size = Random.Range(minSize, maxSize);
        movementSpeed = Random.Range(minVelocity, maxVelocity);
        transform.localScale = new Vector3(size, size, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1) * speed * Time.deltaTime);
        transform.Translate(new Vector3(-1,0,0) * movementSpeed * Time.deltaTime, Space.World);
    }
}
