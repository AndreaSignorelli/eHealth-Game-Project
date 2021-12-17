using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    Vector3 mousePos;
    Vector3 worldPos;
    public GameObject clickEffect;
    public float xDisplacement = 0.5f;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            //spawn click effect on mouse position
            mousePos = Input.mousePosition;
            mousePos.z = 1;
            worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.x += xDisplacement;
            Instantiate(clickEffect, worldPos, Quaternion.identity);
        }
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}