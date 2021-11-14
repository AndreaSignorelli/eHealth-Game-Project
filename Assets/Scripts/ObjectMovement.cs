using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMovement : MonoBehaviour
{

    Transform idlePoint;
    Text wordText;
    public float initSpeed = 2;
    public float exitSpeed = 5;
    public float maxDistance = 0.05f;
    public int movementType = 0; //0=initial movement, 1=no movements required, 2=ending movement

    public GameObject playerExplosion;

    public bool isAngry = false;

    public string thisCategory = "soft"; //or hard
    public string thisName = "funghetto";

    // Start is called before the first frame update
    void Start()
    {
        idlePoint = GameObject.Find("Object Idle Point").transform;
        wordText = GameObject.Find("Word Text").GetComponent<Text>();
        SpawnText();
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

        if (movementType == 2)
        {
            transform.Translate(Vector2.left * exitSpeed * Time.deltaTime);
        }
    }

    public void SpawnText()
    {
        wordText.text = thisName;
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
        }
    }

    public void CheckResult(string choice)
    {
        //here I check if the input (choice) is the same as the category of the current card.
        //If it is, I increase the score by one. Otherwise, nothing happens.
        if (choice == thisCategory)
        {
            Debug.Log("Correct!");
            //score++;
            GetCaptured();
        }
        else
        {
            Debug.Log("Error");
            Charge();
        }
        //I destroy the current card
        //Destroy(curCard);
        //I increase the number of the current card, so I will spawn the next card in the deck
        //curCardNumber++;
        //If the number is not higher than the number of the cards available for the level, I proceed to create the next card
        //if (curCardNumber < cardsList.Count)
        //{
        //SpawnCard(curCardNumber);
        //}
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

}


