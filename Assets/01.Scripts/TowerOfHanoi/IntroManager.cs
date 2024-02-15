using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [SerializeField]
    Transform Title;
    [SerializeField]
    Image Button;

    bool oneTime;

    public void Init()
    {
        ButtonFade(1, 0.2f);
        Title.GetComponent<Image>().DOFade(1, 0.2f);
        oneTime = false;
    }

    public void OnClickStart()
    {
        GameManager.Instance.API1(()=>
        {
            GameManager.Instance.API6();
        });
        if (oneTime) return;

        oneTime = true;
        transform.gameObject.SetActive(false);
    }

    IEnumerator StartOnClickStart()
    {
        TitleMove();
        yield return new WaitForSeconds(0.3f);

        yield return new WaitForSeconds(0.5f);
        Title.GetComponent<Image>().DOFade(0, 0.6f);
        ButtonFade(0, 0.6f);
        yield return new WaitForSeconds(0.6f);
        transform.gameObject.SetActive(false);
    }

    private void TitleMove()
    {
        Title.DORotate(new Vector3(0, 0, 10), 0.2f);
        Title.DORotate(new Vector3(0, 0, -5), 0.1f).SetDelay(0.2f);
        Title.DORotate(new Vector3(0, 0, 0), 0.1f).SetDelay(0.3f);
    }
    private void ButtonFade(float a, float duration)
    {
        Button.DOFade(a, duration);
    }

}
