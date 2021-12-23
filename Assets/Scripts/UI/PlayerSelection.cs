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

    private CharacterButton currentSelection;

    private void OnEnable()
    {
        currentSelection = defaultSelection;
        defaultSelection.Select(playerIndex);
    }

    private void OnDisable()
    {
        currentSelection.Deselect(playerIndex);
    }

    private void OnNavigate(InputValue inputValue)
    {
        Vector2 movement = inputValue.Get<Vector2>();

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
        currentSelection.Press(playerIndex);
    }

    private void OnCancel()
    {
        // Do something when canceling selection?
    }
}
