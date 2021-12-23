using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField]
    private TMP_Text overlayText;
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private Fighter fighter1;
    [SerializeField]
    private Fighter fighter2;
    [SerializeField]
    private RoundCounter roundCounter;
    [SerializeField]
    private Vector3 initialPlayer1Position;
    [SerializeField]
    private Vector3 initialPlayer2Position;

    private bool fightEnded = false;

    private void Start()
    {
        timer.OnTimerFinished += FinishFight;
        roundCounter.Initialize();
        fightEnded = false;

        StartCountdown();
    }

    private void Update()
    {
        if (fightEnded)
        {
            return;
        }
        if (fighter1.CurrentHealth <= 0 || fighter2.CurrentHealth <= 0)
        {
            FinishFight();
        }
    }

    private void FinishFight()
    {
        fightEnded = true;
        fighter1.FightFinished = true;
        fighter2.FightFinished = true;

        bool fighter1Dead = fighter1.CurrentHealth <= 0;
        bool fighter2Dead = fighter2.CurrentHealth <= 0;
        float fighter1HealthRatio = fighter1.CurrentHealth / fighter1.MaxHealth;
        float fighter2HealthRatio = fighter2.CurrentHealth / fighter2.MaxHealth;

        if (fighter1Dead && fighter2Dead)
        {
            Tie();
            return;
        }

        if (fighter1Dead)
        {
            Player2Win();
            return;
        }

        if (fighter2Dead)
        {
            Player1Win();
            return;
        }

        if (fighter1HealthRatio > fighter2HealthRatio)
        {
            Player1Win();
            return;
        }
        if (fighter2HealthRatio > fighter1HealthRatio)
        {
            Player2Win();
            return;
        }

        Tie();
    }

    private void Tie()
    {
        overlayText.gameObject.SetActive(true);
        overlayText.text = "Match nul";
        TweenFactory.Tween("overlaytextAlpha", 3f, -1f, 4, null, UpdateOverlayTextTransparency, StartCountdown);
    }

    private void Player1Win()
    {
        roundCounter.Player1Won();

        overlayText.gameObject.SetActive(true);
        if (roundCounter.MatchEnded())
        {
            overlayText.text = "Joueur 1 est le vainceur";
            TweenFactory.Tween("overlaytextAlpha", 3f, -1f, 4, null, UpdateOverlayTextTransparency, ReturnToMenu);
        }
        else
        {
            overlayText.text = "Joueur 1 Gagne";
            TweenFactory.Tween("overlaytextAlpha", 3f, -1f, 4, null, UpdateOverlayTextTransparency, StartCountdown);
        }
    }

    private void Player2Win()
    {
        roundCounter.Player2Won();

        overlayText.gameObject.SetActive(true);
        if (roundCounter.MatchEnded())
        {
            overlayText.text = "Joueur 2 est le vainceur";
            TweenFactory.Tween("overlaytextAlpha", 3f, -1f, 4, null, UpdateOverlayTextTransparency, ReturnToMenu);
        }
        else
        {
            overlayText.text = "Joueur 2 Gagne";
            TweenFactory.Tween("overlaytextAlpha", 3f, -1f, 4, null, UpdateOverlayTextTransparency, StartCountdown);
        }
    }

    private void UpdateOverlayTextTransparency(ITween<float> tween)
    {
        overlayText.alpha = tween.CurrentValue;
    }

    private void ReturnToMenu(ITween<float> oldTween = null)
    {
        SceneManager.LoadScene("Scenes/Menu");
    }

    private void StartCountdown(ITween<float> oldTween = null)
    {
        fightEnded = false;
        timer.ResetTimer();
        fighter1.Initialize(initialPlayer1Position);
        fighter2.Initialize(initialPlayer2Position);
        roundCounter.NextRound();

        overlayText.gameObject.SetActive(true);
        overlayText.text = "Le Boss Fight commence dans";
        TweenFactory.Tween("overlaytextAlpha", 2f, -0.25f, 3f, null, UpdateOverlayTextTransparency, ContinueCountdown3);
    }

    private void ContinueCountdown3(ITween<float> oldTween)
    {
        overlayText.text = "3";
        TweenFactory.Tween("overlaytextAlpha", 2f, -0.25f, 1.25f, null, UpdateOverlayTextTransparency, ContinueCountdown2);
    }

    private void ContinueCountdown2(ITween<float> oldTween)
    {
        overlayText.text = "2";
        TweenFactory.Tween("overlaytextAlpha", 2f, -0.25f, 1.25f, null, UpdateOverlayTextTransparency, ContinueCountdown1);
    }

    private void ContinueCountdown1(ITween<float> oldTween)
    {
        overlayText.text = "1";
        TweenFactory.Tween("overlaytextAlpha", 2f, -0.25f, 1.25f, null, UpdateOverlayTextTransparency, StartFight);
    }

    private void StartFight(ITween<float> oldTween)
    {
        overlayText.text = "Fight!";
        TweenFactory.Tween("overlaytextAlpha", 2f, -0.25f, 1.25f, null, UpdateOverlayTextTransparency, null);

        timer.StartTimer();
        fighter1.FightStarted = true;
        fighter2.FightStarted = true;
    }
}
