using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    //[SerializeField] private Player player;
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private TextMeshPro grenadeTxt, ammoTxt, pointsTxt, roundTxt;

    private void Update()
    {
        UpdateAmmo();
        UpdateGrenadeAmount();
        UpdateRound();
        UpdateGamePoints();
    }

    private void UpdateAmmo()
    {

    }

    private void UpdateGrenadeAmount()
    {

    }

    private void UpdateGamePoints()
    {
        
    }

    private void UpdateRound()
    {

    }
}
