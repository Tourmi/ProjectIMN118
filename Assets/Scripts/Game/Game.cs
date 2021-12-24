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
    private LifeBar leftHealth;
    [SerializeField]
    private LifeBar rightHealth;
    [SerializeField]
    private GameCamera gameCamera;
    [SerializeField]
    private RoundCounter roundCounter;
    [SerializeField]
    private Vector3 initialPlayer1Position;
    [SerializeField]
    private Vector3 initialPlayer2Position;

    [SerializeField]
    private Transform gameplay;

    private bool fightEnded = false;
    private Fighter fighter1Instance;
    private Fighter fighter2Instance;

    private void Start()
    {
        timer.OnTimerFinished += FinishFight;
        roundCounter.Initialize();
        fightEnded = false;

        if (CharacterSelectScreen.player1Fighter != null)
        {
            fighter1 = CharacterSelectScreen.player1Fighter;
        }
        if (CharacterSelectScreen.player2Fighter != null)
        {
            fighter2 = CharacterSelectScreen.player2Fighter;
        }
        fighter1Instance = Instantiate(fighter1, gameplay);
        fighter2Instance = Instantiate(fighter2, gameplay);

        leftHealth.fighter = fighter1Instance;
        rightHealth.fighter = fighter2Instance;
        gameCamera.character1 = fighter1Instance.transform;
        gameCamera.character2 = fighter2Instance.transform;
        fighter1Instance.Opponent = fighter2Instance.transform;
        fighter2Instance.Opponent = fighter1Instance.transform;

        StartCountdown();
    }

    private void Update()
    {
        if (fightEnded)
        {
            return;
        }
        if (fighter1Instance.CurrentHealth <= 0 || fighter2Instance.CurrentHealth <= 0)
        {
            FinishFight();
        }
        if (fighter1Instance.EnemyDirection < 0 && fighter1Instance.transform.localScale.x > 0)
        {
            fighter1Instance.transform.localScale = new Vector3(-fighter1Instance.transform.localScale.x, fighter1Instance.transform.localScale.y, fighter1Instance.transform.localScale.z);
        }
        else if (fighter1Instance.EnemyDirection > 0 && fighter1Instance.transform.localScale.x < 0)
        {
            fighter1Instance.transform.localScale = new Vector3(-fighter1Instance.transform.localScale.x, fighter1Instance.transform.localScale.y, fighter1Instance.transform.localScale.z);
        }

        if (fighter2Instance.EnemyDirection < 0 && fighter2Instance.transform.localScale.x > 0)
        {
            fighter2Instance.transform.localScale = new Vector3(-fighter2Instance.transform.localScale.x, fighter2Instance.transform.localScale.y, fighter2Instance.transform.localScale.z);
        }
        else if (fighter2Instance.EnemyDirection > 0 && fighter2Instance.transform.localScale.x < 0)
        {
            fighter2Instance.transform.localScale = new Vector3(-fighter2Instance.transform.localScale.x, fighter2Instance.transform.localScale.y, fighter2Instance.transform.localScale.z);
        }
    }

    private void FinishFight()
    {
        fightEnded = true;
        fighter1Instance.FightFinished = true;
        fighter2Instance.FightFinished = true;

        bool fighter1Dead = fighter1Instance.CurrentHealth <= 0;
        bool fighter2Dead = fighter2Instance.CurrentHealth <= 0;
        float fighter1HealthRatio = fighter1Instance.CurrentHealth / fighter1Instance.MaxHealth;
        float fighter2HealthRatio = fighter2Instance.CurrentHealth / fighter2Instance.MaxHealth;

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
        fighter1Instance.Initialize(initialPlayer1Position);
        fighter2Instance.Initialize(initialPlayer2Position);
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
        fighter1Instance.FightStarted = true;
        fighter2Instance.FightStarted = true;
    }
}
