using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundCounter : MonoBehaviour
{
    [SerializeField]
    private GameObject LeftCircle1;
    [SerializeField]
    private GameObject LeftCircle2;
    [SerializeField]
    private GameObject RightCircle1;
    [SerializeField]
    private GameObject RightCircle2;

    [SerializeField]
    private TMP_Text RoundNumber;

    private int roundNumber = 1;
    private int player1WonCount = 0;
    private int player2WonCount = 0;

    public void Initialize()
    {
        roundNumber = 1;
        player1WonCount = 0;
        player2WonCount = 0;

        UpdateUI();
    }

    private void UpdateUI()
    {
        RoundNumber.text = roundNumber.ToString();
        LeftCircle1.SetActive(player1WonCount > 0);
        LeftCircle2.SetActive(player1WonCount > 1);
        RightCircle1.SetActive(player2WonCount > 0);
        RightCircle2.SetActive(player2WonCount > 1);
    }

    public void Player1Won()
    {
        player1WonCount++;

        UpdateUI();
    }

    public void Player2Won()
    {
        player2WonCount++;

        UpdateUI();
    }

    public void NextRound()
    {
        roundNumber++;

        UpdateUI();
    }
}
