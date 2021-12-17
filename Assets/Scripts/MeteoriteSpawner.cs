using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour
{

    public GameObject[] meteorites;
    public float minTimeToWait = 2.0f;
    public float maxTimeToWait = 10.0f;
    public float yRange = 4f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("WaitAndSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(Random.Range(minTimeToWait,maxTimeToWait));
        int index = Random.Range(0, meteorites.Length);
        GameObject thisMeteorite = Instantiate(meteorites[index], new Vector3(15, Random.Range(yRange * (-1), yRange), 0), Quaternion.identity);
        StartCoroutine("WaitAndSpawn");
    }
}
