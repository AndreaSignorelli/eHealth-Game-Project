using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{

    public float timeToDestroy = 1.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TimedSelfDestruction");
    }

    // Update is called once per frame
    IEnumerator TimedSelfDestruction()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(this.gameObject);
    }
}
