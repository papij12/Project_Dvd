using System;

namespace Project_Dvd
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Welcome to the database");
            Console.WriteLine("what would you like to do?");
            Console.WriteLine("--------options--------");
            Console.WriteLine("1. List of all movies");
            Console.WriteLine("2. Add new copies");
            Console.WriteLine("3. About clients");
            Console.WriteLine("4. Register new rentals");
            Console.WriteLine("5. Copy return");
            Console.WriteLine("6. Statistics");
            Console.WriteLine("7. Overdue rentals");
            Console.WriteLine("8. Creat new user");
            Console.WriteLine("------------------------");
            int option =int.Parse(Console.ReadLine());
            switch (option)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("Enter the id of the movie");
                    int id = int.Parse(Console.ReadLine());
                    var mapper1 = new MovieMapper();
                    var movie1 = mapper1.GetById(id);
                    Console.WriteLine(movie1);
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("2. database under constaction");
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("3. database under constaction");
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("4. database under constaction");
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("5. database under constaction");
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("6. database under constaction");
                    break;
                case 7:
                    Console.Clear();
                    Console.WriteLine("7. database under constaction");
                    break;
                case 8:
                    Console.Clear();
                    Console.WriteLine("8. database under constaction");
                    break;

            }
        }
    }
}
