using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFightScene : MonoBehaviour
{
    public void LoadFight(){
        SceneManager.LoadScene("Scenes/Fight");
    }
}
