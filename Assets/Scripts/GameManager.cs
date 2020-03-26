using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance; 

    /// <summary>
    /// Link to the star prefab. 
    /// </summary>
    public GameObject StarPrefab;

    /// <summary>
    /// The UI Manager of this game. 
    /// </summary>
    public UIManager UIManager; 

    /// <summary>
    /// List of all factions. Code/data.  
    /// </summary>
    public Dictionary<string, Faction> Factions;

    /// <summary>
    /// List of all stars. 
    /// </summary>
    [HideInInspector] //the amount of stars are a lot. 
    public List<Star> Stars;

    /// <summary>
    /// List of all dates available. 
    /// </summary>
    public List<string> Dates;

    /// <summary>
    /// The current date. 
    /// </summary>
    public string CurrentDate; 

    /// <summary>
    /// List of systems that are currently out there. 
    /// </summary>
    [HideInInspector]
    public List<StarScript> Systems;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        UIManager = GetComponent<UIManager>(); 
        StartCoroutine(LoadMapData()); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator LoadMapData()
    {
        foreach (var sys in Systems)
            Destroy(sys.gameObject);
        Systems = new List<StarScript>(); 
        Factions = new Dictionary<string, Faction>();
        Stars = new List<Star>(); 
        yield return null; 

        //faction data. 
        var text = Resources.Load<TextAsset>("Sarna Unified Cartography Kit - Factions");
        foreach(var line in Regex.Split(text.text, "\n|\r|\r\n").Where(x => !string.IsNullOrWhiteSpace(x)))
        {
            var fact = new Faction(line); 
            if (!string.IsNullOrWhiteSpace(fact.ID))
                Factions.Add(fact.ID, fact);
        }
        yield return null;

        yield return StartCoroutine(LoadSystemInformation());

        yield return StartCoroutine(IniatilizeSystems());

        yield return StartCoroutine(UIManager.InitializeUI()); 

    }

    /// <summary>
    /// Loads all the star data from resources. 
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadSystemInformation()
    {
        var starText = Resources.Load<TextAsset>("Sarna Unified Cartography Kit - Systems");
        yield return null;

        var lines = Regex.Split(starText.text, "\n|\r|\r\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        var titleLineel = lines[1].Split('\t');

        int nonDateRows = 8;
        var dates = new string[titleLineel.Length - nonDateRows];
        for (int idx = nonDateRows; idx < titleLineel.Length; idx++)
        {
            dates[idx - nonDateRows] = titleLineel[idx];
        }
        Dates = dates.ToList();
        CurrentDate = Dates[0]; 
        yield return null;

        int count = 0;

        for (int idx = 2; idx < lines.Length; idx++)
        {
            var linesplit = lines[idx].Split('\t');
            if (linesplit.Length < titleLineel.Length)//need to have a value for each column. 
                continue;
            var star = new Star()
            {
                ID = linesplit[0],
                SystemName = linesplit[1],
                AlternativeNames = linesplit[2].Split(',').ToList(),
                X = linesplit[3],
                Y = linesplit[4],
                size = linesplit[5],
                SarnaLink = linesplit[6],
                Ownerships = new Dictionary<string, StarOwnership>()
            };
            for (int jdx = 0; jdx < dates.Length; jdx++)
            {
                var ownerInfo = linesplit[jdx + nonDateRows].Split(',');
                string faction = ownerInfo[0].Trim();
                //handle the disputed systems. 
                bool dispute = false;
                bool isHidden = false; 
                if (ownerInfo[0].Contains("D("))
                {
                    faction = ownerInfo[0] + ", " + ownerInfo[1];
                    dispute = true;
                }
                else if (ownerInfo[0].Contains("(H)"))
                {
                    faction = faction.Replace("(H)", ""); 
                    isHidden = true; 
                }

                var ownership = new StarOwnership()
                {
                    StarId = star.ID,
                    Year = dates[jdx],
                    FactionCode =  faction,
                    IsCapital = false,
                    SubRegion = new List<string>(),  
                    IsDisputed = dispute, 
                    IsHidden = isHidden
                };
                if (ownerInfo.Length > 1 && !dispute)
                {
                    if (ownerInfo[1].Contains("Faction Capital"))
                        ownership.IsCapital = true;
                    else
                        ownership.Region = ownerInfo[1];

                    for (int rdx = 2; rdx < ownerInfo.Length; rdx++)
                    {
                        if (ownerInfo[rdx].Contains("Faction Capital"))
                            ownership.IsCapital = true;
                        else
                            ownership.SubRegion.Add(ownerInfo[rdx]);
                    }
                }
                if (!star.Ownerships.ContainsKey(ownership.Year)) //why do we have duplicate years? 
                    star.Ownerships.Add(ownership.Year, ownership);
            }


            Stars.Add(star);

            count++;
            if (count > 100)
            {
                yield return null;
                count = 0;
            }
        }
    }


    IEnumerator IniatilizeSystems()
    {
        foreach(var star in Stars)
        {
            var newSystem = GameObject.Instantiate(StarPrefab);
            var script = newSystem.GetComponent<StarScript>();
            script.Star = star;
            Systems.Add(script); 
            yield return null; 
        }
    }
}
