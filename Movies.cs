using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Dvd
{
    class Movies
    {
        public string Title { get;  set; }
        public int YearOfProduction { get; set; }
       
        public int MovieId { get; set; }
        public double PriceOfMovie { get; set; }
      
        public Movies(string title, int year, int id, double price)
        {
            Title = title;
            YearOfProduction = year;
            
            MovieId = id;
            PriceOfMovie = price;
        }
        public override string ToString()
        {
            return $"movie {MovieId}: {Title} produced in {YearOfProduction}  cost {PriceOfMovie}";
        }

    }
}
