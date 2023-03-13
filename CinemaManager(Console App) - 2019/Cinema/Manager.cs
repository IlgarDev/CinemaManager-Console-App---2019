using Cinema.Entities;
using Cinema.Logic;
using Cinema.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Cinema
{
    static class ItemPosition
    {
        static public bool Item = true;
        static public int X = 0;
        static public int Y = 0;

        static public void Reset()
        {
            X = 1;
            Y = 0;
        }
    }


    enum MyEnum
    {
        Show = 0,
        Add = 1,
        Edit = 2,
        Remove = 3,
        Moviestate = 4
    }
    /// <summary>
    /// type which includes bought ticket info
    /// </summary>
    [Serializable]
    public class Ticket_Print
    {
        /// <summary>
        /// current hall
        /// </summary>
        public Hall Hall { get; set; }
        /// <summary>
        /// current film
        /// </summary>
        public Movie Film { get; set; }
        /// <summary>
        /// current time
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// current ticket
        /// </summary>
        public Ticket Ticket { get; set; }
    }

    [Serializable]
    public class Session_Print
    {
        public Session Session { get; set; }
    }
    class Manager : IWriter
    {
        public Menuu ActiveMenu;
        public Stack<Menuu> History = new Stack<Menuu>();
        DataBase Database;

        public Manager()
        {
            Database = new DataBase();
            DeSerializeToXml("database.xml");
            List<Menuitem> SessionMenuItems = new List<Menuitem>()
            {
                 new Menuitem{Text = "Show all sassions", del = () => ShowAllSessions()},
                 new Menuitem{Text = "Add sassion", del = () => FilmSelection() },
                 new Menuitem{Text = "Back", del = () => Back() }
            };
            Menuu SessionsMenu = new Menuu { Title = "SESSIONS MENU", MenuItems = SessionMenuItems };


            //Movies
            List<Menuitem> MoviesMenuItems = new List<Menuitem>()
            {
                 new Menuitem{Text = "Show all movies", del = () => MenuMovies()},
                 new Menuitem{Text = "Add movie", del = () => AddingMovie() },
                 new Menuitem{Text = "Back", del = () => Back() }
            };
            Menuu MoviesMenu = new Menuu { Title = "MOVIES MENU", MenuItems = MoviesMenuItems };

            //Hallitem
            List<Menuitem> HallsMenuItems = new List<Menuitem>()
            {
                 new Menuitem{Text = "Show all halls", del = () => HallsMenu()},
                 new Menuitem{Text = "Add hall", del = () => AddHall() },
                 new Menuitem{Text = "Back", del = () => Back() }
            };

            Menuu HallMenu = new Menuu { Title = "HALL MENU", MenuItems = HallsMenuItems };

            //MainmenuItem
            List<Menuitem> MainMenuItems = new List<Menuitem>()
            {
                new Menuitem{Text = "Hall Menu", LinkMenu = HallMenu },
                new Menuitem{Text = "Movie Menu" , LinkMenu = MoviesMenu },
                new Menuitem{Text = "Sessions Menu", LinkMenu = SessionsMenu },
                new Menuitem{Text = "Statistic Menu" },
                new Menuitem{Text = "Exit", del = () => ExitMenu()}
            };

            Menuu MainMenu = new Menuu { Title = "MAIN MENU", MenuItems = MainMenuItems };
            ActiveMenu = MainMenu;
        }
        public void ExitMenu()
        {
            DialogResult res = MessageBox.Show("Do you want to save this data to file?", "Caption of window", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                SerializeToXml("database.xml");
                Environment.Exit(0);
            }
            else
            {
                Environment.Exit(0);
            }

        }
        public void Start()
        {
            Console.CursorVisible = false;
            ActiveMenu.Run();
            int cursorPosition_Y = ActiveMenu.MenuItems.First().Y;
            int cursorPosition_X = 0;

            while (true)
            {

                ActiveMenu.Run();
                foreach (Menuitem item in ActiveMenu.MenuItems)
                {
                    if (cursorPosition_Y == item.Y && cursorPosition_X == item.X)
                    {
                        item.isHover = true;
                        item.Draw();
                    }
                    else
                    {
                        item.isHover = false;
                        item.Draw();
                    }
                }
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {

                    case ConsoleKey.UpArrow:
                        {
                            if (cursorPosition_Y > ActiveMenu.MenuItems.First().Y)
                            {
                                cursorPosition_Y -= 3;
                            }
                            else
                            {
                                cursorPosition_Y = ActiveMenu.MenuItems.First().Y;
                            }
                            break;
                        }

                    case ConsoleKey.DownArrow:
                        {
                            if (cursorPosition_Y != ActiveMenu.MenuItems.Last().Y)
                            {
                                cursorPosition_Y += 3;
                            }
                            else cursorPosition_Y = ActiveMenu.MenuItems.Last().Y;
                            break;
                        }

                    case ConsoleKey.Enter:
                        {

                            foreach (Menuitem item in ActiveMenu.MenuItems)
                            {
                                if (item.isHover)
                                {
                                    if (item.LinkMenu != null)
                                    {
                                        cursorPosition_Y = ActiveMenu.MenuItems.First().Y;
                                        cursorPosition_X = 0;
                                        History.Push(ActiveMenu);
                                        ActiveMenu = item.LinkMenu;
                                    }
                                    else
                                    {
                                        cursorPosition_Y = ActiveMenu.MenuItems.First().Y;
                                        cursorPosition_X = 0;
                                        item.del?.Invoke();
                                    }

                                }
                            }
                        }

                        break;
                }
            }
        }




        //HALL
        #region
        private void AddHall()
        {
            Console.CursorVisible = true;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n Adding a hall");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Title:");
            string HallName = Console.ReadLine();
            Console.Write("Rows:");
            uint Rows = Convert.ToUInt32(Console.ReadLine());
            Console.Write("Seats by row:");
            uint RowsbySeats = Convert.ToUInt32(Console.ReadLine());
            Console.Write("Type ( 1) - 2D,  2) - 3D, 3) - IMAX) :");
            uint tmpType = Convert.ToUInt32(Console.ReadLine());

            string type = ((HallType)tmpType).ToString();
            char[] s = type.ToCharArray();
            Array.Reverse(s);
            type = new string(s);


            Database.Halls.Add(
                new Hall
                {
                    HallTitle = HallName,
                    Rows = Rows,
                    RowsbySeats = RowsbySeats,
                    Type = type
                });
            Console.CursorVisible = false;
            MessageBox.Show("Hall added");
        }


        private void HallsMenu()
        {
            List<Menuitem> menuItems = new List<Menuitem>();
            Menuitem back = new MenuSeparator()
            {
                Y = ItemPosition.Y += 3,

                Text = "Back",
                del = Back
            };
            menuItems.Add(back);


            foreach (Hall hall in Database.Halls)
            {
                Menuitem HallInfoMenu = new MenuSeparator
                {
                    Y = ItemPosition.Y += 3,
                    Text = hall.ToString(),
                    LinkMenu = new EditHall(hall, "HALL INFO",
                     new List<Menuitem>()
                     {
                         new MenuSeparator{Text = "Edit hall name", del = ()=> EditHallTitle(hall) },
                         new MenuSeparator{Text = "Edit Rows count", del = () => ChangeRows(hall) },
                         new MenuSeparator{Text = "Edit seats in row", del = () => ChangeRowsSeats(hall) },
                         new MenuSeparator{Text = "Edit hall type", del = () => EditHallType(hall) },
                         new MenuSeparator{Text = "Delete hall", del = () => DeleteHall(hall)},
                         new MenuSeparator{Text = "Back", del = () => Back() }
                     }


                    )
                };
                menuItems.Add(HallInfoMenu);
            }
            Menuu Halls = new Menuu { Title = "ALL HALLS", MenuItems = menuItems };
            History.Push(ActiveMenu);
            ActiveMenu = Halls;
        }


        private void EditHallTitle(Hall m)
        {
            bool Incorrect = false;
            string Filmname;

            do
            {
                Console.CursorVisible = true;
                Console.SetCursorPosition(41, 10);
                Console.Write(" : ");

                Filmname = Console.ReadLine();
                Incorrect = Regex.IsMatch(Filmname, @"^$");

                if (Incorrect)
                {
                    MessageBox.Show("Incorrect data");
                }
            }

            while (Incorrect);
            Console.CursorVisible = false;
            DialogResult res = MessageBox.Show("Change movie title??", "Change the name of the movie", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                MessageBox.Show("The name of the film has been changed to " + Filmname);
                m.HallTitle = Filmname;
            }


        }


        private void ChangeRows(Hall m)
        {
            int tmp_rows = -1;

            do
            {
                Console.CursorVisible = true;
                Console.SetCursorPosition(41, 12);
                Console.Write(" : ");
                tmp_rows = Convert.ToInt32(Console.ReadLine());

                if (tmp_rows <= 0 || tmp_rows > 50)
                {
                    MessageBox.Show("Enter the correct amount");
                }

            }
            while (tmp_rows <= 0 || tmp_rows > 50);

            DialogResult res = MessageBox.Show("Do you really want to change the number of rows??", "Changing the number of rows", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                MessageBox.Show("Row numbers changed");
                m.Rows = Convert.ToUInt32(tmp_rows);
            }
            Console.CursorVisible = false;

        }


        private void ChangeRowsSeats(Hall m)
        {
            int tmp_rows;

            do
            {
                Console.CursorVisible = true;
                Console.SetCursorPosition(41, 14);
                Console.Write(" : ");
                tmp_rows = Convert.ToInt32(Console.ReadLine());

                if (tmp_rows <= 0 || tmp_rows > 100)
                {
                    MessageBox.Show("Enter the correct amount");
                }

            }
            while (tmp_rows <= 0 || tmp_rows > 100);

            DialogResult res = MessageBox.Show("Do you really want to change the number of seats in a row?", "Changing the number of seats in a row", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                MessageBox.Show("Number of seats in the row changed");
                m.RowsbySeats = Convert.ToUInt32(tmp_rows);
            }

            Console.CursorVisible = false;


        }


        private void EditHallType(Hall m)
        {
            int tmp_Type;

            do
            {
                Console.CursorVisible = true;
                Console.SetCursorPosition(41, 16);
                Console.Write(" (1 - 2D, 2 - 3D, 3 - IMAX ): ");

                tmp_Type = Convert.ToInt32(Console.ReadLine());

                if (tmp_Type < 1 || tmp_Type > 3)
                {
                    MessageBox.Show("This Type does not exist");
                }

            }
            while (tmp_Type < 1 || tmp_Type > 3);
            Console.CursorVisible = false;

            DialogResult res = MessageBox.Show("Do you really want to change the Type of the hall?", "Change of Type", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {


                string type = ((HallType)tmp_Type).ToString();
                char[] s = type.ToCharArray();
                Array.Reverse(s);
                type = new string(s);
                MessageBox.Show("Type changed");
                m.Type = type;

            }

        }


        private void DeleteHall(Hall m)
        {
            DialogResult res = MessageBox.Show("Remove hall??", "Deleting a hall", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                Database.Halls.Remove(m);
                MessageBox.Show("Hall removed");
                Back();
                Back();
                HallsMenu();
            }

        }
        #endregion


        //MOVIE
        #region
        private void AddingMovie()
        {
            Console.CursorVisible = true;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n Adding a movie");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Movie title : ");
            string MovieTitle = Console.ReadLine();
            Console.Write(" Date of release:");
            uint Release = Convert.ToUInt32(Console.ReadLine());
            Console.Write(" Genre (1 - Comedy, 2 - Historical, 3 - Sport, 4 - Melodrama) : ");
            uint Genre = Convert.ToUInt32(Console.ReadLine());
            Console.Write(" Producer :");
            string Producer = Console.ReadLine();

            Database.Movies.Add(
                new Movie
                {
                    FilmName = MovieTitle,
                    yearOfIssue = Release,
                    Genre = ((Genres)Genre).ToString(),
                    Producer = Producer,
                    Сondition = Conditions.FUTURE.ToString()
                });
            Console.CursorVisible = false;
            MessageBox.Show("Movie added");
        }


        private void MenuMovies()
        {
            List<Menuitem> menuItems = new List<Menuitem>();
            Menuitem back = new MenuSeparator()
            {
                Y = ItemPosition.Y += 3,
                Text = "Back",
                del = Back
            };
            menuItems.Add(back);


            foreach (Movie movie in Database.Movies)
            {
                Menuitem MovieInfoMenu = new MenuSeparator
                {
                    Y = ItemPosition.Y += 3,
                    Text = movie.ToString(),
                    LinkMenu = new EditMovie(movie, "MOVIE INFO",
                     new List<Menuitem>()
                     {
                         new MenuSeparator{Text = "Edit movie name", del = ()=> EditMovieName(movie) },
                         new MenuSeparator{Text = "Chenge release date", del = () => CReleaseDate(movie) },
                         new MenuSeparator{Text = "Edit Movie Genre", del = () => EditMovieGenre(movie) },
                         new MenuSeparator{Text = "Delete movie", del = () => DeleteMovie(movie)},
                         new MenuSeparator{Text = "Back", del = () => Back() }
                     }


                    )
                };
                menuItems.Add(MovieInfoMenu);
            }
            Menuu Movies = new Menuu { Title = "ALL MOVIES", MenuItems = menuItems };
            History.Push(ActiveMenu);
            ActiveMenu = Movies;
        }


        private void EditMovieName(Movie m)
        {
            bool Incorrect = false;
            string Filmname;

            do
            {
                Console.CursorVisible = true;
                Console.SetCursorPosition(41, 10);
                Console.Write(" : ");

                Filmname = Console.ReadLine();
                Incorrect = Regex.IsMatch(Filmname, @"^$");

                if (Incorrect)
                {
                    MessageBox.Show("Incorrect data");
                }
            }

            while (Incorrect);
            Console.CursorVisible = false;
            DialogResult res = MessageBox.Show("Change movie title??", "Change the name of the movie", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                MessageBox.Show("The name of the film has been changed to " + Filmname);
                m.FilmName = Filmname;
            }

        }


        private void CReleaseDate(Movie m)
        {
            bool Incorrect = false;
            string releaseDate;
            do
            {
                Console.CursorVisible = true;
                Console.SetCursorPosition(41, 12);
                Console.Write(" : ");
                releaseDate = Console.ReadLine();


                Incorrect = Regex.IsMatch(releaseDate, @"^\d+$");
                if (Incorrect)
                {
                    if (Convert.ToUInt32(releaseDate) < 1985)
                    {
                        MessageBox.Show("No film was officially shot");
                    }
                    else if (Convert.ToUInt32(releaseDate) > 2019)
                    {
                        MessageBox.Show("Come when the " + releaseDate + " comes");
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect date");
                    CReleaseDate(m);
                }

            }
            while (Convert.ToUInt32(releaseDate) < 1895 || Convert.ToUInt32(releaseDate) > 2019);
            Console.CursorVisible = false;

            DialogResult res = MessageBox.Show("You definitely want to change the release date?", "Change the release date", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                MessageBox.Show("Release date changed");
                m.yearOfIssue = Convert.ToUInt32(releaseDate);
            }


        }


        private void EditMovieGenre(Movie m)
        {

            bool Incorrect = false;
            string tmp_Genre;

            do
            {
                Console.CursorVisible = true;
                Console.SetCursorPosition(41, 14);
                Console.Write("(1 - Comedy, 2 - Historical, 3 - Sport, 4 - Melodrama) : ");

                tmp_Genre = Console.ReadLine();
                Incorrect = Regex.IsMatch(tmp_Genre, @"^\d+$");
                if (Incorrect)
                {
                    if (Convert.ToUInt32(tmp_Genre) < 1 || Convert.ToUInt32(tmp_Genre) > 4)
                    {
                        MessageBox.Show("This genre does not exist");
                    }
                }
                else
                {
                    MessageBox.Show("This genre does not exist");
                    EditMovieGenre(m);
                }
            }
            while (Convert.ToUInt32(tmp_Genre) < 1 || Convert.ToUInt32(tmp_Genre) > 4);
            Console.CursorVisible = false;

            DialogResult res = MessageBox.Show("Do you really want to change the genre of the movie?", "Change of genre", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                MessageBox.Show("Movie removed");
                m.Genre = ((Genres)Convert.ToUInt32(tmp_Genre)).ToString();

            }
        }


        private void DeleteMovie(Movie m)
        {
            DialogResult res = MessageBox.Show("Remove movie??", "Deleting a movie", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                Database.Movies.Remove(m);
                MessageBox.Show("Movie removed");
                Back();
                Back();
                MenuMovies();
            }




        }
        #endregion


        //SESSIONS
        #region
        private void ShowAllSessions()
        {
            List<Menuitem> menuItems = new List<Menuitem>();
            Menuitem back = new MenuSeparator()
            {
                Y = ItemPosition.Y += 3,
                Text = "Back",
                del = () => Back()
            };
            menuItems.Add(back);
            foreach (Session s in Database.Sessions)
            {
                Menuitem sessions = new MenuSeparator
                {
                    Y = ItemPosition.Y += 3,
                    Text = s.ToString(),
                    LinkMenu = new SessionOperations(s, "SESSION",
                    new List<Menuitem>
                    {
                         new MenuSeparator{Text = "Sale ticket", del = ()=> SaleTicket(s) },
                         new MenuSeparator{Text = "Remove sassion", del = ()=> RemoveSession(s) },
                         new MenuSeparator{Text = "Back", del = () => Back()}
                    })
                };

                menuItems.Add(sessions);
            };
            Menuu Ses = new Menuu { Title = "ALL SESSIONS", MenuItems = menuItems };
            History.Push(ActiveMenu);
            ActiveMenu = Ses;
        }


        private void RemoveSession(Session s)
        {
            DialogResult res = MessageBox.Show("Remove session?", "Deleting a session", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                if (Database.Sessions.Remove(s))
                {
                    MessageBox.Show("Session removed");
                    s.movie.Сondition = Conditions.PAST.ToString();  
                }
                Back();
                Back();
                ShowAllSessions();
            }
            else History.Peek();
        }



        private void SaleTicket(Session ses)
        {
            Console.Clear();
            Console.WriteLine(ses.ToString() + "\n\n");

            int x = 1, y = 3;
            char Ok = ' ';

            for (int i = 0; i < ses.hall.RowsbySeats; i++)
            {
                Console.SetCursorPosition(x += 4, y);
                Console.Write(i + 1);
            }

            x = 0; y = 2;

            for (int i = 0; i < ses.hall.Rows; i++)
            {
                Console.SetCursorPosition(x, y += 3);
                Console.Write(i + 1);
            }

            int my_x = 1, my_y = 1;
            var key = System.ConsoleKey.A;

            while (true)
            {
                x = 0; y = 4;

                for (int i = 0; i < ses.hall.Rows; i++)
                {
                    for (int j = 0; j < ses.hall.RowsbySeats; j++)
                    {
                        x += 4;
                        foreach (var one in ses.tickets)
                        {
                            if (one.Seat == j + 1 && one.Row == i + 1)
                            {
                                Ok = '#'; break;
                            }
                        }
                        if (my_x == j + 1 && my_y == i + 1) Ok = '*';
                        DrawEachHall(x, y, Ok);
                        Ok = ' ';
                    }
                    x = 0; y += 3;
                }

                key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        if (my_x - 1 >= 1) --my_x;
                        break;
                    case ConsoleKey.UpArrow:
                        if (my_y - 1 >= 1) --my_y;
                        break;

                    case ConsoleKey.RightArrow:
                        if (my_x + 1 <= ses.hall.RowsbySeats) ++my_x;
                        break;


                    case ConsoleKey.DownArrow:
                        if (my_y + 1 <= ses.hall.Rows) ++my_y;
                        break;

                    case ConsoleKey.Enter:
                        bool chk = false;
                        foreach (var one in ses.tickets)
                        {
                            if (my_x == one.Seat && my_y == one.Row)
                            {
                                chk = true; break;
                            }
                        }
                        if (!chk)
                        {
                            ses.tickets.Add(new Ticket { Row = my_y, Seat = my_x });

                            DialogResult res = MessageBox.Show($"Ticket (row {my_y} seat {my_x}) Sold.\nDo you want to print it?", "Ticket", MessageBoxButtons.YesNo);
                            if (res == DialogResult.Yes)
                            {
                                Ticket_Print for_print = new Ticket_Print { Film = ses.movie, Hall = ses.hall, Time = ses.dateTime, Ticket = ses.tickets.Last() };

                                string path = $@"..\..\{ses.movie.FilmName} Row{my_y} seat{my_x}.xml";
                                XmlSerializer xs = new XmlSerializer(typeof(Ticket_Print));

                                using (FileStream fs = new FileStream(path, FileMode.Create))
                                {
                                    xs.Serialize(fs, for_print);
                                }
                                MessageBox.Show("Printed");
                            }
                            return;
                        }
                        break;
                }

            }

        }

        private void DrawEachHall(int X, int Y, char symbol)
        {
            int Height = 3;
            int Width = 4;
            for (int j = 0; j < Height; ++j)
            {
                for (int i = 0; i < Width; ++i)
                {
                    Console.SetCursorPosition(X + i, Y + j);
                    if (i == 0 && j == 0) Console.Write('┌');

                    else if (i == 0 && j == Height - 1) Console.Write('└');

                    else if (i == Width - 1 && j == 0) Console.Write('┐');

                    else if (i == Width - 1 && j == Height - 1) Console.WriteLine('┘');

                    else if (i == 0 || i == Width - 1) Console.Write('│');

                    else if (j == 0 || j == Height - 1) Console.Write('─');

                    else if (i == 1 || i == Width - 2)
                    {
                        if (symbol == '#') Console.Write("##");
                        else if (symbol == '*') Console.Write("**");
                        else if (symbol == ' ') Console.Write("  ");
                    }

                }
            }

        }



        private void FilmSelection()
        {
            List<Menuitem> menuItems = new List<Menuitem>();
            Menuitem back = new MenuSeparator()
            {
                Y = ItemPosition.Y += 3,
                Text = "Back",
                del = Back
            };
            menuItems.Add(back);

            foreach (Movie movie in Database.Movies)
            {
                Menuitem movies = new MenuSeparator
                {
                    Y = ItemPosition.Y += 3,
                    Text = movie.ToString(),
                    del = () => HallSelection(movie)
                };

                menuItems.Add(movies);
            };
            Menuu halls = new Menuu { Title = "SELECT MOVIES", MenuItems = menuItems };
            History.Push(ActiveMenu);
            ActiveMenu = halls;
        }


        private void HallSelection(Movie movie)
        {
            List<Menuitem> menuItems = new List<Menuitem>();
            Menuitem back = new MenuSeparator()
            {
                Y = ItemPosition.Y += 3,
                Text = "Back",
                del = Back
            };
            menuItems.Add(back);

            foreach (Hall hall in Database.Halls)
            {
                Menuitem halls = new MenuSeparator
                {
                    Y = ItemPosition.Y += 3,
                    Text = hall.ToString(),
                    del = () => Database.Sessions.Add(new Session()
                    {
                        movie = movie,
                        hall = hall,
                        dateTime = DateTime.Now,
                        
                    })

                };
                menuItems.Add(halls);
                foreach (Menuitem item in menuItems)
                {
                    item.del += () => movie.Сondition = Conditions.CURRENT.ToString();
                    item.del += Back;
                    item.del += Back;
                    item.del += () => MessageBox.Show("Session Created");
                }
            };
            Menuu Movies = new Menuu { Title = "SELECT HALL", MenuItems = menuItems };
            History.Push(ActiveMenu);
            ActiveMenu = Movies;
        }
        #endregion

        public void Back()
        {

            ActiveMenu = History.Pop();
        }

        public void SerializeToXml(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(DataBase));

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                xs.Serialize(fs, Database);
            }
        }
        public void DeSerializeToXml(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataBase));

            using (Stream reader = new FileStream(path, FileMode.OpenOrCreate))
            {
                FileInfo sizecheck = new FileInfo(path);
                if (sizecheck.Length > 0)
                    Database = (DataBase)serializer.Deserialize(reader);
            }
        }
    }

}

