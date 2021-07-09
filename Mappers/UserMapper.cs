using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using Project_Dvd.Models;

namespace Project_Dvd.Mappers
{
    class UserMapper
    {
        private readonly string connection_string = "Server=127.0.0.1;User Id=postgres;Password=pwd;Database=rental;";

        public void NewUser (User User)
        {

            var userName = User.UserName;
            var password = User.Password;
            using (NpgsqlConnection conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("CREATE USER @username WITH ENCRYPTED PASSWORD '@password'"))
                {
                    command.Parameters.AddWithValue("@username", userName);
                    command.Parameters.AddWithValue("@passwod", password);
                    command.ExecuteNonQuery();
                }

            }
        }
    }
}
