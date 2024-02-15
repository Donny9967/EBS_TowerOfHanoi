using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    InGameManager ingameManager;

    [SerializeField]
    public int PopupNumber;

    [SerializeField]
    public InputName_InPopup inputName;

    [SerializeField]
    Transform Pivot_Home;

    InPopup[] inPopupChildren;
    [SerializeField]
    List<InPopup> inPopups = new List<InPopup>();

    [SerializeField]
    public Image Dim_Dark;

    public GameObject ExitButtons_Popup;

    [SerializeField]
    private Transform Prev_Pos;

    private AudioSource audi;
    [SerializeField]
    private AudioClip ButtonSound;

    private void Awake()
    {
        audi = GetComponent<AudioSource>();
        inPopupChildren = GetComponentsInChildren<InPopup>();
        // 현재 오브젝트의 모든 자식 오브젝트를 순회
        foreach (InPopup child in inPopupChildren)
        {
            // 자식 오브젝트에 대한 작업 수행
            if (child.GetComponent<InPopup>().onTutorial)
            {
                inPopups.Add(child.GetComponent<InPopup>());
            }
            // 여기에 추가적인 코드를 작성할 수 있습니다.
        }

        Init();
    }

    // 초기화
    public void Init()
    {
        transform.DOLocalMove(Vector3.zero, 0.2f);
        transform.DOScale(1, 0.2f);

        PopupNumber = 0;

        for (int i = 0; i < inPopupChildren.Length; i++)
        {
            inPopupChildren[i].Number = i;
            inPopupChildren[i].gameObject.SetActive(false);
        }

        inPopupChildren[0].gameObject.SetActive(true);
        inPopupChildren[0].transform.DOLocalMove(Vector3.zero, 0.2f);
        inPopupChildren[0].transform.DOScale(1, 0.2f);

        inputName.GetName_Random();
    }

    public void OnClickButtonSound()
    {
        audi.PlayOneShot(ButtonSound);
    }

    // 메뉴눌러서 팝업 활성화 할 때 켜지는 효과
    public void EnterPopup(Transform Pivot)
    {
        audi.PlayOneShot(ButtonSound);
        Prev_Pos = Pivot;
        transform.position = Pivot.transform.position;
        transform.DOLocalMove(Vector3.zero, 0.2f);
        transform.DOScale(1, 0.2f);
    }

    // 메뉴 눌러서 팝업 활성화
    public void EnterPopupActive(GameObject Target)
    {
        DimFadeIn();
        PopupNumber = Target.GetComponent<InPopup>().Number;

        for (int i = 0; i < inPopups.Count; i++)
        {
            inPopups[i].gameObject.SetActive(false);
        }

        if (Target.GetComponent<InPopup>())
        {
            Target.transform.localScale = Vector3.zero;
            Target.SetActive(true);
            InPopup pop = Target.GetComponent<InPopup>();
            pop.OnPopup = true;
            pop.OnPopupActive();
            Target.transform.DOScale(1, 0.2f);
        }

    }

    // 팝업 닫기 버튼
    public void ExitPopup()
    {
        if (GameManager.Instance.OnTutorial)
        {
            // 튜토리얼의 마지막 팝업이라면
            if (PopupNumber >= inPopups.Count - 1)
            {
                //inPopups[PopupNumber].gameObject.SetActive(false);
                //Dim.SetActive(false);

                DimFadeOut();
                // 작아지기
                transform.DOMove(Pivot_Home.transform.position, 0.2f);
                transform.DOScale(0, 0.2f);
            }
            else
            {
                inPopups[PopupNumber].transform.DOScale(0, 0.2f).OnComplete(TutoCompleteFunction);
                inPopups[PopupNumber + 1].gameObject.SetActive(true);
                inPopups[PopupNumber + 1].transform.localScale = Vector3.zero;
                inPopups[PopupNumber + 1].transform.DOScale(1, 0.2f);
                inPopups[PopupNumber + 1].OnPopupActive();
                PopupNumber++;
            }
        }
        // 튜토리얼이 아니라 인게임 상황이라면
        else
        {
            DimFadeOut();
            if (Prev_Pos != null)
            {
                transform.DOMove(Prev_Pos.transform.position, 0.2f);
                transform.DOScale(0, 0.2f).OnComplete(CompleteFunction);
            }
            else
            {
                transform.DOMove(Pivot_Home.transform.position, 0.2f);
                transform.DOScale(0, 0.2f).OnComplete(CompleteFunction);
            }

        ;
        }

    }

    public void TutoCompleteFunction()
    {
        inPopups[PopupNumber - 1].gameObject.SetActive(false);
    }
    public void CompleteFunction()
    {
        inPopupChildren[PopupNumber].gameObject.SetActive(false);
    }

    // 이름 입력
    public void NameConfirm(string PlayerName, int popNumber)
    {
        GameManager.Instance.Username = PlayerName;
        NextPopup(popNumber);
    }

    // 모드 선택
    public void ModeConfirm(bool PracticeMode, int popNumber)
    {

        GameManager.Instance.OnPractice = PracticeMode;

        if (PracticeMode)
        {
            NextPopup(popNumber);
        }
        else
        {
            NextPopup(popNumber + 1);
        }
    }

    // 스테이지 선택
    public void StageConfirm(int StageNumber, int popNumber)
    {
        GameManager.Instance.Stage = StageNumber;
        GameManager.Instance.API4();

        ingameManager.Init();
        ingameManager.ResetPlay(StageNumber);

        NextPopup(popNumber + 1);
    }

    public void NextPopup(int popNumber)
    {
        // 마지막 팝업이라면
        if (popNumber >= inPopups.Count - 1)
        {
            for (int i = 0; i < popNumber; i++)
            {
                inPopups[i].gameObject.SetActive(false);
            }
            // 방금 팝업 끄고
            transform.DOLocalMove(Vector2.zero, 0.2f);
            transform.DOScale(0, 0.2f);

            // 딤처리 끄고
            DimFadeOut();

            // 튜토리얼을 끝낸다
            GameManager.Instance.OnTutorial = false;


            // 마지막 팝업이 끝나고 게임을 시작한다
            ingameManager.ResetPlay(GameManager.Instance.Stage);


        }
        else
        {
            // 마지막 팝업이 아니라 뒤에 더 있다면
            // 현재 팝업을 끄고
            inPopups[popNumber].transform.DOScale(0, 0.2f).OnComplete(() => NextPopupCompleteFunction(popNumber));

            // 다음 팝업을 키고
            inPopups[PopupNumber + 1].transform.localScale = Vector2.zero;
            inPopups[popNumber + 1].gameObject.SetActive(true);
            inPopups[PopupNumber + 1].transform.DOScale(1, 0.2f);
            // 현재의 팝업 번호를 1 증가한다
            PopupNumber++;

        }
    }

    public void NextPopupCompleteFunction(int popNumber)
    {
        inPopups[popNumber].gameObject.SetActive(false);
    }

    public MonoBehaviour targetScript;


    public void DimFadeIn()
    {
        targetScript.enabled = false;
        ingameManager.onGameover = true;
        Dim_Dark.raycastTarget = true;
        Dim_Dark.DOFade(1, 0.2f);
    }

    public void DimFadeOut()
    {
        targetScript.enabled = true;
        ingameManager.onGameover = false;
        Dim_Dark.raycastTarget = false;
        Dim_Dark.DOFade(0, 0.2f);
    }
}
