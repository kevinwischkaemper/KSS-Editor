using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC1_Router
{
    public class HoleTooSmall
    {
        public string Piecemark { get; set; }
        public double thickness;
        public string Thickness
        {
            get
            {
                return UnitConverter.ConvertMilsToFracInches(thickness);   
            }
            set
            {
                Thickness = value;
            }    
        }
        public string HoleSize { get; set; }
    }
}
