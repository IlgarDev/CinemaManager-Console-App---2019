using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Entities
{
    public class Site
    {
        public int Num { get; set; }
        public bool IsFree { get; set; }
        public Site() { }

        public Site(int num)
        {
            Num = num;
            IsFree = true;
        }


    }
}
