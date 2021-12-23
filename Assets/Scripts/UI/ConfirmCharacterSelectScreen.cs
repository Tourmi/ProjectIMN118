using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmCharacterSelectScreen : MonoBehaviour
{
    [SerializeField]
    private Selectable defaultSelection;
    [SerializeField]
    private PlayerSelection player1Selection;
    [SerializeField]
    private PlayerSelection player2Selection;

    private void OnEnable()
    {
        if (defaultSelection != null)
        {
            defaultSelection.Select();
        }
    }

    private void OnDisable()
    {
        player1Selection.IsInConfirmationScreen(false);
        player2Selection.IsInConfirmationScreen(false);
    }

}
