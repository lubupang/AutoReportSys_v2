using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoreport_v2
{
    public class Brandshop:Sourcetable
    {
        public enum Tablename
        {
            brandshop_campaign_report = 0,//品专表
        }

        public Brandshop(Range range) : base(range)
        {
            this.type = "branding";
            this.datefieldname = "thedate";
            this.name = "brandshop_campaign_report";
        }

    }
}
