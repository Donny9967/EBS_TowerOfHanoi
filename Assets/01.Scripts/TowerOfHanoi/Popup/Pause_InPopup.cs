using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_InPopup : InPopup
{
    [SerializeField]
    Transform Menu_Pause;

    [SerializeField]
    private bool OnPause;


    public void OnClick()
    {
        OnPause = false;
        GetComponentInParent<PopupManager>().ExitPopup();
    }

    public override void OnPopupActive()
    {
        OnPause = true;
    }
}
