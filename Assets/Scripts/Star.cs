using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    [Serializable]
    public class Star
    {
        public string ID;

        public string SystemName;

        public List<string> AlternativeNames;

        /// <summary>
        /// System X coordinate. 
        /// </summary>
        public string X;

        /// <summary>
        /// System Y Coordinate. 
        /// </summary>
        public string Y;

        public string size;

        public string SarnaLink;

        /// <summary>
        /// List of all ownership information. year/data
        /// </summary>
        public Dictionary<string, StarOwnership> Ownerships; 
    }
}
