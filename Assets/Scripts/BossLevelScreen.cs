using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLevelScreen : MonoBehaviour
{
    void Start()
    {
        transform.Find("GoButton").GetComponent<Button>().interactable = Camera.main.gameObject.GetComponent<LevelSelection>().IsBossLevelUnlocked();
    }

}
