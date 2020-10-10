using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public CardDisplay cardPrefab;
    public List<string> main = new List<string>();
    public List<string> extra = new List<string>();

    [Client]
    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        if (hasAuthority)
            CmdSpawnPack();
    }

    [Command]
    void CmdSpawnPack()
    {
        List<Card> pack = DraftPool.instance.OpenPack();
        for (int i = 0; i < pack.Count; i++)
        {
            CardDisplay card = Instantiate(cardPrefab, transform);
            card.displaying = pack[i];
            float transformAmount = ((float)i % 5) - ((float)pack.Count / 3 - 1) / 2;
            float angle = transformAmount;
            Vector3 position = new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad) * 35f,
                (i / 5) - 1,
                0
                ) * 3f;
            card.transform.localPosition = position;
            NetworkServer.Spawn(card.gameObject, gameObject);
            RpcSetParent(card.gameObject);
        }
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
