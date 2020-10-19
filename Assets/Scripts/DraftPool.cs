using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftPool
{
    public static DraftPool _instance = null;
    public int numberOfPlayers;
    public int numberOfCardsPerPack = 15;
    public static List<Card> cards = new List<Card>();

    public static DraftPool instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new DraftPool();
                SpawnCardInfo();
            }
            return _instance;
        }
    }

    public static void SpawnCardInfo()
    {
        TextAsset csv = Resources.Load<TextAsset>("pool");

        string[] data = csv.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            Card card = new Card();
            card.cardName = row[0];
            int.TryParse(row[10], out card.passcode);
            card.cardType = row[1];
            card.subType = row[2];
            card.type = row[3];
            card.attribute = row[4];
            int.TryParse(row[5], out card.level);
            int.TryParse(row[6], out card.atk);
            card.cardEffect = row[11];

            //Debug.Log(row[0] + " " + row[10] + " " + row[1] + " " + row[3] + " " + row[4] + " " + row[5] + " " + row[6] + " " + row[11]);

            cards.Add(card);
        }
    }

    public List<Card> OpenPack()
    {
        List<Card> newPack = new List<Card>();
        for (int i = 0; i < numberOfCardsPerPack; i++)
        {
            int rng = Random.Range(0, cards.Count);
            newPack.Add(cards[rng]);
            Debug.Log(cards[rng].cardName);
            cards.RemoveAt(rng);
        }
        return newPack;
    }

    public void RemovePack(List<Card> pack)
    {
        foreach (Card card in pack)
        {
            foreach (Card cardInfo in cards)
            {
                if (cardInfo.passcode.Equals(card.passcode))
                {
                    cards.Remove(cardInfo);
                    break;
                }
            }
        }
    }

    public void RemoveCard(Card card)
    {
        foreach (Card cardInfo in cards)
        {
            if (cardInfo.passcode.Equals(card.passcode))
            {
                cards.Remove(cardInfo);
                break;
            }
        }
    }

    public int GetCount()
    {
        return cards.Count;
    }
}
