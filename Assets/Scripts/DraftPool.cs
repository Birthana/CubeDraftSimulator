using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftPool : MonoBehaviour
{
    public List<Card> cards = new List<Card>();
    public List<Card> pack = new List<Card>();

    private void Start()
    {
        TextAsset csv = Resources.Load<TextAsset>("pool");

        string[] data = csv.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            Card card = ScriptableObject.CreateInstance<Card>();
            card.cardName = row[0];
            int.TryParse(row[10], out card.passcode);
            card.cardType = row[1];
            card.type = row[3];
            card.attribute = row[4];
            int.TryParse(row[5], out card.level);
            int.TryParse(row[6], out card.atk);

            //Debug.Log(row[0] + " " + row[10] + " " + row[1] + " " + row[3] + " " + row[4] + " " + row[5] + " " + row[6]);

            cards.Add(card);
        }

        OpenPack();
    }

    public void OpenPack()
    {
        for (int i = 0; i < 15; i++)
        {
            int rng = Random.Range(0, cards.Count);
            //Debug.Log(rng + "-" + cards[rng].cardName);
            pack.Add(cards[rng]);
            cards.RemoveAt(rng);
        }
    }
}
