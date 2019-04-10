using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoreport_v2
{
    public class SYCMSHOPtable:Sourcetable
    {
        private readonly string[] names = new string[5];

        private readonly string[][] needdims = new string[5][];
        public enum Tablename
        {
            shop_by_category = 0,//整体
            shop_by_hour = 1,//店铺分时
            shop_by_keyword = 2,//店铺分词
            shop_by_source = 3,//店铺分渠道
            shop_by_day = 4//店铺分类目
        }

        public SYCMSHOPtable(Range range) : base(range)
        {
            this.type = "shop";
            this.datefieldname = "date";
            names[0] = "shop_by_category";
            names[1] = "shop_by_hour";
            names[2] = "shop_by_keyword";
            names[3] = "shop_by_source";
            names[4] = "shop_by_day";
            needdims[0] = new string[3];
            needdims[0][0] = "category_1";
            needdims[0][1] = "category_2";
            needdims[0][2] = "category_3";
            needdims[1] = new string[2];
            needdims[1][0] = "hour";
            needdims[2] = new string[2];
            needdims[2][0] = "keyword";
            needdims[2][1] = "pcormb";
            needdims[3] = new string[3];
            needdims[3][0] = "source";
            needdims[3][1] = "sourcedetial";
            needdims[3][2] = "pcormb";
            needdims[4] = new string[0];
            int indexk = 4;
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
            if (findtime > 2) { throw new Exception("没法从现有表组合出这样的维度"); }
            else
            {
                if (findtime == 2 && indexk != 4) { throw new Exception("没法从现有表组合出这样的维度"); }
                else
                {
                    if (findtime < 2)
                    {
                        this.name = names[indexk];
                    }
                    else { this.name = names[4]; }
                }
            }



        }

    }
}
