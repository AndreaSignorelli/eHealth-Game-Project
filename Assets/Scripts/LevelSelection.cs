using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{

    public Button[] levelButtons;
    public int maxLevelUnlocked;
    List<Transform> stars;

    // Start is called before the first frame update
    void Start()
    {
        maxLevelUnlocked = PlayerPrefs.GetInt("MaxLevel");

        //Debug.Log()

        for(int i=maxLevelUnlocked; i<levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
            stars = new List<Transform>();
            stars.Add(levelButtons[i].transform.Find("Star1"));
            stars.Add(levelButtons[i].transform.Find("Star2"));
            stars.Add(levelButtons[i].transform.Find("Star3"));
            for (int k = 0; k < 3; k++)
            {
                stars[k].gameObject.SetActive(false);
            }
        }

        for(int j=0; j<maxLevelUnlocked; j++)
        {
            stars = new List<Transform>();
            int starCount = PlayerPrefs.GetInt("MaxStars"+(j+1).ToString());
            // = [levelButtons[j].transform.Find("Star1"), levelButtons[j].transform.Find("Star2"), levelButtons[j].transform.Find("Star3")];
            stars.Add(levelButtons[j].transform.Find("Star1"));
            stars.Add(levelButtons[j].transform.Find("Star2"));
            stars.Add(levelButtons[j].transform.Find("Star3"));
            for (int k = 0; k < 3; k++)
            {
                stars[k].gameObject.SetActive(false);
            }
            for ( int k=0; k<starCount; k++)
            {
                stars[k].gameObject.SetActive(true);
            }
        }

    }

    public void PlayGame(int n)
    {
        SceneManager.LoadScene(n);
    }

    public bool IsBossLevelUnlocked()
    {
        for (int j = 1; j < 5; j++)
        {
            int starCount = PlayerPrefs.GetInt("MaxStars" + (j).ToString());
            if (starCount < 3)
            {
                return false;
            }
        }
        return true;
    }
}
