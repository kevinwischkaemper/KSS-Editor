using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC1_Router
{
    public class Part
    {
        // Obtained from KSS
        public List<Tuple<string,int>> AssemblyMarks = null;
        public bool IsMain;
        public string DrawingNumber;
        public int Quantity;
        public string PieceMark;
        public string Sequence; //kss parsing only assigns sequences to main members
        public string Finish;
        public string MainMember;

        // Obtained from nc1
        public double Length; // in mm
        public string ShapeType;
        public string ShapeDetails;
        public string Grade;
        public List<string> RoutingCodes;
        public double Weight;  // in kgs
        public double ProfileHeight; // in mm
        public double FlangeWidth; // in mm


    }
}
