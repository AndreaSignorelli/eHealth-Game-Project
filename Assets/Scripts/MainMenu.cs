using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public AudioSource musicAudioSource;
    public AudioSource generalAudioSource;

    public float musicVolume = 1;
    public float generalVolume = 1;

    public Slider musicVolumeSlider;
    public Slider generalVolumeSlider;

    public Transform[] menuTabs;

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
            Focus(9);
        }
        else Focus(0);

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
        menuTabs[i].localScale = new Vector3(1f, 1f, 1f);
        Debug.Log(menuTabs[i].gameObject.name);

        if(i==1)
        {
            musicVolumeSlider.value = musicVolume;
            generalVolumeSlider.value = generalVolume;
        }
        
    }

    public void SetPlayerName()
    {
        string nome = GameObject.Find("EnterName").transform.Find("NomeText").GetComponent<Text>().text;
        PlayerPrefs.SetString("PlayerName", nome);
    }

}
