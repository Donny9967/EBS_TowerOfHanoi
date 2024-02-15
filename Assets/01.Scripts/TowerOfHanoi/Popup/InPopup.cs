using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InPopup : MonoBehaviour
{
    public bool OnPopup;

    [SerializeField]
    public bool onTutorial;

    [SerializeField]
    public int Number;

    protected PopupManager popupManager;

    public virtual void Awake()
    {
        popupManager = GetComponentInParent<PopupManager>();
    }

    public virtual void OnEnable()
    {

    }



    public virtual void OnPopupActive()
    {

    }

    public virtual void OnClickExit()
    {

    }
}
