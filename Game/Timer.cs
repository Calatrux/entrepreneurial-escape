using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public float currentTime;
    private float startTime;
    private bool isRunning = false;

    public void Start() {
        //startTime = Time.time;
        isRunning = true;
        startTime = Time.time;
    }

    private void Update() {
        if (isRunning) {
            currentTime = Time.time - startTime + 500;

            string minutes = ((int) currentTime / 60).ToString("00");
            string seconds = ((int) currentTime % 60).ToString("00");

            timerText.text = minutes + ":" + seconds;
        }
    }
}