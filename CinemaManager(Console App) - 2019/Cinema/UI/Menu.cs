using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.UI
{


    public class Menuu
    {

        public List<Menuitem> MenuItems;
        public string Title { get; set; }

        public virtual void Run()
        {
            ItemPosition.Reset();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  " + Title);
            foreach (Menuitem item in MenuItems)
            {

                item.Y = ItemPosition.Y += 3;
                item.Draw();
            }
        }



    }
}
