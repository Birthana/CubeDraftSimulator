using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardDisplay : NetworkBehaviour
{
    public Card card;
    public string cardName;

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
            cardName = card.cardName;
        }
        get
        {
            return card;
        }
    }
}
