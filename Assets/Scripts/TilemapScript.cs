using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapScript : MonoBehaviour
{

    /// <summary>
    /// The Tilemap in use. 
    /// </summary>
    public Tilemap Tilemap;

    /// <summary>
    /// The current date. 
    /// </summary>
    public string date;


    Dictionary<string, List<TileOwnershipInfo>> TileOwnershipInfo; 

    // Start is called before the first frame update
    void Start()
    {
        Tilemap = GetComponent<Tilemap>();
        TileOwnershipInfo = new Dictionary<string, List<TileOwnershipInfo>>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(date != GameManager.Instance.CurrentDate)
            refreshMap();
    }


    void refreshMap()
    {
        Tilemap.ClearAllTiles(); 
        date = GameManager.Instance.CurrentDate;
        StartCoroutine(FillInPoliticalBorders()); 
    }

    IEnumerator FillInPoliticalBorders()
    {
        if (!TileOwnershipInfo.ContainsKey(date))
            yield break;
        int idx = 0;

        Dictionary<string, OwnershipTile> tiles = new Dictionary<string, OwnershipTile>(); 

        foreach (var tile in TileOwnershipInfo[date])
        {
            var polity = tile.PolityDistances.OrderBy(x => x.Distance).FirstOrDefault();

            if (polity.Faction?.ID == null)
                continue;

            if (!tiles.ContainsKey(polity.Faction.ID))
            {
                var newtile = ScriptableObject.CreateInstance<OwnershipTile>();
                newtile.color = polity.Faction.UnityColour;
                newtile.sprite = GameManager.Instance.tilesprite; 
                tiles.Add(polity.Faction.ID, newtile); 
            }
            Tilemap.SetTile(tile.Position, tiles[polity.Faction.ID]); 

            idx++; 
            if (idx > 1000)
            {
                yield return null;
                idx = 0; 
            }
        }
    }

    

    public IEnumerator GeneratePoliticalBorders()
    {
        int dist = 30; 
        TileOwnershipInfo = new Dictionary<string, List<TileOwnershipInfo>>(); 

       
        foreach(var date in GameManager.Instance.Dates)
        {
            if (TileOwnershipInfo.ContainsKey(date))//figure out how to deal with the multiple years. 
                continue; 

            int idx = 0; 
            Dictionary<Vector3Int, TileOwnershipInfo> ownerships = new Dictionary<Vector3Int, TileOwnershipInfo>(); 
            foreach (var star in GameManager.Instance.Systems)
            {
                
                if (!star.Star.Ownerships.ContainsKey(date))
                    continue; 
                var ownership = star.Star.Ownerships[date];
                if (ownership.FactionCode == "U" || ownership.IsHidden) //unihabited, can skip over it. 
                    continue;

                var nearestCell = Tilemap.WorldToCell(star.transform.position);
                for (int xdx = nearestCell.x - dist; xdx < nearestCell.x + (dist * 2); xdx++)
                {
                    for (int ydx = nearestCell.y - dist; ydx < nearestCell.y + (dist *2); ydx++)
                    {
                        var coord = new Vector3Int(xdx, ydx, 0);
                        var distance = Vector3.Distance(Tilemap.CellToWorld(coord), star.transform.position); 
                        if (distance <= dist)
                        {
                            if (!ownerships.ContainsKey(coord))
                                ownerships.Add(coord, new global::TileOwnershipInfo()
                                {
                                    date = date,
                                    Position = coord
                                });

                            var factioncode = star.Star.Ownerships[date]?.FactionCode;
                            if (factioncode == null || !GameManager.Instance.Factions.ContainsKey(factioncode))
                                continue;

                            ownerships[coord].PolityDistances.Add(new PolityDistance(distance, GameManager.Instance.Factions[factioncode]));
                        }
                    }
                }
                idx++; 
               // yield return null; 
               if (idx > 10)
                {
                    yield return null;
                    idx = 0; 
                }


            }

            TileOwnershipInfo.Add(date, ownerships.Values.ToList());
            yield return null;
           // break; 
        }

        yield return FillInPoliticalBorders(); 
    }

}
