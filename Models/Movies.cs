using Project_Dvd.Models;
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
       
        
        public Movies(string title, int year, int id, double price)
        {
            Title = title;
            YearOfProduction = year;
            MovieId = id;
            PriceOfMovie = price;
         
        }
        public override string ToString()
        {
                return ($" {Title} produced in {YearOfProduction} movie {MovieId}  cost {PriceOfMovie}");  
              
        }
    }
}
