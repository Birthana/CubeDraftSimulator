using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardDisplay : NetworkBehaviour
{
    [SyncVar]public GameObject card;

    private void Start()
    {
        if (!hasAuthority)
        {
            gameObject.SetActive(false);
        }
    }

    void OnMouseEnter()
    {
        CardInformationManager.instance.Display(card.GetComponent<Card>());
    }

    public GameObject displaying
    {
        set
        {
            card = value;
        }
        get
        {
            return card;
        }
    }
}
