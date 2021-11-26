using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public List<ProtoCard> cardsList;

    //names and the relative categories are added in the editor
    public string[] names;
    public Categoria[] categories;

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

    void Start()
    {
        //let's find the Canvas in the scene
        canvas = GameObject.Find("Canvas");
        enemySpawner = GameObject.Find("Enemy Spawner").transform;

        SpawnEnemy();

        //let's create all of the cards, and let's add them to the List cardsList
        for (int i = 0; i < names.Length; i++)
        {
            string curName = names[i];
            //I get the initial as the first letter of the name
            string curInitial = curName[0].ToString();
            //I create a card and I add it to the list
            cardsList.Add(new ProtoCard());
            cardsList.Last().Initialize_ProtoCard(curName, curInitial, categories[i]);
        }
        //Let's spawn the first card of the level!
        SpawnCard(curCardNumber);
    }

    public void SpawnEnemy()
    {
        if (curEnemy)
        {
            Destroy(curEnemy.gameObject);
        }
        curEnemy = Instantiate(enemyPrefab, enemySpawner.position, this.transform.rotation);
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
    }

    //this function is called when I click one of the two buttons on screen. They will give 'soft' or 'hard' as input strings
    public void CheckResult(int choice)
    {
        //here I check if the input (choice) is the same as the category of the current card.
        //If it is, I increase the score by one. Otherwise, nothing happens.
        Categoria cat_choice;
        if (choice == 0)
            cat_choice = Categoria.Hard;
        else
            cat_choice = Categoria.Soft;

        if (cat_choice == cardsList[curCardNumber].GetCategoria)
        {
            Debug.Log("Correct!");
            score++;
            curEnemy.GetComponent<ObjectMovement>().GetCaptured();
        }
        else
        {
            Debug.Log("Error");
            curEnemy.GetComponent<ObjectMovement>().Charge();
        }
        //I destroy the current card
        //Destroy(curCard);
        curEnemy = null;
        //I increase the number of the current card, so I will spawn the next card in the deck
        curCardNumber++;
        //If the number is not higher than the number of the cards available for the level, I proceed to create the next card
        if (curCardNumber < cardsList.Count)
        {
            SpawnEnemy();
            SpawnCard(curCardNumber);
        }
    }

}
