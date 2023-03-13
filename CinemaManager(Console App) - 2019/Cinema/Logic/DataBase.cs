using Cinema.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Logic
{
    public class DataBase
    {
        public List<Movie> Movies { get; set; }

        public List<Hall> Halls { get; set; }
        public List<Session> Sessions { get; set; }

        public DataBase()
        {
            Movies = new List<Movie>();
            Halls = new List<Hall>();
            Sessions = new List<Session>();
        }
    }
}
