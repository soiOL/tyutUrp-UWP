using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urp.Struct
{
    public class Blank
    {
    }

    public class EcardStatus
    {
        public int total { get; set; }
        public string resultCode { get; set; }
        public List<Blank> value { get; set; }
    }
}
