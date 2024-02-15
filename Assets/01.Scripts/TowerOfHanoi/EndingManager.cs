using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [SerializeField]
    Transform GameClear;
    [SerializeField]
    RankingManager rankingManager;
    [SerializeField]
    PopupManager popupManager;

    [Space(10)]
    [SerializeField]
    TextMeshProUGUI record_Title;
    [SerializeField]
    TextMeshProUGUI record_Name;
    [SerializeField]
    TextMeshProUGUI record_Count;
    [SerializeField]
    TextMeshProUGUI record_Time;

    [Space(10)]
    [SerializeField]
    GameObject MainDim;
    [SerializeField]
    Transform RankingDim;

    private void Awake()
    {
        rankingManager.gameObject.SetActive(true);
        rankingManager.transform.localScale = Vector3.zero;
    }

    public void Init()
    {
        record_Name.text = "";
        record_Count.text = "";
        record_Time.text = "";
    }

    void Active()
    {
        transform.localScale = Vector3.zero;
        transform.DOLocalMove(Vector3.zero, 0.2f);
        transform.DOScale(1, 0.2f);
    }

    public void ClearGame()
    {
        GameClear.gameObject.SetActive(true);
        SetCount(GameManager.Instance.Score);
        SetTime(GameManager.Instance.Time);
        SetName(GameManager.Instance.Username);
        SetTite(GameManager.Instance.Stage);

        Active();
    }

    public void SetCount(int recordCount)
    {
        record_Count.text = recordCount.ToString();
    }

    public void SetTime(float recordTime)
    {
        // 시간, 분, 초, 밀리초 계산
        int minutes = (int)(recordTime / 60000);
        int seconds = (int)((recordTime % 60000) / 1000);
        int milliseconds = (int)(recordTime % 1000);

        record_Time.text = $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
    }

    public void SetName(string recordName)
    {
        record_Name.text = recordName + " 님의 기록은";
    }

    public void SetTite(int stageNumber)
    {
        record_Title.text = "원판 " + stageNumber.ToString() + "개를 옮기는데 성공했습니다.";
    }

    public void OnClickAgain()
    {
        transform.DOLocalMove(Vector3.zero, 0.2f);
        transform.DOScale(0, 0.2f);
    }

    public void OnClickMain()
    {
        if (!GameManager.Instance.OnPractice)
            GameManager.Instance.API4();

        transform.DOLocalMove(Vector3.zero, 0.2f);
        transform.DOScale(0, 0.2f);
        GameManager.Instance.Init();
    }

    public void OnClickRanking()
    {
        GameManager.Instance.API5(RankingAppear);
    }

    public void RankingAppear()
    {
        rankingManager.Init();
        popupManager.DimFadeIn();
        RankingDim.gameObject.SetActive(true);
        rankingManager.transform.DOLocalMove(Vector3.zero, 0.2f);
        rankingManager.transform.DOScale(1, 0.2f);
    }

    public void OnClickExitRanking()
    {
        RankingDim.gameObject.SetActive(false);
        rankingManager.transform.DOLocalMove(Vector3.zero, 0.2f);
        rankingManager.transform.DOScale(0, 0.2f).OnComplete(AfterExitRanking);
    }

    public void AfterExitRanking()
    {
        for (int i = 0; i < rankingManager.rankings.Count; i++)
        {
            rankingManager.rankings[i].name.text = null;
            rankingManager.rankings[i].time.text = null;
        }
    }

}
