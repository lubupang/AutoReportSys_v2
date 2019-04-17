using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoreport_v2
{
    public class Rangeforitem:Range
    {
        public Rangeforitem(Item item, int id) : base(item.report.module, id)
        {

            this.item = item;
            this.report = item.report;
            this.effect = report.effects[id ];
            this.effecttype = report.effecttypes[id ];
            this.starttime = report.starttimes[id ];
            this.endtime = report.endtimes[id ];
            this.endwherepart = report.endwhereparts[id ];
            this.limitsstr = report.limits[id];

            Getsqlstring();
            this.Getcsvfullname();
        }
        private void Getsqlstring()
        {
            string selectpart;
            string groupbypart;
            string orderpart;
            string campaign_modelstr;
            string effecttypestr;
            string effectstr;
            switch (sourcetype)
            {
                case "subway":
                    campaign_modelstr = "";
                    effecttypestr = "";
                    effectstr = " and  effect='" + effect + "'";
                    break;
                case "zuanshi":
                    campaign_modelstr = " and campaign_model='";
                    effectstr = " and  effect='" + effect + "'";
                    if (campaign_models == "1" || campaign_models == "4" || campaign_models == "8" || campaign_models == "9")
                    {
                        campaign_modelstr = campaign_modelstr + campaign_models + "'";
                    }
                    else { campaign_modelstr = ""; }
                    if (effecttype == "click" || effecttype == "impression")
                    {
                        effecttypestr = " and effect_type='" + effecttype + "'";
                    }
                    else { effecttypestr = " and effect_type='click'"; }
                    break;
                default:
                    effectstr = "";
                    campaign_modelstr = "";
                    effecttypestr = " ";
                    break;
            }


            sqlstring = "";
            selectpart = "select ";
            if (dims.Length == 1 && dims[0] == "") { groupbypart = ""; }
            else
            {
                groupbypart = " group by ";
                for (int i = 0; i < dims.Length; i++)
                {
                    selectpart = selectpart + dims[i] + ",";
                    groupbypart = groupbypart + dims[i] + ",";
                }
                groupbypart = groupbypart.Substring(0, groupbypart.Length - 1);
            }
            if (fields.Length == 1 && fields[0] == "")
            {
                orderpart = ""; selectpart = selectpart.Substring(0, selectpart.Length - 1);
            }
            else
            {
                orderpart = " order by sum(" + fields[0] + ") desc";
                for (int i = 0; i < fields.Length; i++)
                {
                    selectpart = selectpart + "sum(" + fields[i] + "),";
                }
                selectpart = selectpart.Substring(0, selectpart.Length - 1);
            }
            sqlstring = selectpart + " from " + sourcetable.name + " where " + sourcetable.datefieldname + " between " + "'" + starttime.ToString("yyyy/MM/dd") + "' and '" + endtime.ToString("yyyy/MM/dd") + "' and " + "nick='" + item.branding + "'"+ effectstr + campaign_modelstr + effecttypestr + " " + endwherepart + groupbypart + orderpart+limitsstr;
        }
        private void Getcsvfullname()
        {
            this.csvname = item.name + "_" + rangename + "_" + DateTime.Now.ToString("yyyyMMddhhmmss");
        }

    }
}
