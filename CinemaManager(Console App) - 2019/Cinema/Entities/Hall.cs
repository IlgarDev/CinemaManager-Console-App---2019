using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Entities
{
    public class Hall
    {
        public string HallTitle { get; set; }
        public uint Rows { get; set; }
        public uint RowsbySeats { get; set; }
        public string Type { get; set; }
        public List<Site> Sites = new List<Site>();

        public string ShowInfo()
        {
            return ($"\n  Title: \t {HallTitle}\n" +
                $"  Rows: \t {Rows}\n" +
                $"  Seats by rows: {RowsbySeats}\n" +
                $"  Seats: \t {Rows * RowsbySeats}\n" +
                $"  Type: \t {Type}"
                );
        }

        public override string ToString()
        {
            return ($"{HallTitle}  ({Type} , {Rows * RowsbySeats} seats)");
        }

    }

    public enum HallType
    {
        D3 = 1,
        D2 = 2,
        XAMI = 3
    }
}
