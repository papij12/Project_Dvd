using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Dvd.Models
{
    class Rentals
    {
        public int RentalId { get; set; }
        public int CopyId { get; set; }
        public int ClientId { get; set; }
        public DateTime DateOfRental { get; set;}
        public DateTime? DateOfReturn { get; set; }
        public Rentals(int id, int copyid, int clientid, DateTime date_of_rental, DateTime? date_of_return)
        {
            RentalId = id;
            CopyId = copyid;
            ClientId = clientid;
            DateOfRental = date_of_rental;
            DateOfReturn = date_of_return;
        }
        
        public void SetReturned()
        {
            DateOfReturn = DateTime.Now;
        }
        public override string ToString()
        {
            return $"Rent {RentalId}: , rented copy {CopyId} by customer {ClientId} at {DateOfRental}, returned at {DateOfReturn}";
        }
    }
}
