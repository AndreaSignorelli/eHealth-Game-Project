using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroDialogue : MonoBehaviour
{

    public Text dialogueBox;
    public Dialogue levelDialogue;
    public Button dialogueButton;
    string playerName;
    public bool intro = true;

    // Start is called before the first frame update
    void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName");
        PrintDialogue(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintDialogue(int lineIndex)
    {
        dialogueButton.interactable = false;
        levelDialogue.allLines[lineIndex] = levelDialogue.allLines[lineIndex].Replace("$", playerName);
        dialogueBox.text = levelDialogue.allLines[lineIndex];
        dialogueButton.interactable = true;
        lineIndex++;
        if (lineIndex >= levelDialogue.allLines.Length)
        {
            //dialogo finito
            if (intro)
            {
                dialogueButton.onClick.AddListener(() => StartTutorial());
            } else
            {
                dialogueButton.onClick.AddListener(() => LoadMainMenu());
            }
            if (intro)
            {
                dialogueButton.transform.Find("Text").GetComponent<Text>().text = "VAI!";
            } else
            {
                dialogueButton.transform.Find("Text").GetComponent<Text>().text = "Grazie!";
            }
        }
        else
        {
            //procedi nel dialogo
            dialogueButton.transform.Find("Text").GetComponent<Text>().text = "Avanti";
            dialogueButton.onClick.AddListener(() => PrintDialogue(lineIndex));
        }
    }

    public void StartTutorial()
    {
        //SceneManager.LoadScene("Tutorial");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
