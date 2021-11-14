using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//let's create a struct that keeps all the variables of every card
[System.Serializable]
public struct Card
{
    public string name;
    public string initial;
    public string category;
    //Image picture; we will add pictures later

    //this is the initialization function; its name is the same of the struct's one
    public Card(string n, string i, string c)
    {
        this.name = n;
        this.initial = i;
        this.category = c;
    }
}

public class LevelManager : MonoBehaviour
{
    public List<Card> cardsList;

    //names and the relative categories are added in the editor
    public string[] names;
    public string[] categories;

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

        //let's create all of the cards, and let's add them to the List cardsList
        for (int i = 0; i < names.Length; i++)
        {
            string curName = names[i];
            //I get the initial as the first letter of the name
            string curInitial = curName[0].ToString();
            //I create a card and I add it to the list
            cardsList.Add(new Card(curName, curInitial, categories[i]));
        }
        //Let's spawn the first card of the level!
        SpawnCard(curCardNumber);
    }

    public void SpawnCard(int n)
    {
        //I instantiate the general card prefab, and I assign it to the curCard variable. Its position is centered on the canvas' position, and its rotation is null
        curCard = Instantiate(cardPrefab, canvas.transform.position, this.transform.rotation);
        //I have to put the card as child of the Canvas, otherwise it will not be visible on screen (ONLY WORKS FOR UI ELEMENTS, like in this case)
        curCard.transform.parent = canvas.transform;
        //now I change the various texts on the instantiated card to match the one of the current card that has been drawn:
        curCard.transform.Find("Maiusc word").GetComponent<Text>().text = cardsList[n].name.ToUpper();
        curCard.transform.Find("Little word").GetComponent<Text>().text = cardsList[n].name.ToLower();
        curCard.transform.Find("Maiusc letter").GetComponent<Text>().text = cardsList[n].initial.ToUpper();
        curCard.transform.Find("Little letter").GetComponent<Text>().text = cardsList[n].initial.ToLower();
    }

    //this function is called when I click one of the two buttons on screen. They will give 'soft' or 'hard' as input strings
    public void CheckResult(string choice)
    {
        //here I check if the input (choice) is the same as the category of the current card.
        //If it is, I increase the score by one. Otherwise, nothing happens.
        if (choice == cardsList[curCardNumber].category)
        {
            Debug.Log("Correct!");
            score++;
        }
        else
        {
            Debug.Log("Error");
        }
        //I destroy the current card
        Destroy(curCard);
        //I increase the number of the current card, so I will spawn the next card in the deck
        curCardNumber++;
        //If the number is not higher than the number of the cards available for the level, I proceed to create the next card
        if (curCardNumber < cardsList.Count)
        {
            SpawnCard(curCardNumber);
        }
    }

}
