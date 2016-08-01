using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC1_Router
{
    public class Difference
    {
        public int ExportLineNumber { get; set; }
        public int GeneratedLineNumber { get; set; }
        public int FieldNumber { get; set; }
        public string ExportValue { get; set; }
        public string GeneratedValue { get; set; }
    }
}
