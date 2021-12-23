using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectScreen : MonoBehaviour
{
    [SerializeField]
    private PlayerSelection player1Selection;
    [SerializeField]
    private PlayerSelection player2Selection;
    [SerializeField]
    private CharacterSelectScreen selectScreen;
    [SerializeField]
    private GameObject confirmCharacterSelection;

    public void LoadFightScene()
    {

        SceneManager.LoadScene("Scenes/Fight");
    }

    public void ShowConfirmation()
    {
        if (!player1Selection.canMove && !player2Selection.canMove)
        {
            confirmCharacterSelection.SetActive(true);
            player1Selection.IsInConfirmationScreen(true);
            player2Selection.IsInConfirmationScreen(true);

        }
    }
}


