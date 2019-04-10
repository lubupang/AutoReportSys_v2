using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoreport_v2
{
    public class Subwaytable : Sourcetable
    {
        private readonly string[] names = new string[3];

        private readonly string[][] needdims = new string[3][];
        public enum Tablename
        {
            subway_sku_by_adgroupid = 0,//直通车宝贝表
            subway_keyword_by_adgroupid = 1,//直通车关键词表
            subway_adgroupid_report = 3,//直通车定向表
        }

        public Subwaytable(Range range) : base(range)
        {
            this.type = "subway";
            names[0] = "subway_sku_by_adgroupid";
            names[1] = "subway_keyword_by_adgroupid";
            names[2] = "subway_adgroupid_report";
            needdims[0] = new string[0];
            needdims[1] = new string[1];
            needdims[1][0] = "keywordstr";
            needdims[2] = new string[1];
            needdims[2][0] = "crowdname";
            int indexk = 0;
            int findtime = 0;
            for (int i = 0; i < needdims.Length; i++)
            {
                if (needdims[i].Length > 0)
                {
                    for (int ii = 0; ii < range.dims.Length; ii++)
                    {
                        for (int j = 0; j < needdims[i].Length; j++)
                        {
                            if (range.dims[ii] == needdims[i][j]) { indexk = i; findtime++; break; }
                        }
                    }
                }
            }
            if (findtime >= 2) { throw new Exception("没法从现有表组合出这样的维度"); }
            else
            {
                this.name = names[indexk];
            }
            switch (name)
            {
                case "subway_adgroupid_report":
                    this.datefieldname = "thedate";
                    break;
                default:
                    this.datefieldname = "date";
                    break;
            }


        }
    }
}
