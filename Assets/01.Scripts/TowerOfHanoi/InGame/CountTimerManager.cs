using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountTimerManager : MonoBehaviour
{
    public TextMeshProUGUI timeText; // UI Text 컴포넌트를 할당하기 위한 변수
    public TextMeshProUGUI countText; // UI Text 컴포넌트를 할당하기 위한 변수
    private bool isTiming = false; // 시간 측정 여부를 판단하는 변수
    [SerializeField]
    private float t;
    [SerializeField]
    private int moveCount;
    void Start()
    {
        t = 0;
        moveCount = 0;
    }

    public void Init()
    {
        isTiming = false;
        t = 0;
        moveCount = 0;
        countText.text = moveCount.ToString();
        timeText.text = "00:00:00";

        OnClearTime();
        OnClearCount();
    }

    public void CountUp()
    {
        moveCount++;
        countText.text = moveCount.ToString();
    }

    public void StartTime()
    {
        if (GameManager.Instance.OnTutorial) return;

        isTiming = true;
    }

    // 시간 측정을 중지할 때 호출
    public void StopTime()
    {
        isTiming = false;
    }

    public void ResetTime()
    {
        t = 0;
    }

    public void ResetStartTime()
    {
        t = 0;
        isTiming = true;
    }


    void Update()
    {
        if (isTiming)
        {
            t = t + Time.deltaTime; // 경과 시간을 계산

            int totalMilliseconds = (int)Math.Round(t * 1000);

            // 시간, 분, 초, 밀리초 계산
            int minutes = totalMilliseconds / 60000;
            int secs = (totalMilliseconds % 60000) / 1000;
            int milliseconds = totalMilliseconds % 1000;

            // "MM:SS.fff" 형식으로 포맷 (분:초.밀리초)
            timeText.text = $"{minutes:D2}:{secs:D2}.{milliseconds:D3}";

            //string minutes = ((int)(t % 3600) / 60).ToString("00");
            //string seconds = (t % 60).ToString("00");
            //string milliseconds = ((int)((t % 1) * 1000)).ToString("000");

            //timeText.text = minutes + ":" + seconds + ":" + milliseconds; // UI Text 업데이트
        }
    }
    public void OnClearTime()
    {
        StopTime();
        int totalMilliseconds = (int)Math.Round(t * 1000);
        GameManager.Instance.Time = totalMilliseconds;

        //ResetTime();
    }

    public string tt;

    public void OnClearCount()
    {
        GameManager.Instance.Score = moveCount;
    }


}
