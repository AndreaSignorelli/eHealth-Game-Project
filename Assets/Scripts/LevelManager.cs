using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<ProtoCard> cardsList;

    //names and the relative categories are added in the editor
    public string[] names;
    public Categoria[] categories;
    public Sprite[] pics;
    public bool[] isSound;
    public AudioClip[] audioClips;

    public GameObject enemyPrefab;
    public Transform enemySpawner;
    public GameObject curEnemy;

    //this is the general card prefab, we will modify it with the current card specifications
    public GameObject cardPrefab;

    //the Canvas is fundamental to make the cards visible in the scene
    GameObject canvas;

    //the score of the current level
    public int score = 0;

    //this is the current instantiated card
    GameObject curCard;

    //the index of the currently instantiated card
    int curCardNumber = 0;

    Text timerText;
    Slider timerBar;
    public bool timerOn = false;
    public float lastSpawnTime = 0f;
    public float timerDuration = 5.0f;
    float curTimerStatus = 0f;

    public bool buttonsOn = true;

    public bool levelEnded = false;

    SpaceshipMovement player;

    public string levelName = "C1";
    public string report;
    public float levelScore;
    public float[] scoreRequiredPerStar;// = [15f, 25f, 35f]; official score thresholds?

    public GameObject finishLevelScreen;

    float missingTimeToChangeColor;

    public GameObject star1, star2, star3;

    public Text dialogueBox;
    public Dialogue[] levelDialogues;
    public Sprite[] characters; //0=player, 1=friend, 2=adult, 3=friend's family, 4=boss
    //public string[] dialogueLines;
    //public int[] interlocutore; 
    public bool pause = false;
    Button dialogueButton;
    Image playerImage;
    Image characterImage;

    public Vector3 imgScale;

    float musicVolume;
    float generalVolume;

    public AudioClip[] allLevelSounds; //0=musica background, 1=correct sound, 2=wrong sound, 3=player arrival, 4=enemy spawn, 5=congrats at end level
    public AudioSource wordsSource;
    public AudioSource musicSource;
    public AudioSource generalSource;

    string playerName;

    void Awake()
    {

        playerName = PlayerPrefs.GetString("PlayerName");
        //playerName = "Roberto";

        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        generalVolume = PlayerPrefs.GetFloat("GeneralVolume");

        generalSource.volume = generalVolume;
        musicSource.volume = musicVolume;
        wordsSource.volume = 1;

        dialogueButton = GameObject.Find("DialogueButton").GetComponent<Button>();
        playerImage = GameObject.Find("playerImage").GetComponent<Image>();
        characterImage = GameObject.Find("characterImage").GetComponent<Image>(); ;
    }

    void Start()
    {
        ChangeButtonsState();
        finishLevelScreen.transform.localScale = new Vector3(0,0,0);
        //let's find the Canvas in the scene
        canvas = GameObject.Find("Canvas");
        timerText = GameObject.Find("Timer").GetComponent<Text>();
        timerBar = GameObject.Find("TimerBar").GetComponent<Slider>();
        enemySpawner = GameObject.Find("Enemy Spawner").transform;
        player = GameObject.Find("Spaceship").GetComponent<SpaceshipMovement>();

        Debug.Log(SceneManager.GetActiveScene().buildIndex);

        musicSource.clip = allLevelSounds[0];
        musicSource.Play();

        InitiateReport();

        //SpawnEnemy();

        //let's create all of the cards, and let's add them to the List cardsList
        for (int i = 0; i < names.Length; i++)
        {
            string curName = names[i];
            //I get the initial as the first letter of the name
            string curInitial = curName[0].ToString();
            //I create a card and I add it to the list
            cardsList.Add(new ProtoCard());
            cardsList.Last().Initialize_ProtoCard(curName, curInitial, categories[i], pics[i], isSound[i]);
        }
        //Let's spawn the first card of the level!
        //SpawnCard(curCardNumber);
        dialogueBox = GameObject.Find("Dialogue Box").GetComponent<Text>();
        PrintDialogueThenSpawn(0, 0, false);
        //SpawnEnemy();
    }

    public void PrintDialogueThenSpawn(int dialogueIndex, int lineIndex, bool oneRandomLine)
    {
        if(oneRandomLine)
        {
            lineIndex = Random.Range(0, levelDialogues[dialogueIndex].allLines.Length);
        }
        
        playerImage.gameObject.SetActive(false);
        characterImage.gameObject.SetActive(false);
        dialogueButton.interactable = false;
        pause = true;
        levelDialogues[dialogueIndex].allLines[lineIndex] = levelDialogues[dialogueIndex].allLines[lineIndex].Replace("$", playerName);
        dialogueBox.text = levelDialogues[dialogueIndex].allLines[lineIndex];
        if(levelDialogues[dialogueIndex].allInterlocutori[lineIndex]==0)
        {
            playerImage.gameObject.SetActive(true);
            playerImage.sprite = characters[levelDialogues[dialogueIndex].allInterlocutori[lineIndex]];
        } else
        {
            characterImage.gameObject.SetActive(true);
            characterImage.sprite = characters[levelDialogues[dialogueIndex].allInterlocutori[lineIndex]];
        }

        dialogueButton.interactable = true;
        if (!oneRandomLine)
        {
            lineIndex++;
            if (lineIndex >= levelDialogues[dialogueIndex].allLines.Length)
            {
                //dialogo finito
                dialogueButton.onClick.AddListener(() => StartEnemySpawn());
            }
            else
            {
                //procedi nel dialogo
                dialogueButton.onClick.AddListener(() => PrintDialogueThenSpawn(dialogueIndex, lineIndex, false));
            }
        }
        else
        {
            dialogueButton.onClick.AddListener(() => StartEnemySpawn());
        }
        
    }

    public void StartEnemySpawn()
    {
        playerImage.gameObject.SetActive(false);
        characterImage.gameObject.SetActive(false);
        dialogueButton.interactable = false;
        dialogueBox.text = "";
        pause = false;

        if (curCardNumber < cardsList.Count)
        {
            SpawnEnemy();
            if (cardsList[curCardNumber].GetSound)
            {
                GameObject.Find("Audio Button").transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                GameObject.Find("Audio Button").transform.localScale = new Vector3(0, 0, 0);
            }
        } else
        {
            StartCoroutine("FinishLevel");
        }
    }

    public void Update()
    {
        if (!pause)
        {
            if (timerOn)
            {
                curTimerStatus = timerDuration - (Time.time - lastSpawnTime);
                //Debug.Log(curTimerStatus);
                if (curTimerStatus <= 0)
                {
                    //StopTimer();
                    Debug.Log("Timer ended");
                    //chiama CheckResult con l'errore:
                    Categoria rightCategory = cardsList[curCardNumber].GetCategoria;
                    int categoriaSbagliata;
                    if (rightCategory == Categoria.Hard)
                    {
                        categoriaSbagliata = 1;
                    }
                    else
                    {
                        categoriaSbagliata = 0;
                    }
                    CheckResult(categoriaSbagliata);
                }
                else
                {
                    timerText.text = (System.Math.Round(curTimerStatus, 2)).ToString();
                    //change color of the timer if less than a second is left
                    if (curTimerStatus <= timerDuration / 5)
                    {
                        Color lerpedColor = Color.Lerp(Color.yellow, Color.red, missingTimeToChangeColor);
                        timerText.color = lerpedColor;
                        missingTimeToChangeColor -= Time.deltaTime;
                        if (missingTimeToChangeColor <= 0)
                        {
                            missingTimeToChangeColor = timerDuration / 8;
                        }
                    }
                    timerBar.value = curTimerStatus;
                }
            }

        }

    }

    public void SpawnEnemy()
    {
        if (curEnemy)
        {
            Destroy(curEnemy.gameObject);
        }
        curEnemy = Instantiate(enemyPrefab, enemySpawner.position, this.transform.rotation);

        if(curCardNumber==(cardsList.Count-1))
        {
            curEnemy.GetComponent<ObjectMovement>().last = true;
        }

        SpawnCard(curCardNumber);
    }

    public void SpawnCard(int n)
    {
        //I instantiate the general card prefab, and I assign it to the curCard variable. Its position is centered on the canvas' position, and its rotation is null
        //the card prefab will also have attached the various "Enemy" scripts; in this function I just spawn it and set the text elements
        curCard = Instantiate(cardPrefab, curEnemy.transform.Find("Carta spawner").position, this.transform.rotation);
        //I have to put the card as child of the Canvas, otherwise it will not be visible on screen (ONLY WORKS FOR UI ELEMENTS, it won't be a UI element in the future)
        curCard.transform.parent = curEnemy.transform;
        curEnemy.GetComponent<ObjectMovement>().GetSpecifics(cardsList[n].GetCategoria, cardsList[n].GetNome);
        //now I change the various texts on the instantiated card to match the one of the current card that has been drawn:
        curCard.transform.Find("Maiusc Word").GetComponent<TextMesh>().text = cardsList[n].GetNome.ToUpper();
        curCard.transform.Find("Little Word").GetComponent<TextMesh>().text = cardsList[n].GetNome.ToLower();
        curCard.transform.Find("Maiusc Letter").GetComponent<TextMesh>().text = cardsList[n].GetIniziale.ToUpper();
        curCard.transform.Find("Little Letter").GetComponent<TextMesh>().text = cardsList[n].GetIniziale.ToLower();
        curCard.transform.Find("Immagine").GetComponent<SpriteRenderer>().sprite = cardsList[n].GetPic;
        curCard.transform.Find("Immagine").transform.localScale = imgScale;

        if (cardsList[n].GetSound)
        {
            curCard.transform.Find("Maiusc Word").GetComponent<TextMesh>().text = "";
            curCard.transform.Find("Little Word").GetComponent<TextMesh>().text = "";
            curCard.transform.Find("Maiusc Letter").GetComponent<TextMesh>().text = "";
            curCard.transform.Find("Little Letter").GetComponent<TextMesh>().text = "";
            curCard.transform.Find("SoundBar").transform.localScale = new Vector3(0.05f, 0.05f, 1);
        }

    }

    public void PlayWordSound()
    {
        if (cardsList[curCardNumber].GetSound && !wordsSource.isPlaying)
        {
            wordsSource.clip = audioClips[curCardNumber];
            wordsSource.Play(0);
        }
    }

    public void StartTimer()
    {
        if (!timerOn)
        {
            lastSpawnTime = Time.time;
            timerOn = true;
        }
        timerText.color = Color.white;
        ChangeButtonsState();
    }

    public void StopTimer(string result)
    {
        timerOn = false;
        ChangeButtonsState();
        if (curTimerStatus <= 0)
        {
            curTimerStatus = 0f;
            timerText.color = Color.red;
        }
        else timerText.color = Color.white;
        AppendScoreToReport(curTimerStatus, result);
        curTimerStatus = 0f;
    }

    public void ChangeButtonsState()
    {
        buttonsOn = !buttonsOn;
        if(buttonsOn)
        {
            GameObject.Find("Soft").GetComponent<Button>().interactable = true;
            GameObject.Find("Hard").GetComponent<Button>().interactable = true;
        } else
        {
            GameObject.Find("Soft").GetComponent<Button>().interactable = false;
            GameObject.Find("Hard").GetComponent<Button>().interactable = false;
        }
    }


    //this function is called when I click one of the two buttons on screen. They will give 'soft' or 'hard' as input strings
    public void CheckResult(int choice)
    {
        //here I check if the input (choice) is the same as the category of the current card.

        string result;

        Categoria cat_choice;
        if (choice == 0)
            cat_choice = Categoria.Hard;
        else
            cat_choice = Categoria.Soft;

        if (cat_choice == cardsList[curCardNumber].GetCategoria)
        {
            Debug.Log("Correct!");
            result = "correct";
            score++;
            curEnemy.GetComponent<ObjectMovement>().GetCaptured();
            generalSource.clip = allLevelSounds[1];
            generalSource.Play();
        }
        else
        {
            Debug.Log("Error");
            result = "error";
            curEnemy.GetComponent<ObjectMovement>().Charge();
        }

        StopTimer(result);
        //I destroy the current card
        //Destroy(curCard);
        curEnemy = null;
        //I increase the number of the current card, so I will spawn the next card in the deck
        curCardNumber++;
        //If the number is not higher than the number of the cards available for the level, I proceed to create the next card
        if (result == "correct")
            PrintDialogueThenSpawn(1,0, true);
        else
            PrintDialogueThenSpawn(2, 0, true);
    }

    public IEnumerator FinishLevel()
    {
        levelEnded = true;
        yield return new WaitForSeconds(1.5f);
        SpaceShipEscapes();
        yield return new WaitForSeconds(3f);
        generalSource.clip = allLevelSounds[5];
        generalSource.Play();
        finishLevelScreen.transform.localScale = new Vector3(1, 1, 1);
        dialogueBox.gameObject.SetActive(false);
        SaveReport();
        //SceneManager.LoadScene("MainMenu");
        //spawn UI of end level, with scores etc
    }

    public void SpaceShipEscapes()
    {
        player.movementType = 2;
    }

    public void InitiateReport()
    {
        report = PlayerPrefs.GetString("Report");
        report = report + levelName + ": " + System.DateTime.Now.ToString("MM/dd") + "\n";
    }

    public void AppendScoreToReport(float timerStatus, string result)
    {
        Debug.Log("added score to report");
        float thisCardScore = timerStatus;
        int thisCardIndex = curCardNumber;
        report = report + thisCardIndex.ToString() + " - " + cardsList[thisCardIndex].GetNome + " " + curTimerStatus.ToString() + " " + result + "\n";
        Debug.Log(report);
        if (result == "correct")
        {
            levelScore += thisCardScore;
        }

    }

    public void SaveReport()
    {
        //calculate stars:
        int stars = 0;
        if(levelScore<scoreRequiredPerStar[2])
        {
            if (levelScore < scoreRequiredPerStar[1])
            {
                if (levelScore < scoreRequiredPerStar[0])
                {
                    stars = 0;
                    star1.SetActive(false);
                    star2.SetActive(false);
                    star3.SetActive(false);
                } 
                else
                {
                    stars = 1;
                    star1.SetActive(true);
                    star2.SetActive(false);
                    star3.SetActive(false);
                }
            }
            else
            {
                stars = 2;
                star1.SetActive(true);
                star2.SetActive(true);
                star3.SetActive(false);
            }
        }
        else
        {
            stars = 3;
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
        }

        //updating report with final level results:
        report = report + "Final score: " + levelScore + " - " + stars.ToString() + " Stars" + "\n\n";

        int previousStars = PlayerPrefs.GetInt("MaxStars" + SceneManager.GetActiveScene().buildIndex);
        if(stars>previousStars)
        {
            PlayerPrefs.SetInt("MaxStars" + SceneManager.GetActiveScene().buildIndex, stars);
        }

        //update UI
        
        if (stars>=1)
        {
            UnlockNextLevel();
        }

        //saving report
        PlayerPrefs.SetString("Report", report);
        Debug.Log("Report saved");

        if(PlayerPrefs.GetInt("MaxLevel") > SceneManager.GetActiveScene().buildIndex)
        {
            GameObject.Find("NextLevelButton").GetComponent<Button>().interactable = true;
        }

    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void UnlockNextLevel()
    {
        //unlock next level if not already unlocked
        int maxLevelUnlocked = PlayerPrefs.GetInt("MaxLevel");
        if(maxLevelUnlocked == SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt("MaxLevel", maxLevelUnlocked + 1);
            Debug.Log("Next Level Unlocked!");
            GameObject.Find("NextLevelButton").GetComponent<Button>().interactable = true;
        }

        //text on UI + sounds etc to show that the next level was unlocked

    }

    public void ResetReport()
    {
        PlayerPrefs.SetString("Report", " ");
        report = " ";
        PlayerPrefs.SetInt("MaxLevel", 1);
        PlayerPrefs.SetInt("MaxStars1", 0);
        PlayerPrefs.SetInt("MaxStars2", 0);
        PlayerPrefs.SetInt("MaxStars3", 0);
        PlayerPrefs.SetInt("MaxStars4", 0);
        PlayerPrefs.SetInt("MaxStars5", 0);
    }

}
