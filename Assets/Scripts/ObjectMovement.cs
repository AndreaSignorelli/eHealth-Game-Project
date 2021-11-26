using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMovement : MonoBehaviour
{

    Transform idlePoint;

    public float initSpeed = 2;
    public float exitSpeed = 5;
    public float maxDistance = 0.05f;
    public int movementType = 0; //0=initial movement, 1=no movements required, 2=ending movement

    public GameObject playerExplosion;

    public bool isAngry = false;

    public Categoria thisCategory; //soft or hard
    public string thisName;

    public float oscillationRange = 0.0003f;

    public float timeToDestroy = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        idlePoint = GameObject.Find("Object Idle Point").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementType == 0)
        {
            if (Vector3.Distance(transform.position, idlePoint.position) > maxDistance)
            {
                transform.Translate((idlePoint.position - transform.position) * initSpeed * Time.deltaTime);
            }
            else movementType = 1;
        }

        if (movementType == 1)
        {
            transform.position = new Vector3(transform.position.x, this.transform.position.y + Mathf.Sin(Time.time) * oscillationRange, transform.position.z);
        }

        if (movementType == 2)
        {
            transform.Translate(Vector2.left * exitSpeed * Time.deltaTime);
        }
    }

    //public void SpawnText()
    //{
    //wordText.text = thisName;
    //}

    public void GetSpecifics(Categoria c, string n)
    {
        thisCategory = c;
        thisName = n;
    }

    public void Charge()
    {
        //play the angry animation
        isAngry = true;
        movementType = 2;
        StartCoroutine(ChangeColorOverTime(GetComponent<SpriteRenderer>().color, Color.red, 1.0f));
    }

    public void GetCaptured()
    {
        //play the get captured animation
        isAngry = false;
        movementType = 2;
        StartCoroutine(ChangeColorOverTime(GetComponent<SpriteRenderer>().color, Color.green, 1.0f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && !isAngry)
        {
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Player" && isAngry)
        {
            other.gameObject.GetComponent<SpaceshipMovement>().TakeDamage();
            StartCoroutine(DestroyAfterTime(timeToDestroy));
        }
    }

    IEnumerator ChangeColorOverTime(Color start, Color end, float duration)
    {
        Color someColorValue;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            someColorValue = Color.Lerp(start, end, normalizedTime);
            GetComponent<SpriteRenderer>().color = someColorValue;
            yield return null;
        }
        someColorValue = end; //without this, the value will end at something like 0.9992367
        GetComponent<SpriteRenderer>().color = someColorValue;
    }

    IEnumerator DestroyAfterTime(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(this.gameObject);
    }

}


