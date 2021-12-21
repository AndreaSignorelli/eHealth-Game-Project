using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;
using System.IO;

public class MainMenu : MonoBehaviour
{

    public AudioSource musicAudioSource;
    public AudioSource generalAudioSource;

    public float musicVolume = 1;
    public float generalVolume = 1;

    public Slider musicVolumeSlider;
    public Slider generalVolumeSlider;

    public Transform[] menuTabs;

    public GameObject[] Badges;

    public void Start()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        generalVolume = PlayerPrefs.GetFloat("GeneralVolume");

        musicVolumeSlider.value = musicVolume;
        generalVolumeSlider.value = generalVolume;

        musicAudioSource.volume = musicVolume;
        generalAudioSource.volume = generalVolume;

        if(PlayerPrefs.GetString("PlayerName")=="")
        {
            Focus(1);
        }
        else Focus(0);

        SpawnBadges();

    }

    public void PlayGame ()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void QuitGame ()
    {
        //SceneManager.LoadScene("MainMenu");
        Debug.Log("quitted");
    }
    
    public void SetMusicVolume (float new_volume)
    {
        musicVolume = new_volume;
        musicAudioSource.volume = musicVolume;
        Debug.Log(new_volume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetGeneralVolume(float new_volume)
    {
        generalVolume = new_volume;
        generalAudioSource.volume = new_volume;
        Debug.Log(new_volume);
        PlayerPrefs.SetFloat("GeneralVolume", generalVolume);
    }

    public void PlayClickSound()
    {
        generalAudioSource.Play();
    }

    public void Focus(int i) //0 = main menu, 1 = options, 2 = exit, 3 = level selection
    {
        switch (i)
        {
            case 1:
                menuTabs[1].localScale = new Vector3(1f, 1f, 1);
                menuTabs[2].localScale = new Vector3(0f, 0f, 1);
                musicVolumeSlider.value = musicVolume;
                generalVolumeSlider.value = generalVolume;
                break;
            case 2:
                menuTabs[2].localScale = new Vector3(1f, 1f, 1);
                menuTabs[1].localScale = new Vector3(0f, 0f, 1);
                break;
            default:
                for (int j = 0; j < menuTabs.Length; j++)
                {
                    menuTabs[j].localScale = new Vector3(0f, 0f, 0f);
                }
                menuTabs[i].localScale = new Vector3(1f, 1f, 1f);
                Debug.Log(menuTabs[i].gameObject.name);
                break;
        }
    }

    public void SetPlayerName()
    {
        string nome = GameObject.Find("EnterName").transform.Find("NomeText").GetComponent<Text>().text;
        PlayerPrefs.SetString("PlayerName", nome);
    }

    public void PrintReport()
    {
        string path = "Assets/Resources/report" + System.DateTime.Now + ".txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(PlayerPrefs.GetString("Report"));
        writer.Close();
    }

    public void SpawnBadges()
    {
        for (int j = 1; j <= 5; j++)
        {
            int isBadge = PlayerPrefs.GetInt("Badge" + (j).ToString());
            if (isBadge!=1)
            {
                Badges[j - 1].SetActive(false);
            }
        }
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("Intro_prova");
    }

}
