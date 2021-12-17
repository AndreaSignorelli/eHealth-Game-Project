using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{

    Transform idlePoint;
    public float initSpeed = 2;
    public float exitSpeed = 5;
    public float maxDistance = 0.05f;
    public int movementType = 0; //0=initial movement, 1=no movements required, 2=ending movement

    public GameObject playerExplosion;

    public float oscillationRange = 0.0003f;

    public GameObject exitFireEffect;
    
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        idlePoint = GameObject.Find("Idle Point").transform;
        exitFireEffect = GameObject.Find("exitFireEffect");

        levelManager = GameObject.Find("GameManager").GetComponent<LevelManager>();
        levelManager.generalSource.clip = levelManager.allLevelSounds[3];
        levelManager.generalSource.Play();
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

        if(movementType==1)
        {
            transform.position = new Vector3(transform.position.x, this.transform.position.y + Mathf.Sin(Time.time) * oscillationRange, transform.position.z);
        }

        if(movementType==2)
        {
            transform.Translate(Vector2.right * exitSpeed * Time.deltaTime);
            exitFireEffect.GetComponent<ParticleSystem>().Play();
        }

        if (Input.GetKeyDown(KeyCode.Space)) TakeDamage();
    }

    public void TakeDamage()
    {
        //play take damage animation
        GameObject explosion = Instantiate(playerExplosion, transform.position, transform.rotation);
        //explosion.GetComponent<Animator>().Play('ExplosionAnimation');
        Destroy(explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        levelManager.generalSource.clip = levelManager.allLevelSounds[2];
        levelManager.generalSource.Play();
    }
}
