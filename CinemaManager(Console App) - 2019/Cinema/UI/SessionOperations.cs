using Cinema.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.UI
{
    public class SessionOperations : Menuu
    {
        public Session session;

        public SessionOperations(Session session, string title, List<Menuitem> MenuItems)
        {
            Title = title;
            this.MenuItems = MenuItems;
            this.session = session;
        }

        public override void Run()
        {
            ItemPosition.Reset();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  "+ Title + " : " + session.ToString());
            foreach (Menuitem item in MenuItems)
            {
                item.Y = ItemPosition.Y += 3;
                item.Draw();
            }
        }
    }
}
