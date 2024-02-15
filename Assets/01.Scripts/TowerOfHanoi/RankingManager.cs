using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public List<Ranking> rankings = new List<Ranking>();

    [SerializeField]
    TextMeshProUGUI rankingStage;

    private void Awake()
    {
        GetComponentsInChildren(rankings);
        SetNumber();
    }

    void SetNumber()
    {
        for (int i = 0; i < rankings.Count; i++)
        {
            rankings[i].number.text = (i + 1).ToString();
        }
    }

    public void Init()
    {
        rankingStage.text = "¿øÆÇ " + GameManager.Instance.Stage.ToString() + "°³";
        
    }
}
