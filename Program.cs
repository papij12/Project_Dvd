using Project_Dvd.Mappers;
using Project_Dvd.Menu;
using System;

namespace Project_Dvd
{
    class Program
    {
        static void Main(string[] args)
        {
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
            Console.WriteLine("List of all movies");
            Console.WriteLine("Enter the id of the movie");
            int id = int.Parse(Console.ReadLine());
            var mapper1 = new MovieMapper();
            var movie1 = mapper1.GetById(id);
            Console.WriteLine(movie1);
        }
        public static void Option2()
        {
            Console.WriteLine("Add new copies");
        }
        public static void Option3()
        {
            Console.WriteLine("About clients");
        }
        public static void Option4()
        {
            Console.WriteLine("Register new rentals");
        }
        public static void Option5()
        {
            Console.WriteLine("Copy return");
        }
        public static void Option6()
        {
            Console.WriteLine("Statistics");
        }
        public static void Option7()
        {
            Console.WriteLine("Overdue rentals");
        }
        public static void Option8()
        {
            Console.WriteLine("Creat new user");
        }
        public static void ExitProgram()
        {
            Console.WriteLine("Thank you for using my program. Now we will exit.");
            Environment.Exit(0);
        }
    }
}

