using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatoryMovement : MonoBehaviour
{
    Vector3 initialPos;
    public float XAxis = 0f;
    public float YAxis = 0f;
    public float speed = 1f;
    float curTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        initialPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        transform.position = new Vector3(this.transform.position.x + Mathf.Sin(Time.time) * speed * XAxis, this.transform.position.y + Mathf.Sin(Time.time * speed) * YAxis, 0f);

    }
}
