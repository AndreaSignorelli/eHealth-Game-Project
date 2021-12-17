using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void GoToEarthIntro ()
    {
        SceneManager.LoadScene("EarthIntro");
    }
    
    public void GoToMap ()
    {
        SceneManager.LoadScene("ToTheLevels-Disabled Planets");
    }

}
