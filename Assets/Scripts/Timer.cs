using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action OnTimerFinished;

    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private int startValue = 99;

    private bool timerStarted = false;
    private float startTime;
    private int currValue;

    private void Start()
    {
        currValue = startValue;
    }

    void Update()
    {
        if (timerStarted)
        {
            float timeElapsed = Time.time - startTime;

            currValue = startValue - (int)timeElapsed;

            if (currValue <= 0)
            {
                currValue = 0;

                OnTimerFinished?.Invoke();
                timerStarted = false;
            }
        }

        timerText.text = currValue.ToString();
    }

    public void StartTimer()
    {
        timerStarted = true;
        startTime = Time.time;
        currValue = startValue;
    }
}
