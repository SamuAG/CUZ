using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayToWin : MonoBehaviour, IInteract
{
    [SerializeField] GameManagerSO gM;
    [SerializeField] int price;
    [SerializeField] private TMPro.TMP_Text title;
    [SerializeField] private TMPro.TMP_Text priceText;

    // TODO: implementar cobrar al jugador cuando interactúe con el objeto
    private void Start()
    {
        title.text = "Pay to Win";
        priceText.text = price.ToString();
    }

    public void interact()
    {
        if (gM.Points < price)
            return;

        gM.WinGame();
    }
}
