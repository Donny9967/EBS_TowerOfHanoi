﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_InPopup : InPopup
{
    [HideInInspector]
    public Popup_Contents popup_Contents;

    public override void Awake()
    {
        base.Awake();
        popup_Contents = GetComponentInChildren<Popup_Contents>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        //popup_Contents.ResetContent();
        popup_Contents.ResetActive();
    }

    public override void OnClickExit()
    {
        base.OnClickExit();
        popupManager.ExitPopup();
    }

}
