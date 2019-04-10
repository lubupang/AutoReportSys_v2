using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Autoreport_v2
{
    public class ExcelModule
    {
        public string file;
        public JObject[] ranges;
        public ExcelModule(JObject module)
        {
            file = @"C:\Sysexcelmodule\" + module["FILE"].ToString();
            int ranges_count = module["RANGES"].Count();
            ranges = new JObject[ranges_count];
            for(int i = 0; i < ranges_count; i++)
            {
                ranges[i] = (JObject) module["RANGES"][i]["INFO"];
            }
        }
    }
}
