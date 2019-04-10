using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoreport_v2
{
    public class SYCMITEMtable:Sourcetable
    {
        private readonly string[] names = new string[2];

        private readonly string[][] needdims = new string[2][];
        public enum Tablename
        {
            item_by_day = 0,//整体
            item_by_sku = 1,//分SKU
    
        }

        public SYCMITEMtable(Range range) : base(range)
        {
            this.type = "item";
            this.datefieldname = "date";
            names[0] = "item_by_day";
            names[1] = "item_by_sku";
            needdims[0] = new string[0];
            needdims[1] = new string[2];
            needdims[1][0] = "skuid";
            needdims[1][1] = "skuname";
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
                    else { this.name = names[0]; }
                }
            }



        }


    }
}
