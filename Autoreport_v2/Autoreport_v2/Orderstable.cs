using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoreport_v2
{
    public class Orderstable:Sourcetable 
    {
        public Orderstable(Range range) : base(range)
        {
            this.type = "ordersinfo";
            this.name = "orders_detial";
            this.datefieldname = "time_createorder";

        }
    }
}
