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
    public Fighter character;

    [SerializeField]
    private GameObject player1Cursor;
    [SerializeField]
    private GameObject player2Cursor;

    [SerializeField]
    private Image player1Preview;
    [SerializeField]
    private Image player2Preview;
    [SerializeField]
    private string previewImage;

    [SerializeField]
    private AudioSource selectedButtonAudio;
    [SerializeField]
    private AudioSource pressedButtonAudio;

    [SerializeField]
    private CharacterSelectScreen selectScreen;

    protected Button button;
    public void Start()
    {
        button = GetComponent<Button>();
    }

    public void Select(int playerIndex)
    {
        if (playerIndex == 1)
        {
            if (player1Preview != null)
            {
                Sprite previewSprite = Resources.Load<Sprite>($"Textures/{previewImage}");
                player1Preview.sprite = previewSprite;
            }

            player1Cursor.SetActive(true);
        }
        else if (playerIndex == 2)
        {
            if (player2Preview != null)
            {
                Sprite previewSprite = Resources.Load<Sprite>($"Textures/{previewImage}");
                player2Preview.sprite = previewSprite;
            }
            player2Cursor.SetActive(true);
        }
        selectedButtonAudio.Play();
    }

    public void Deselect(int playerIndex)
    {
        if (playerIndex == 1)
        {
            player1Cursor.SetActive(false);
        }
        else if (playerIndex == 2)
        {
            player2Cursor.SetActive(false);
        }
    }

    public virtual void Press(int playerIndex)
    {
        pressedButtonAudio.Play();

        selectScreen.ShowConfirmation();
    }
}
