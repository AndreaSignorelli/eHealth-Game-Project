using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string[] allLines;
    public int[] allInterlocutori; //0=player, 1=friend, 2=adult, 3=friend's family, 4=boss
}
