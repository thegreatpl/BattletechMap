using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    [Serializable]
    public class StarOwnership
    {
        public string StarId;

        /// <summary>
        /// Year of this ownership. 
        /// </summary>
        public string Year;

        /// <summary>
        /// The faction who currently owns this star. 
        /// </summary>
        public string FactionCode;

        /// <summary>
        /// Any regional data for the period. 
        /// </summary>
        public string Region;

        /// <summary>
        /// Any subregion information for the period. 
        /// </summary>
        public List<string> SubRegion;

        /// <summary>
        /// Is this system a capital. 
        /// </summary>
        public bool IsCapital;

        /// <summary>
        /// Whether or not the system is disputed. 
        /// </summary>
        public bool IsDisputed;

        /// <summary>
        /// Whether or not the system is hidden and not generally known about. 
        /// </summary>
        public bool IsHidden; 
    }
}
