using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageToggle : MonoBehaviour
{
    SelectStage_InPopup _inPopup;
    private Toggle Toggle;

    private void Awake()
    {
        Toggle = GetComponent<Toggle>();
        _inPopup = GetComponentInParent<SelectStage_InPopup>();
    }

    public void EnterHighLight()
    {
        // Dotween 5-10% ���� ȿ��
        if (Toggle.isOn) return;

        transform.DOScale(1.1f, 0.2f);

    }

    public void ExitHighLight()
    {
        // Dotween

        if (Toggle.isOn) return;
        transform.DOScale(1f, 0.2f);
    }

    public void OnClickHighLight(int StageNumber)
    {
        transform.DOScale(1f, 0.2f);
        _inPopup.OnClickStage(StageNumber);
    }


}
