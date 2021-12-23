using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private TMP_Text overlayText;
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private LifeBar lifebarLeft;
    [SerializeField]
    private LifeBar lifebarRight;
    [SerializeField]
    private RoundCounter roundCounter;

    private bool fightStarted = false;
    private bool fightEnded = false;

    private void Start()
    {
        timer.OnTimerFinished += HandleTimeout;

        StartCountdown();
    }

    private void HandleTimeout()
    {
        fightEnded = true;

        // time out, check who won
    }

    private void UpdateOverlayTextTransparency(ITween<float> tween)
    {
        overlayText.alpha = tween.CurrentValue;
    }

    private void StartCountdown()
    {
        overlayText.gameObject.SetActive(true);
        overlayText.text = "Boss Fight begins in";
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

        fightStarted = true;
        timer.StartTimer();
    }
}
