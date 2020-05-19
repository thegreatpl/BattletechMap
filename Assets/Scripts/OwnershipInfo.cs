using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class TileOwnershipInfo
{
    public Vector3Int Position;

    public string date;

    public List<PolityDistance> PolityDistances; 

    public TileOwnershipInfo()
    {
        PolityDistances = new List<PolityDistance>(); 
    }
}

public struct PolityDistance
{
    /// <summary>
    /// The faction. 
    /// </summary>
    public Faction Faction;

    /// <summary>
    /// Distance from the center of the tile to the nearest owned star system. 
    /// </summary>
    public float Distance; 

    public PolityDistance(float distance, Faction faction)
    {
        Distance = distance;
        Faction = faction; 
    }
}
