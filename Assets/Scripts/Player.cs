using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<string> main = new List<string>();
    public List<string> extra = new List<string>();

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
