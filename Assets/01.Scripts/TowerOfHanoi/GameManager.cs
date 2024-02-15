using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class Api1Response
{
    public string CODE;
    public ResultData1 RESULT;
}

[System.Serializable]
public class ResultData1
{
    public int pkgOrgStdyTracSno;
    public int attendDtlId;
    public int rscSno;
}

[System.Serializable]
public class Api2Response
{
    public string CODE;
    public string RESULT;
}


[System.Serializable]
public class Api5Response
{
    public string CODE;
    public List<RankingData> LIST;
}

[System.Serializable]
public class RankingData
{
    public string GAME_TYPE;
    public string BEST_RECORD;
    public string STAGE_STEP;
    public int USER_RANK;
    public int RSC_SNO;
    public string USER_NAME;
    public int TRAC_SNO;
    public string GAME_NAME;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;



    [SerializeField]
    public bool OnTutorial;
    [SerializeField]
    public bool onPractice;
    [SerializeField]
    private string userName;
    [SerializeField]
    private int stage;
    [SerializeField]
    private int level;
    [SerializeField]
    private int score;
    [SerializeField]
    private float time;


    [Space(30)]
    [SerializeField]
    IntroManager Intro;

    [SerializeField]
    Transform Menu;
    [SerializeField]
    PopupManager popupManager;
    [SerializeField]
    InGameManager inGameManager;
    [SerializeField]
    EndingManager endingManager;
    [SerializeField]
    RankingManager rankingManager;
    [SerializeField]
    Toggle toggle;
    [SerializeField]
    SelectStage_InPopup popupStage;

    public bool OnPractice
    {
        get { return onPractice; }
        set { onPractice = value; }
    }

    public string Username
    {
        get { return userName; }
        set { userName = value; }
    }

    public int Stage
    {
        get { return stage; }
        set { stage = value; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public float Time
    {
        get { return time; }
        set { time = value; }
    }



    [Space(30), Header("API")]
    [SerializeField]
    WebManager webManager;

    string _baseUrl = "https://www.ebsmath.co.kr";

    [Space(10)]
    [Header("Input")]
    [SerializeField]
    bool isTest;
    [SerializeField]
    int _rscSno;
    [SerializeField]
    string _historyYn = "study";
    [SerializeField]
    string _evtSsnCd = "";
    [SerializeField]
    string _gameType = "C";
    [SerializeField]
    string _divType = "PC";
    [SerializeField]
    long _startDate;
    [SerializeField]
    long _endDate;

    [Space(10)]
    [Header("Get")]
    [SerializeField]
    string _PTS_PRS_PRD = ""; // Base64.encode(게임시작시 pkgOrgStdyTracSno값)


    [Space(10)]
    [Header("GetResult")]
    [SerializeField]
    int _pkgOrgStdyTracSno;
    [SerializeField]
    int _attendDtlId;


    private void Awake()
    {
        Instance = this;

        if (isTest)
        {
            _rscSno = 29447;
            _gameType = "C";
        }
        else
        {
            _rscSno = 29825;
            _gameType = "B";
        }

        if (!Menu.gameObject.activeSelf)
        {
            Menu.gameObject.SetActive(true);
        }

        if (!popupManager.gameObject.activeSelf)
        {
            popupManager.gameObject.SetActive(true);
        }
    }

    void Start()
    {
        Init();
    }


    public void Init()
    {
        OnTutorial = true;
        userName = null;
        stage = 0;
        popupStage.Init();
        popupStage.stageNumber = 1;

        Intro.gameObject.SetActive(true);
        Intro.Init();

        Menu.gameObject.SetActive(true);

        popupManager.Init();
        inGameManager.Init();
        endingManager.Init();
        endingManager.transform.localScale = Vector3.zero;
    }

    #region API

    public void API1(Action callback = null)
    {
        webManager.Get_1StudyHistory("GET", _baseUrl, _rscSno, _historyYn, _evtSsnCd, (uwr) =>
        {
            Api1Response apiResponse = JsonUtility.FromJson<Api1Response>(uwr.downloadHandler.text);
            if (apiResponse.CODE == "SUCCESS")
            {
                _pkgOrgStdyTracSno = apiResponse.RESULT.pkgOrgStdyTracSno;
                _attendDtlId = apiResponse.RESULT.attendDtlId;
            }
            callback();
        });
    }


    public void API2(Action callback = null)
    {
        webManager.Get_2RankingCrypt("GET", _baseUrl, _pkgOrgStdyTracSno.ToString(), _attendDtlId.ToString(), Time.ToString(), (uwr) =>
        {
            Api2Response apiResponse = JsonUtility.FromJson<Api2Response>(uwr.downloadHandler.text);
            if (apiResponse.CODE == "SUCCESS")
            {
                _PTS_PRS_PRD = apiResponse.RESULT;
            }
            callback();
        });
    }

    public void API3(Action callback = null)
    {
        SetEndDate();
        webManager.Get_3GameHistory_Save("GET", _baseUrl, _pkgOrgStdyTracSno, _rscSno, _gameType, Stage, _PTS_PRS_PRD, _divType, _startDate, _endDate, _evtSsnCd, (uwr) =>
        {
            callback();
            print(Stage);
        });
    }

    public void API4()
    {
        webManager.Get_4GameHistory_Update("GET", _baseUrl, _pkgOrgStdyTracSno, _attendDtlId, _rscSno, _historyYn, (uwr) =>
        {
        });
    }

    public void API5(Action callback)
    {
        webManager.Get_5RankingList("GET", _baseUrl, _rscSno, _gameType, stage, (uwr) =>
        {
            Api5Response apiResponse = JsonUtility.FromJson<Api5Response>(uwr.downloadHandler.text);
            if (apiResponse.CODE == "SUCCESS")
            {
                foreach (var gameRecord in apiResponse.LIST)
                {
                    rankingManager.rankings[gameRecord.USER_RANK - 1].name.text = gameRecord.USER_NAME;

                    int t = int.Parse(gameRecord.BEST_RECORD);

                    string minutes = ((int)(t % 3600) / 60).ToString("00");
                    string seconds = (t % 60).ToString("00");
                    string milliseconds = ((int)((t % 1) * 100)).ToString("00");

                    rankingManager.rankings[gameRecord.USER_RANK - 1].time.text = minutes + ":" + seconds + ":" + milliseconds; // UI Text 업데이트
                }
            }
            callback();
        });
    }

    public void API6()
    {
        webManager.Get_6RankingList("GET", _baseUrl, _pkgOrgStdyTracSno, (uwr) =>
        {

        });
    }

    #endregion

    public void SetStartDate()
    {
        _startDate = GetCurrentTimeAsLong();
    }

    public void SetEndDate()
    {
        _endDate = GetCurrentTimeAsLong();
    }

    public static long GetCurrentTimeAsLong()
    {
        // 현재 시간을 가져옵니다.
        DateTime now = DateTime.Now;

        // 현재 시간을 'yyyymmddHHmmss' 형식의 문자열로 변환합니다.
        string formattedTime = now.ToString("yyyyMMddHHmmss");

        // 문자열을 long 타입으로 파싱합니다.
        long timeAsLong = long.Parse(formattedTime);

        return timeAsLong;
    }

    private void OnApplicationQuit()
    {
        API4();
    }





}
