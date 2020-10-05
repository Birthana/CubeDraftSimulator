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
        cardInformation.text = "Card Type: " + card.cardType + "\nType: " + card.cardType + "\nAttribute: " + card.attribute + "\nLevel/Link: " + card.level + "\nAttack: " + card.atk;
        cardEffect.text = card.cardEffect;
    }
}
