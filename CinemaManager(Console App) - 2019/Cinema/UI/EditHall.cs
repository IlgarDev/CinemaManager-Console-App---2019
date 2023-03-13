using Cinema.Entities;
using Cinema.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema.UI
{
    class EditHall : Menuu
    {
        public Hall Hall;

        public EditHall(Hall hall, string title, List<Menuitem> MenuItems)
        {
            this.MenuItems = MenuItems;
            Hall = hall;
        }


        public override void Run()
        {
            Console.Clear();
            ItemPosition.Reset();
            ItemPosition.Y = 6;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  " + Title);
            Console.WriteLine(Hall.ShowInfo());


            foreach (Menuitem item in MenuItems)
            {
                item.Y = ItemPosition.Y += 3;
                item.Draw();
            }


        }

    }
}
