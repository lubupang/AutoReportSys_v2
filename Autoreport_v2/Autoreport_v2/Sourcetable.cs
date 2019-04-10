using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoreport_v2
{
    public class Sourcetable
    {
        public string name;
        public string datefieldname;
        public Range range;
        public string type;
        public Sourcetable(Range range)
        {
            this.range = range;

        }
    }
}
