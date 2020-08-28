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

    public List<PolityDistance> PolityDistances; 

    public TileOwnershipInfo()
    {
        PolityDistances = new List<PolityDistance>(); 
    }
}

public class PolityDistance
{
    /// <summary>
    /// The faction. 
    /// </summary>
    public Star Star;

    /// <summary>
    /// Distance from the center of the tile to the nearest owned star system. 
    /// </summary>
    public float Distance; 

    public PolityDistance(float distance, Star star)
    {
        Distance = distance;
        Star = star; 
    }
}
