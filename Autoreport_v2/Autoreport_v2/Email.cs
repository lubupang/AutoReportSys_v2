using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoreport_v2
{
    public class Email
    {
        public string file;
        public string title;
        public string txt;
        public string[] emails;
        public string itemname;
        public Email(Report report, string txt)
        {
            this.file = report.file;
            this.title = report.item.name + "_for_" + report.item.branding;
            this.txt = txt;
            this.emails = report.item.emails;
            this.itemname = report.item.name;
        }

    }
}
