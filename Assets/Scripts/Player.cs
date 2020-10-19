using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public CardDisplay cardPrefab;
    public List<List<CardDisplay>> packs = new List<List<CardDisplay>>();
    public List<CardDisplay> cards = new List<CardDisplay>();
    public List<string> main = new List<string>();
    public List<string> extra = new List<string>();

    [ClientRpc]
    public void RpcStartGame()
    {
        if (hasAuthority)
        {
            CmdAfterPackSpawn();
        }
    }

    [Command]
    public void CmdAfterPackSpawn()
    {
        int numberOfPacks = 21;
        for (int j = 0; j < numberOfPacks / 3; j++)
        {
            List<Card> pack = DraftPool.instance.OpenPack();
            for (int i = 0; i < pack.Count; i++)
            {
                CardDisplay cardObject = Instantiate(cardPrefab, transform);
                NetworkServer.Spawn(cardObject.gameObject, gameObject);
                RpcSpawnPack(cardObject.gameObject, i, pack[i], pack.Count);
            }
        }
    }

    [ClientRpc]
    public void RpcSpawnPack(GameObject cardObject, int i , Card pack, int packCount)
    {
        CardDisplay card = cardObject.GetComponent<CardDisplay>();
        card.displaying = pack;
        float transformAmount = ((float)i % 5) - ((float)packCount / 3 - 1) / 2;
        float angle = transformAmount;
        Vector3 position = new Vector3(
            Mathf.Sin(angle * Mathf.Deg2Rad) * 35f,
            (i / 5) - 1,
            0
            ) * 3f;
        cardObject.transform.localPosition = position;
        cards.Add(card);
        if (cards.Count == 15)
        {
            packs.Add(cards);
            cards = new List<CardDisplay>();
        }
        if (hasAuthority)
        {
            CmdFinishSpawnPack(cardObject.gameObject);
        }
    }

    [Command]
    public void CmdFinishSpawnPack(GameObject cardObject)
    {
        RpcSetParent(cardObject);
    }

    [ClientRpc]
    public void RpcSetParent(GameObject card)
    {
        card.transform.parent = transform;
        if(isClientOnly)
            card.transform.localPosition += transform.position;
    }

    private void Update()
    {
        if (!hasAuthority) return;
        if (Input.GetMouseButtonDown(0))
        {
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D mouseHit = Physics2D.Raycast(mouseRay.origin, Vector2.zero);
            if (mouseHit)
            {
                Card chosenCard = mouseHit.collider.gameObject.GetComponent<CardDisplay>().displaying;
                Debug.Log(chosenCard.passcode);
                if (chosenCard.subType.Equals("Link"))
                {
                    extra.Add("" + chosenCard.passcode);
                }
                else
                {
                    main.Add("" + chosenCard.passcode);
                }
                //Remove card from list and get next pack to display. For testing remove a random card.
            }
        }
    }

    public string CreateYDKFile()
    {
        string ydkFile = "#created by ...\n#main\n";
        foreach (string passcode in main)
        {
            ydkFile += passcode + "\n";
        }
        ydkFile += "#extra\n";
        foreach (string passcode in extra)
        {
            ydkFile += passcode + "\n";
        }
        ydkFile += "!side";
        return ydkFile;
    }

}
