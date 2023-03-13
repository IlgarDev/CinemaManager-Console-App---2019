using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cinema.Entities
{
    [Serializable]
    [XmlType(TypeName = "Movie")]
    public class Movie
    {

        [XmlAttribute(AttributeName = "FilmName")]
        public string FilmName { get; set; }
        [XmlAttribute(AttributeName = "yearOfIssue")]
        public uint yearOfIssue { get; set; }
        [XmlAttribute(AttributeName = "Genre")]
        public string Genre;
        [XmlAttribute(AttributeName = "Producer")]
        public string Producer { get; set; }
        [XmlAttribute(AttributeName = "Сondition")]
        public string Сondition { get; set; }


        public string ShowInfo()
        {
            return ($"\n  Имя: \t\t{FilmName}\n" +
                $"  Дата релиза: \t{yearOfIssue}\n" +
                $"  Жанр: \t{Genre}\n" +
                $"  Продюссер: \t{Producer}\n" +
                $"  Состояние: \t{Сondition}\n");
        }

        public override string ToString()
        {
            return ($"{FilmName} / {yearOfIssue} / {Genre}");
        }

    }


    public enum Genres
    {
        Comedy = 1,
        History = 2,
        Sport = 3,
        Melodrama = 4
    }


    public enum Conditions
    {
        CURRENT = 1,
        FUTURE = 2,
        PAST = 3
    }
}
