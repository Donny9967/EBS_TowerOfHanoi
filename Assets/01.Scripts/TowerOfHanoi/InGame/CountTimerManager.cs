using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountTimerManager : MonoBehaviour
{
    public TextMeshProUGUI timeText; // UI Text ������Ʈ�� �Ҵ��ϱ� ���� ����
    public TextMeshProUGUI countText; // UI Text ������Ʈ�� �Ҵ��ϱ� ���� ����
    private bool isTiming = false; // �ð� ���� ���θ� �Ǵ��ϴ� ����
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

    // �ð� ������ ������ �� ȣ��
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
            t = t + Time.deltaTime; // ��� �ð��� ���

            int totalMilliseconds = (int)Math.Round(t * 1000);

            // �ð�, ��, ��, �и��� ���
            int minutes = totalMilliseconds / 60000;
            int secs = (totalMilliseconds % 60000) / 1000;
            int milliseconds = totalMilliseconds % 1000;

            // "MM:SS.fff" �������� ���� (��:��.�и���)
            timeText.text = $"{minutes:D2}:{secs:D2}.{milliseconds:D3}";

            //string minutes = ((int)(t % 3600) / 60).ToString("00");
            //string seconds = (t % 60).ToString("00");
            //string milliseconds = ((int)((t % 1) * 1000)).ToString("000");

            //timeText.text = minutes + ":" + seconds + ":" + milliseconds; // UI Text ������Ʈ
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
