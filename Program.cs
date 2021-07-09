using Project_Dvd.Mappers;
using Project_Dvd.Menu;
using Project_Dvd.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Project_Dvd
{
    class Program
    {
        static void Main(string[] args)
        {
            // login in interface 
            //string username, password = string.Empty;
            //Console.WriteLine("enter username");
            //username = Console.ReadLine();
            //Console.WriteLine("enter password");
            //password = Console.ReadLine();
            //using (StreamWriter sw = new StreamWriter(File.Create("D:\\psword.txt")))
            //{
            //    sw.WriteLine(username);
            //    sw.WriteLine(password);
            //    sw.Close();
            //}
            ConsoleMenuElement[] menuElements = {
                new ConsoleMenuElement("List of all movies", Option1),
                new ConsoleMenuElement("Add new copies", Option2),
                new ConsoleMenuElement("About clients", Option3),
                new ConsoleMenuElement("Register new rentals", Option4),
                new ConsoleMenuElement("Copy return", Option5),
                new ConsoleMenuElement("Statistics" , Option6),
                new ConsoleMenuElement("Overdue rentals", Option7),
                new ConsoleMenuElement("Creat new user", Option8),
                new ConsoleMenuElement("Exit", ExitProgram),
            };

            ConsoleMenu consoleMenu = new ConsoleMenu("Choose an option:", menuElements);
            consoleMenu.RunMenu();
            
        }
        public static void Option1()
        {
            Console.Clear();
            Console.WriteLine("List of all movies");
            MovieMapper mapper1 = new MovieMapper();
            Console.WriteLine(mapper1.ToString());
            Console.ReadLine();
            Console.Clear();
        }
        public static void Option2()
        {
            string title;
            int year, price;
            MovieMapper newmovie = new MovieMapper();
            do
            {
                Console.Clear();
                Console.WriteLine("Write name of new movie: ");
                title = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(title));

            do
            {
                Console.Clear();
                Console.Write("Write year of this movie: ");
            } while (!int.TryParse(Console.ReadLine(), out year) || year < 1887 || year > DateTime.Today.Year);

            do
            {
                Console.Clear();
                Console.Write("Write price of this movie: ");
            } while (!int.TryParse(Console.ReadLine(), out price) || price < 0);

     

            var movie = new Movies(title, year, newmovie.GetNextId(), price);
            newmovie.SaveNewAndAddCopy(movie);
        }

    
        public static void Option3()
        {
            Console.Clear();
            Console.WriteLine("About clients");
            Console.WriteLine("Enter client id");
            int clientid = int.Parse(Console.ReadLine());
            ClientMapper mapper3 = new ClientMapper();
            Console.WriteLine(mapper3.Clienthistoric(clientid));
            Console.ReadLine();
            Console.Clear();
        }
        public static void Option4()
        {

            int client_id;
            int copy_id;
            RentalMappe rm = new RentalMappe();
            ClientMapper clm = new ClientMapper();
            CopiesMapper cpm = new CopiesMapper();
            MovieMapper mm = new MovieMapper();

            int lastClient_id = clm.GetLastId();
            do
            {
                Console.Clear();
                Console.WriteLine("\n" + clm.ToString());
                Console.SetCursorPosition(0, 0);
                Console.Write("Client id: ");
            } while (!int.TryParse(Console.ReadLine(), out client_id) || client_id <= 0 || client_id > lastClient_id);

            var AvailableCopies = cpm.GetAvailableCopies();
            string copiesList = "";
            List<int> ids = new List<int>();

            foreach (var copies in AvailableCopies)
            {
                Movies movie = mm.GetByCopyId(copies.Copy_id);
                copiesList += $"{copies.Copy_id}: {movie.Title}, price: {movie.Title}\n";
                ids.Add(copies.Copy_id);
            }

            do
            {
                Console.Clear();
                Console.WriteLine("\n" + copiesList);
                Console.SetCursorPosition(0, 0);
                Console.Write("Select copy id: ");
            } while (!int.TryParse(Console.ReadLine(), out copy_id) || !ids.Contains(copy_id));


            var copy = cpm.GetById(copy_id);
            copy.updateAvailability(false);
            cpm.Save(copy);
            rm.Save(new Rentals(rm.GetNextId(), copy_id, client_id, DateTime.Now, null));
        }
        public static void Option5()
        {
            int copy_id;
            ClientMapper clm = new ClientMapper();
            CopiesMapper cpm = new CopiesMapper();
            MovieMapper mm = new MovieMapper();
            RentalMappe rm = new RentalMappe();

            List<Copies> unreternedCopies = cpm.GetUnreturnedCopies();
            List<int> copyIds = new List<int>();

            if (unreternedCopies.Count != 0)
            {
                do
                {
                    Console.Clear();
                    foreach (var copy in unreternedCopies)
                    {
                        Console.WriteLine($"\nCopy {copy.Copy_id}: {mm.GetByCopyId(copy.Copy_id).Title}");
                        Console.SetCursorPosition(0, 0);
                        Console.Write("Write copy id that was returned: ");
                        copyIds.Add(copy.Copy_id);
                    }
                } while (!int.TryParse(Console.ReadLine(), out copy_id) || !copyIds.Contains(copy_id));

                Rentals rental = rm.GetByCopyId(copy_id);

                rental.SetReturned();
                rm.Save(rental);
            }
        }
        public static void Option6()
        {
            var mm = new MovieMapper();
            var rm = new RentalMappe();
            Console.Clear();
            Console.WriteLine("Statistics");
            Console.WriteLine(rm.GetStatistics() + "\n\n" + mm.GetStatistics());
            Console.ReadLine();

        }
        public static void Option7()
        {
            Console.Clear();
            Console.WriteLine("Overdue rentals");
            var rent = new RentalMappe();
            List<Rentals> rentals = rent.GetOverdueRentals();

            foreach (var rental in rentals)
            {
                Console.WriteLine($"Rental {rental.RentalId}: Copy {rental.CopyId} was rented on {rental.DateOfRental} and overdue by {rent.GetOverdueInDays(rental.RentalId)} days.");
            }
            Console.ReadLine();
        }
        public static void Option8()
        {
            string username;
            string password;
            var newuser = new UserMapper();
            Console.Clear();
            Console.WriteLine("Creat new user\n");
            Console.WriteLine("Enter username");
            username = Console.ReadLine();
            Console.WriteLine("Enter password");
            password = Console.ReadLine();
            var user = new User(username, password);
            newuser.NewUser(user);
        }
        public static void ExitProgram()
        {
            Console.WriteLine("Thank you for using my program. Now we will exit.");
            Environment.Exit(0);
        }
    }
}

