using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class Faction
    {
        public string ID; 

        public string Name;

        /// <summary>
        /// The hex colour that this faction has. 
        /// </summary>
        public string HexColour;

        public Color UnityColour;

        public string FoundingYear;

        public string FallYear; 


        public Faction() { }

        public Faction(string line)
        {
            var data = line.Split('\t');
            ID = data[0].Trim();
            Name = data[1];
            HexColour = data[2];
            FoundingYear = data[3];
            FallYear = data[4];

            AssignUnityColourFromHex(HexColour); 
        }

        /// <summary>
        /// Attempts to parse the hex colour to the RGB colour unity uses. 
        /// </summary>
        /// <param name="hexColour"></param>
        public void AssignUnityColourFromHex(string hexColour)
        {
            ColorUtility.TryParseHtmlString(hexColour, out UnityColour); 
        }

    }
}
