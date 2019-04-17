using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Autoreport_v2
{
    public class Range
    {
        public string rangename;
        public ExcelModule module;
        public Item item;
        public Report report;
        public Sourcetable sourcetable;
        public string sheetname;
        public int startrow;
        public int startcol;
        public string[] dims;
        public string[] fields;
        public string sourcetype;//直通车,钻石,明星店铺,品牌专区
        public string campaign_models;//单品,全店,内容,直播
        public string effecttype;//click or impression
        public string effect;//转化周期
        public DateTime starttime;
        public DateTime endtime;
        public string endwherepart;
        public string limitsstr;
        public string sqlstring;//sqlstring
        public string csvname;//csv的名字不包含后缀
        public string csvfullname;//全路径
        public Boolean successconnect = false;
        public Boolean csvfinished = false;//csv是否创建完成
        public Range(ExcelModule module,int id)
        {
            this.module = module;
            rangename = module.ranges[id]["name"].ToString();
            sheetname = module.ranges[id ]["sheetname"].ToString();
            startrow =Convert.ToInt32( module.ranges[id ]["startrow"].ToString());
            startcol = Convert.ToInt32(module.ranges[id ]["startcol"].ToString());
            dims = module.ranges[id ]["dims"].ToString().Split(',');
            fields = module.ranges[id ]["fields"].ToString().Split(',');
            sourcetype = module.ranges[id ]["sourcetype"].ToString();
            campaign_models = module.ranges[id ]["campaign_models"].ToString();
            switch (sourcetype)
            {
                case "zuanshi":
                    sourcetable = new Zuanshitable(this);
                    break;
                case "subway":
                    sourcetable = new Subwaytable(this);
                    break;
                case "shop":
                    sourcetable = new SYCMSHOPtable(this);
                    break;
                case "item":
                    sourcetable = new SYCMITEMtable(this);
                    break;
                case "ordersinfo":
                    sourcetable = new Orderstable(this);
                    break;
                default:
                    sourcetable = new Sourcetable(this);
                    break;
            }

        }

    }
}
