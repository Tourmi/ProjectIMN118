using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Selectable defaultSelection;

    private void OnEnable()
    {
        if (defaultSelection != null)
        {
            defaultSelection.Select();
        }
    }

    public void LoadFight(){
        SceneManager.LoadScene("Scenes/Fight");
    }
}
