using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace Project_Dvd
{
    class MovieMapper
    {
        private string connection_string = "Server=127.0.0.1;User Id=postgres;Password=pwd;Database=rental;";
        public Movies GetById (int id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("Select * from movies where movie_id = @id ", conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string title = (string)reader["title"];
                        int year = (int)reader["year"];
                    
                        double price = Convert.ToDouble(reader["price"]);

                        return new Movies(title, year, id, price);
                    }
                    else { return null; }
                }
            }
        }
    }
}
