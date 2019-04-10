using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace Autoreport_v2
{
    public class Item
    {
        public string id;
        public string name;
        public string branding;
        public string reportid;
        public string itemstarttime;
        public string effect;
        public string effecttype;
        public string[] emails;
        public DateTime datastarttime;
        public DateTime dataendtime;


        public Report report;
        public Rangeforitem[] ranges;
        public string reportfile;
        public Item(string itemid)
        {
            JObject item = (JObject)Client.items[itemid];
            id = itemid;
            name = item["name"].ToString();
            branding = item["brandingname"].ToString();
            reportid = item["reportid"].ToString();
            itemstarttime = item["itemstarttime"].ToString();
            effect = item["effect"].ToString();
            effecttype = item["effecttype"].ToString();
            emails = item["emails"].ToString().Split('|');
            datastarttime = Convert.ToDateTime(item["datastarttime"].ToString());
            dataendtime = Convert.ToDateTime(item["dataendtime"].ToString());
            report = new Reportforitem(this);
            report.Ini();
            ranges = report._ranges;

        }
        public Item(JObject  item)
        {
            name = item["name"].ToString();
            branding = item["brandingname"].ToString();
            reportid = item["reportid"].ToString();
            itemstarttime = item["itemstarttime"].ToString();
            effect = item["effect"].ToString();
            effecttype = item["effecttype"].ToString();
            emails = item["emails"].ToString().Split('|');
            datastarttime = Convert.ToDateTime(item["datastarttime"].ToString());
            dataendtime = Convert.ToDateTime(item["dataendtime"].ToString());
            report = new Reportforitem(this);
            report.Ini();
            ranges = report._ranges;

        }


        public Item Copy()
        {
            Item copyitem = this;
            return copyitem;
        }



    }
}
