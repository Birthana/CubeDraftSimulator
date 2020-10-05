using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public Card card;

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
