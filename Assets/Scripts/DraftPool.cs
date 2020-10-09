using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DraftPool : NetworkBehaviour
{
    public static DraftPool instance = null;
    public CardDisplay cardPrefab;
    public List<Card> cards = new List<Card>();

    private void Awake()
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
        for (int i = 0; i < 15; i++)
        {
            int rng = Random.Range(0, cards.Count);
            newPack.Add(cards[rng]);
            cards.RemoveAt(rng);
        }
        return newPack;
    }

    public void DisplayPack(List<Card> pack, GameObject ob)
    {
        for (int i = 0; i < pack.Count; i++)
        {
            CardDisplay card = Instantiate(cardPrefab, transform);
            card.displaying = pack[i];
            float transformAmount = ((float) i % 5) - ((float)pack.Count / 3 - 1) / 2;
            float angle = transformAmount;
            Vector3 position = new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad) * 35f,
                (i / 5) - 1,
                0
                ) * 3f;
            card.transform.localPosition = position;
            NetworkServer.Spawn(card.gameObject, ob);
        }
    }
}
