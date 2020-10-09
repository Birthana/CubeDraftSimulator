using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    //public GameObject cardTest;
    public List<string> main = new List<string>();
    public List<string> extra = new List<string>();

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        if (hasAuthority)
            CmdSpawnCard();
    }

    [Command]
    void CmdSpawnCard()
    {
        List<Card> pack = DraftPool.instance.OpenPack();
        DraftPool.instance.DisplayPack(pack, gameObject);
    }

    [ClientRpc]
    void RpcSpawnCard(GameObject card)
    {
        if (hasAuthority)
        {
            if(card != null)
            {
                card.SetActive(true);
                Debug.Log("Card shown to owner player.");
            }
        }
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
