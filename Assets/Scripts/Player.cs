using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public GameObject cardPrefab;
    public SyncListGameObject cards = new SyncListGameObject();
    public List<string> main = new List<string>();
    public List<string> extra = new List<string>();

    [Client]
    void Start()
    {
        //StartGame();
    }

    [ClientRpc]
    public void RpcStartGame()
    {
        Debug.Log("Player Start");
        if (hasAuthority)
        {
            Debug.Log("Player Start Game");
            CmdSpawnPack();
            for (int i = 15; i < cards.Count; i++)
            {
                cards[i].SetActive(false);
            }
        }
    }

    [Command]
    void CmdSpawnPack()
    {
        int numberOfPacks = DraftPool.instance.cards.Count / DraftPool.instance.numberOfCardsPerPack;
        Debug.Log("" + DraftPool.instance.cards.Count);
        for (int j = 0; j < numberOfPacks / 3; j++)
        {
            List<GameObject> pack = DraftPool.instance.OpenPack();
            for (int i = 0; i < pack.Count; i++)
            {
                GameObject cardObject = Instantiate(cardPrefab, transform);
                NetworkServer.Spawn(cardObject, gameObject);
                RpcSpawnPack(cardObject, i, pack[i], pack.Count);
            }
        }
    }

    [ClientRpc]
    public void RpcSpawnPack(GameObject cardObject, int i , GameObject pack, int packCount)
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
        if(hasAuthority)
            CmdFinishSpawnPack(cardObject);
    }

    [Command]
    public void CmdFinishSpawnPack(GameObject cardObject)
    {
        RpcSetParent(cardObject);
        cards.Add(cardObject);
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
        if (Input.GetMouseButtonDown(0))
        {
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D mouseHit = Physics2D.Raycast(mouseRay.origin, Vector2.zero);
            if (mouseHit)
            {
                Card chosenCard = mouseHit.collider.gameObject.GetComponent<CardDisplay>().displaying.GetComponent<Card>();
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
