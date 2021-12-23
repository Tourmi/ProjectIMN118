using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CharacterButton : MonoBehaviour
{
    [SerializeField]
    public CharacterButton up;
    [SerializeField]
    public CharacterButton down;
    [SerializeField]
    public CharacterButton left;
    [SerializeField]
    public CharacterButton right;

    [SerializeField]
    private GameObject player1Cursor;
    [SerializeField]
    private GameObject player2Cursor;

    protected Button button;
    public void Start()
    {
        button = GetComponent<Button>();
    }

    public void Select(int playerIndex)
    {
        if (playerIndex == 1)
        {
            player1Cursor.SetActive(true);
        }
        else if (playerIndex == 2)
        {
            player2Cursor.SetActive(true);
        }
    }

    public void Deselect(int playerIndex)
    {
        if (playerIndex == 1)
        {
            player1Cursor.SetActive(false);
        } else if (playerIndex == 2)
        {
            player2Cursor.SetActive(false);
        }
    }

    public virtual void Press(int playerIndex)
    {

    }
}
