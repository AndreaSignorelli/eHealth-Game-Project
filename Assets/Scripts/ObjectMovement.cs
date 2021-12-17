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

    public LevelManager levelManager;

    public bool last = false;

    GameObject idlePose;
    GameObject damagePose;
    GameObject attackPose;

    // Start is called before the first frame update
    void Start()
    {
        idlePoint = GameObject.Find("Object Idle Point").transform;
        //LevelManager levelManager = GameObject.Find("GameManager").GetComponent<"LevelManager">();
        //Text smth = GameObject.Find("Timer").GetComponent<Text>();
        levelManager = GameObject.Find("GameManager").GetComponent<LevelManager>();
        idlePose = transform.Find("Idle").gameObject;
        damagePose = transform.Find("Damage").gameObject;
        attackPose = transform.Find("Attack").gameObject;

        idlePose.SetActive(false);
        damagePose.SetActive(false);
        attackPose.SetActive(false);

        levelManager.generalSource.clip = levelManager.allLevelSounds[4];
        levelManager.generalSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementType == 0)
        {
            if (Vector3.Distance(transform.position, idlePoint.position) > maxDistance)
            {
                attackPose.SetActive(true);
                transform.Translate((idlePoint.position - transform.position) * initSpeed * Time.deltaTime);
            }
            else
            {
                attackPose.SetActive(false);
                damagePose.SetActive(false);
                idlePose.SetActive(true);
                movementType = 1;
                levelManager.StartTimer();
                levelManager.PlayWordSound();
            }
        }

        if (movementType == 1)
        {
            transform.position = new Vector3(transform.position.x, this.transform.position.y + Mathf.Sin(Time.time) * oscillationRange, transform.position.z);
        }

        if (movementType == 2)
        {
            transform.Translate(Vector2.left * exitSpeed * Time.deltaTime);
        }

        if(movementType==3)
        {
            
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
        attackPose.SetActive(true);
        idlePose.SetActive(false);
        damagePose.SetActive(false);
    }

    public void GetCaptured()
    {
        //play the get captured animation
        isAngry = false;
        movementType = 3;
        StartCoroutine(ChangeColorOverTime(GetComponent<SpriteRenderer>().color, Color.green, 1.0f));
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 150f);
        GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        idlePose.SetActive(false);
        damagePose.SetActive(true);
        attackPose.SetActive(false);


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && !isAngry)
        {
            //Destroy(this.gameObject);
            this.transform.position = new Vector3(9999, 99999, 0);
            if(last)
            {
                StartCoroutine(levelManager.FinishLevel());
            }
        }
        else if (other.gameObject.tag == "Player" && isAngry)
        {
            other.gameObject.GetComponent<SpaceshipMovement>().TakeDamage();
            StartCoroutine(DestroyAfterTime(timeToDestroy));
            if (last)
            {
                StartCoroutine(levelManager.FinishLevel());
            }
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


