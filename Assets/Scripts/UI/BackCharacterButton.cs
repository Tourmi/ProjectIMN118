using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCharacterButton : CharacterButton
{
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private CharacterSelectScreen characterSelectScreen;
    public override void Press(int playerIndex)
    {
        characterSelectScreen.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
}
