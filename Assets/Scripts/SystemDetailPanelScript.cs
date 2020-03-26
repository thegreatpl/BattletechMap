using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemDetailPanelScript : MonoBehaviour
{

    public Text Text;    
    Ray ray;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        Text = GetComponent<Text>(); 
    }



    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            var sys = hit.collider.GetComponent<StarScript>();
            string description = "";

            description = $"Name:{sys.Star.SystemName}"; 
            if (sys.Star.AlternativeNames.Count > 0)
            {
                foreach (var alt in sys.Star.AlternativeNames)
                    description += $", {alt}";

            }
            description += Environment.NewLine;

            description += $"Faction Owner: {sys.Faction.Name} {Environment.NewLine}";
            description += $"Region: {sys.CurrentStarOwnership.Region}";
            if (sys.CurrentStarOwnership.SubRegion.Count > 0)
                foreach (var subreg in sys.CurrentStarOwnership.SubRegion)
                    description += $", {subreg}";
            description += Environment.NewLine;

            if (sys.CurrentStarOwnership.IsHidden)
                description += $"Hidden System {Environment.NewLine}";
            if (sys.CurrentStarOwnership.IsCapital)
                description += $"Faction Capital {Environment.NewLine}";
            if (sys.CurrentStarOwnership.IsDisputed)
                description += $"In Dispute {Environment.NewLine}"; 


            Text.text = description; 
        }
        else
        {
            Text.text = "";
        }
    }
}
