using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Autoreport_v2
{
    public class Report
    {
        public Item item;
        string reportid;
        string name;
        string moduleid;
        public JObject[] ranges;
        public string[] range_indexes;
        public string[] effects;
        public string[] effecttypes;
        public string[] limits;
        public DateTime[] starttimes;
        public DateTime[] endtimes;
        public string[] endwhereparts;
        public ExcelModule module;
        public Rangeforitem[] _ranges;
        public int ranges_count;
        public string[] csvfullnames;
        public string[] sheets;
        public int[] startrows;
        public int[] startcols;
        public string file;
        public JObject report;
        public Report(string reportid)
        {
            this.reportid = reportid;
            report =(JObject) Client.reports[reportid];
            name = report["name"].ToString();
            moduleid = report["moduleid"].ToString();
            module = new ExcelModule((JObject)Client.modules[moduleid]);
            string[] index;
            switch (report["ranges"].ToString())
            {
                case "0":
                    ranges = module.ranges;
                    range_indexes = new string[ranges.Length];

                    for (int i = 0; i < ranges.Length; i++)
                    {
                        range_indexes[i] = (i + 1).ToString();
                    }
                    
                    break;
                case "ALL":
                    ranges = module.ranges;
                    range_indexes = new string[ranges.Length];

                    for (int i = 0; i < ranges.Length; i++)
                    {
                        range_indexes[i] = (i + 1).ToString();
                    }

                    break;
                default:
                    index = report["ranges"].ToString().Split('|');
                    index.CopyTo(range_indexes, 0);
                    ranges = new JObject[index.Length];
                    for(int i = 0; i < ranges.Length; i++) { ranges[i] = module.ranges[Convert.ToInt32(index[i]) - 1]; }
                    break;
            }
            ranges_count = ranges.Length;

        }
        public void Ini()
        {

            _ranges = new Rangeforitem[ranges_count];
            for (int i = 0; i < ranges_count; i++)
            {
                _ranges[i] = new Rangeforitem(item, Convert.ToInt32(range_indexes[i])-1);
            }
            csvfullnames = new string[ranges_count];
            sheets = new string[ranges_count];
            startrows = new int[ranges_count];
            startcols = new int[ranges_count];
            for (int i = 0; i < ranges_count; i++)
            {
                csvfullnames[i] = _ranges[i].csvfullname;
                sheets[i] = _ranges[i].sheetname;
                startrows[i] = _ranges[i].startrow;
                startcols[i] = _ranges[i].startcol;
            }

            file = Client.outreportpath + item.name + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsm";
            int k = 0;
            while (File.Exists(file))
            {
                file = file + "(" + k + ")";
                k++;
            }

        }
    }
}
