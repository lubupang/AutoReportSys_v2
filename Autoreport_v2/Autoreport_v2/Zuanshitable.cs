using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoreport_v2
{
    public class Zuanshitable : Sourcetable
    {
        private readonly string[] names = new string[5];

        private readonly string[][] needdims = new string[5][];
        public enum Tablename
        {
            zuanshi_target = 0,//定向报表
            zuanshi_creative = 1,//创意报表
            zuanshi_campaign = 2,//计划报表
            zuanshi_adzone = 3,//资源位报表
            zuanshi_adgroup = 4//单元报表
        }

        public Zuanshitable(Range range) : base(range)
        {
            this.type = "zuanshi";
            this.datefieldname = "date";
            names[0] = "zuanshi_target";
            names[1] = "zuanshi_creative";
            names[2] = "zuanshi_campaign";
            names[3] = "zuanshi_adzone";
            names[4] = "zuanshi_adgroup";
            needdims[0] = new string[2];
            needdims[0][0] = "target_name";
            needdims[0][1] = "target_id";
            needdims[1] = new string[2];
            needdims[1][0] = "creative_name";
            needdims[1][1] = "creative_id";
            needdims[2] = new string[0];
            needdims[3] = new string[2];
            needdims[3][0] = "adzone_name";
            needdims[3][1] = "adzone_id";
            needdims[4] = new string[2];
            needdims[4][0] = "adgroup_name";
            needdims[4][1] = "adgroup_id";
            int indexk = 2;
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
