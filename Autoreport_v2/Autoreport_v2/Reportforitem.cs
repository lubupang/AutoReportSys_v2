using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Autoreport_v2
{
    public class Reportforitem: Report
    {
        public Reportforitem(Item item) : base(item.reportid)
        {
            this.item = item;
            effects = new string[ranges_count];
            effecttypes = new string[ranges_count];
            starttimes = new DateTime[ranges_count];
            endtimes = new DateTime[ranges_count];
            endwhereparts = new string[ranges_count];
            limits= new string[ranges_count];
            string[] index;

            switch (report["effects"].ToString())
            {
                case "0":
                    for (int i = 0; i < ranges_count; i++) { effects[i] = item.effect; }
                    break;
                default:
                    index = report["effects"].ToString().Split('|');
                    for (int i = 0; i < ranges.Length; i++) { effects[i] = index[i]; }
                    break;
            }
            switch (report["effecttypes"].ToString())
            {
                case "0":
                    for (int i = 0; i < ranges_count; i++) { effecttypes[i] = item.effecttype; }
                    break;
                default:
                    index = report["effecttypes"].ToString().Split('|');
                    for (int i = 0; i < ranges.Length; i++) { effecttypes[i] = index[i]; }
                    break;
            }
            switch (report["starttimes"].ToString())
            {
                case "0":
                    for (int i = 0; i < ranges_count; i++) { starttimes[i] = item.datastarttime; }
                    break;
                default:
                    index = report["starttimes"].ToString().Split('|');
                    for (int i = 0; i < ranges.Length; i++) { starttimes[i] = Convert.ToDateTime(index[i]); }
                    break;
            }
            switch (report["endtimes"].ToString())
            {
                case "0":
                    for (int i = 0; i < ranges_count; i++) { endtimes[i] = item.dataendtime; }
                    break;
                default:
                    index = report["endtimes"].ToString().Split('|');
                    for (int i = 0; i < ranges.Length; i++) { endtimes[i] = Convert.ToDateTime(index[i]); }
                    break;
            }
            switch (report["endwhereparts"].ToString())
            {
                case "0":
                    for (int i = 0; i < ranges_count; i++) { endwhereparts[i] = ""; }
                    break;
                default:
                    index = report["endwhereparts"].ToString().Split('|');

                    for (int i = 0; i < ranges.Length; i++)
                    {
                        if(index[i].ToString()!="0" && index[i].ToString() != "")
                        {
                            endwhereparts[i] = " and " + index[i];

                        }

                    }

                    break;
            }
            switch (report["limits"].ToString())
            {
                case "0":
                    for (int i = 0; i < ranges_count; i++) { effects[i] = item.effect; }
                    break;
                default:
                    index = report["limits"].ToString().Split('|');
                    for (int i = 0; i < ranges.Length; i++) { effects[i] = index[i]; }
                    break;
            }

        }
    }
}
