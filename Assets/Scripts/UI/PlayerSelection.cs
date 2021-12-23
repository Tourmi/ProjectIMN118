using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSelection : MonoBehaviour
{
    [SerializeField]
    private int playerIndex = 1;
    [SerializeField]
    private CharacterButton defaultSelection;

    private CharacterButton currentSelection { get; set; }

    public bool canMove = true;
    private bool isInCharacterSelectScreen = false;
    private bool isInConfirmationScreen = false;

    public void IsInCharacterSelectScreen(bool isInCharacterSelectScreen)
    {
        this.isInCharacterSelectScreen = isInCharacterSelectScreen;
    }

    public void IsInConfirmationScreen(bool isInConfirmationScreen)
    {
        this.isInConfirmationScreen = isInConfirmationScreen;
    }

    public void Deselect()
    {
        canMove = true;
    }

    private void Start()
    {
        canMove = true;
        currentSelection = defaultSelection;
        defaultSelection.Select(playerIndex);
    }

    private void OnNavigate(InputValue inputValue)
    {
        Vector2 movement = inputValue.Get<Vector2>();
        if (!canMove || !isInCharacterSelectScreen)
            return;

        CharacterButton potentialSelection = null;
        //left
        if (movement.x < -0.9)
        {
            potentialSelection = currentSelection.left;
        }
        //right
        else if (movement.x > 0.9)
        {
            potentialSelection = currentSelection.right;
        }
        //down
        else if (movement.y < -0.9)
        {
            potentialSelection = currentSelection.down;
        }
        //up
        else if (movement.y > 0.9)
        {
            potentialSelection = currentSelection.up;
        }

        if (potentialSelection != null)
        {
            currentSelection.Deselect(playerIndex);
            potentialSelection.Select(playerIndex);
            currentSelection = potentialSelection;
        }
    }

    private void OnSubmit()
    {
        if (!isInCharacterSelectScreen)
            return;
        if (isInConfirmationScreen)
            return;

        canMove = (currentSelection is BackCharacterButton);
        currentSelection.Press(playerIndex);
      
    }

    private void OnCancel()
    {
        if (!isInCharacterSelectScreen)
            return;
        if(isInConfirmationScreen)
            return;
        

        canMove = true;
        // Do something when canceling selection?
    }
}
