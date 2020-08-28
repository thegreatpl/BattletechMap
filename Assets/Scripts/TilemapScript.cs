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


    List<TileOwnershipInfo> TileOwnershipInfo; 

    // Start is called before the first frame update
    void Start()
    {
        Tilemap = GetComponent<Tilemap>();
        TileOwnershipInfo = new List<TileOwnershipInfo>(); 
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
        //if (!TileOwnershipInfo.ContainsKey(date))
        //    yield break;
        int idx = 0;

        Dictionary<string, OwnershipTile> tiles = new Dictionary<string, OwnershipTile>();

        //ignore these faction codes when creating borders. 
        string[] missingfactioncodes = { "U", "", "A" }; 

        foreach (var tile in TileOwnershipInfo)
        {
            var polity = tile.PolityDistances.OrderBy(x => x.Distance).
                FirstOrDefault(x => (bool)!x.Star.Ownerships[date]?.IsHidden 
                    && !missingfactioncodes.Contains(x.Star.Ownerships[date]?.FactionCode));

            var factioncode = polity?.Star.Ownerships[date]?.FactionCode;

            if (factioncode == null)
                continue;

            if (!GameManager.Instance.Factions.ContainsKey(factioncode))
                continue; 
            var faction = GameManager.Instance.Factions[factioncode]; 



            if (polity.Star?.Ownerships[date]?.FactionCode == null)
                continue;

            if (!tiles.ContainsKey(faction.ID))
            {
                var newtile = ScriptableObject.CreateInstance<OwnershipTile>();
                newtile.color = faction.UnityColour;
                newtile.sprite = GameManager.Instance.tilesprite; 
                tiles.Add(faction.ID, newtile); 
            }
            Tilemap.SetTile(tile.Position, tiles[faction.ID]); 

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
        var ownerships = new Dictionary<Vector3Int, TileOwnershipInfo>();

        int idx = 0;
        foreach (var star in GameManager.Instance.Systems)
        {
            var nearestCell = Tilemap.WorldToCell(star.transform.position);
            for (int xdx = nearestCell.x - dist; xdx < nearestCell.x + (dist * 2); xdx++)
            {
                for (int ydx = nearestCell.y - dist; ydx < nearestCell.y + (dist * 2); ydx++)
                {
                    var coord = new Vector3Int(xdx, ydx, 0);
                    var distance = Vector3.Distance(Tilemap.CellToWorld(coord), star.transform.position);
                    if (distance <= dist)
                    {
                        if(!ownerships.ContainsKey(coord))
                        {
                            ownerships.Add(coord, new global::TileOwnershipInfo()
                            {
                                Position = coord
                            });
                        }
                        ownerships[coord].PolityDistances.Add(new PolityDistance(distance, star.Star)); 
                    }
                }
            }
            idx++; 
            if (idx % 10 == 0)
            {
                yield return null; 
            }
        }

        TileOwnershipInfo = ownerships.Values.ToList();

        yield return FillInPoliticalBorders(); 
    }

}
