using Cinema.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.UI
{
    class EditMovie : Menuu
    {
        public Movie movie;

        public EditMovie(Movie movie, string title, List<Menuitem> MenuItems)
        {
            this.MenuItems = MenuItems;
            this.movie = movie;
        }


        public override void Run()
        {
            Console.Clear();
            ItemPosition.Reset();
            ItemPosition.Y = 6;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  " + Title);
            Console.WriteLine(movie.ShowInfo());


            foreach (Menuitem item in MenuItems)
            {
                item.Y = ItemPosition.Y += 3;
                item.Draw();
            }


        }

    }

}
