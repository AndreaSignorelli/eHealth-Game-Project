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

    public void Awake()
    {
        //ResetAllPlayerPrefs();
        //PlayerPrefs.SetString("FirstPlay", "");
        if (PlayerPrefs.GetString("FirstPlay") == "")
        {
            ResetAllPlayerPrefs();
            PlayerPrefs.SetString("FirstPlay", "ciao");
            //Debug.Log("debug1");
        }

        if (PlayerPrefs.GetString("PlayerName") == "")
        {
            Focus(1);
            //Debug.Log("debug2");
            PlayerPrefs.SetInt("MaxLevel", 1);
        }
        else
        {
            //Debug.Log("debug3");
            Focus(1);
        }

        SpawnBadges();
    }
    
    public void Start()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        generalVolume = PlayerPrefs.GetFloat("GeneralVolume");

        if(musicVolume==0 || musicVolume==1)
        {
            musicVolume = 0.2f;
            generalVolume = 0.5f;
        }

        musicVolumeSlider.value = musicVolume;
        generalVolumeSlider.value = generalVolume;

        musicAudioSource.volume = musicVolume;
        generalAudioSource.volume = generalVolume;

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
        for (int j = 0; j < menuTabs.Length; j++)
        {
            menuTabs[j].localScale = new Vector3(0f, 0f, 0f);
        }
        switch (i)
        {
            case 1:
                menuTabs[0].localScale = new Vector3(1f, 1f, 1);
                menuTabs[1].localScale = new Vector3(1f, 1f, 1);
                menuTabs[2].localScale = new Vector3(0f, 0f, 1);
                musicVolumeSlider.value = musicVolume;
                generalVolumeSlider.value = generalVolume;
                break;
            case 2:
                menuTabs[0].localScale = new Vector3(1f, 1f, 1);
                menuTabs[2].localScale = new Vector3(1f, 1f, 1);
                menuTabs[1].localScale = new Vector3(0f, 0f, 1);
                break;
            default:
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
        
        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Report.txt";
        Debug.Log(path);
        if(!File.Exists(path))
        {
            File.WriteAllText(path, "REPORT FILE");
        }
        //StreamWriter writer = new StreamWriter(path, true);
        File.AppendAllText(path, PlayerPrefs.GetString("Report"));
        //writer.WriteLine(PlayerPrefs.GetString("Report"));
        //writer.Close();
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

    public void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}
