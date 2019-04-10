using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoreport_v2
{
    public class Starshop:Sourcetable
    {
        public enum Tablename
        {
            brandshop_campaign_report = 0,//品专表
        }

        public Starshop(Range range) : base(range)
        {
            this.type = "star";
            this.datefieldname = "thedate";
            this.name = "";

        }

    }
}
