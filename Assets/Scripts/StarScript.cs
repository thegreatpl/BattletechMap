using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    public SpriteRenderer Sprite; 

    public Star Star;

    public StarOwnership CurrentStarOwnership;

    public Faction Faction; 

    string date; 

    // Start is called before the first frame update
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        date = ""; 
        if (Star != null)
            LoadStarData(); 

    }

    // Update is called once per frame
    void Update()
    {
        if (date != GameManager.Instance.CurrentDate)
            LoadOwnershipInfo(); 
    }

    void LoadStarData()
    {
        gameObject.name = Star.SystemName; 
        try
        {
            transform.position = new Vector3(float.Parse(Star.X), float.Parse(Star.Y)); 
        }
        catch (Exception e)
        {
            gameObject.SetActive(false);
            return; 
        }
        LoadOwnershipInfo(); 
    }


    void LoadOwnershipInfo()
    {
        var date = GameManager.Instance.CurrentDate;
        try
        {
            //need to handle if there is no data. 
            CurrentStarOwnership = Star.Ownerships[date];

            //no data on the system. 
            if (string.IsNullOrWhiteSpace(CurrentStarOwnership.FactionCode))
            {
                Sprite.color = Color.white;
            }

            else
            {
                //some form of independent polity. 
                if (CurrentStarOwnership.FactionCode.Contains("I("))
                    Faction = GameManager.Instance.Factions["I"];
                else if (CurrentStarOwnership.FactionCode.Contains("U (A)") || CurrentStarOwnership.FactionCode.Contains("U(A)")) //is this a faction missing from the list?
                    Faction = GameManager.Instance.Factions["U"];
                else if (CurrentStarOwnership.IsDisputed)
                {
                    Faction = GameManager.Instance.Factions["D"]; 
                }
                else
                    Faction = GameManager.Instance.Factions[CurrentStarOwnership.FactionCode];

                Sprite.color = Faction.UnityColour;
            }

        }
        catch (Exception e)
        {
            Debug.LogError(e);

            Sprite.color = Color.clear; 
        }


    }


}
