using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TimerToMainMenu : MonoBehaviour
{

    public VideoPlayer vp;
    double videoTime;

    // Start is called before the first frame update
    void Start()
    {
        videoTime = vp.length;
        StartCoroutine("WaitForMainMenu");
    }

    // Update is called once per frame
    IEnumerator WaitForMainMenu()
    {
        yield return new WaitForSeconds((float)videoTime);
        SceneManager.LoadScene("MainMenu");
    }
}
