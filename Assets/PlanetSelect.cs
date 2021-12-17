using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetSelect : MonoBehaviour
{
    
    public void GotoEarthIntro ()
    {
        SceneManager.LoadScene("EarthIntro");
    }
    
    public void QuitGame ()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
