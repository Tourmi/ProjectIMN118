using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCharacterButton : CharacterButton
{
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private CharacterSelectScreen characterSelectScreen;
    [SerializeField]
    private PlayerSelection player1Selection;
    [SerializeField]
    private PlayerSelection player2Selection;

    public override void Press(int playerIndex)
    {
        characterSelectScreen.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
        player1Selection.IsInCharacterSelectScreen(false);
        player2Selection.IsInCharacterSelectScreen(false);
    }


}
