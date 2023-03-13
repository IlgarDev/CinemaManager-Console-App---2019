using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.UI
{
    public class Menuitem
    {
        public Menuu LinkMenu = null;
        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; } = 3;
        public int Width { get; set; } = 20;
        public Action del { get; set; } = null;
        public bool isHover;

        public virtual void Draw()
        {
            if (isHover)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.Blue;
            }
            for (int j = 0; j < Height; ++j)
            {
                for (int i = 0; i < Width; ++i)
                {
                    Console.SetCursorPosition(X + i + 1, Y + j);
                    if (i == 0 && j == 0)
                    {
                        Console.Write('╔');
                    }
                    else if (i == 0 && j == Height - 1)
                    {
                        Console.Write('╚');
                    }
                    else if (i == Width - 1 && j == 0)
                    {
                        Console.Write('╗');
                    }
                    else if (i == Width - 1 && j == Height - 1)
                    {
                        Console.Write('╝');
                    }
                    else if (i == 0 || i == Width - 1)
                    {
                        Console.Write('║');
                    }
                    else if (j == 0 || j == Height - 1)
                    {
                        Console.Write('═');
                    }
                    else if (i == 1 || i >= Text.Length + 2)
                    {
                        Console.Write(' ');
                    }
                    else if (i == 1 || i < Text.Length + 2)
                    {
                        Console.Write(Text[i - 2]);
                    }
                }
                Console.WriteLine("");

            }
            Console.ResetColor();

        }

    }
}
