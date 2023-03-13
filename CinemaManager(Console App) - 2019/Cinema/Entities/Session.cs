using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Entities
{
    [Serializable]
    public class Session
    {
        public Hall hall { get; set; }
        public Movie movie { get; set; }
        public DateTime dateTime { get; set; } = new DateTime();
        public List<Ticket> tickets { get; set; } = new List<Ticket>();

        public Session()
        {
            hall = new Hall();
            movie = new Movie();
            dateTime = new DateTime();
            tickets = new List<Ticket>();
        }
        public override string ToString()
        {
            return ($"{movie.FilmName} / { hall.HallTitle} / {dateTime.Day}.{dateTime.Month}.{dateTime.Year} {dateTime.Hour}:{dateTime.Minute}");
        }
    }
}
