using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Dvd.Models
{
    class Client
    {
        public int ClientId { get; private set; }
        public int RentalId { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }
        public string Available { get; set; }

        public Client(int Id, string firstname,  string lastname, DateTime birthday)
        {
            ClientId = Id;
            FirstName = firstname;
            LastName = lastname;
            Date = birthday;
           
            //Available = availability;
        }
        public override string ToString()
        {
            return $"client : {ClientId} | rental-id : {RentalId} | Name : {FirstName} | Last name : {LastName} | date of birth : {Date} | history : {Available} ";
        }
    }
}
