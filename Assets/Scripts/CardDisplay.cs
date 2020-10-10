using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardDisplay : NetworkBehaviour
{
    [SyncVar]public Card card;

    private void Start()
    {
        if (!hasAuthority)
        {
            gameObject.SetActive(false);
        }
    }

    void OnMouseEnter()
    {
        CardInformationManager.instance.Display(card);
    }

    public Card displaying
    {
        set
        {
            card = value;
            DisplayCard();
        }
        get
        {
            return card;
        }
    }

    public void DisplayCard()
    {

    }
}
