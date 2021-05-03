using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Dvd.Models
{
    class Movies
    {
        public string Title { get; set; }
        public int YearOfProduction { get; set; }
        public int MovieId { get; set; }
        public double PriceOfMovie { get; set; }
        public int NumberOfCopies { get; set; }
        public Movies(string title, int year, int id, double price, int numberofcopies)
        {
            Title = title;
            YearOfProduction = year;
            MovieId = id;
            PriceOfMovie = price;
            NumberOfCopies = numberofcopies;
        }
        public override string ToString()
        {
            return $"movie {MovieId}: {Title} produced in {YearOfProduction}  cost {PriceOfMovie} Numberofcopies {NumberOfCopies}";
        }
    }
}
