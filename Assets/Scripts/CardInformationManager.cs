using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardInformationManager : MonoBehaviour
{
    public static CardInformationManager instance = null;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardInformation;
    public TextMeshProUGUI cardEffect;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Display(Card card)
    {
        cardName.text = card.cardName;
        cardInformation.text = (card.cardType.Equals("Monster")) ? "Card Type: " + card.cardType + " / " + card.subType + "\nType: " + card.type + "\nAttribute: " + card.attribute + "\nLevel/Link: " + card.level + "\nAttack: " + card.atk
                                    : "Card Type: " + card.cardType + " / " + card.subType;
        cardEffect.text = card.cardEffect;
    }
}
