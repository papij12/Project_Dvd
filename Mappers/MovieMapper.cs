using Npgsql;
using Project_Dvd.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Dvd.Mappers
{
    class MovieMapper
    {
        private string connection_string = "Server=127.0.0.1;User Id=postgres;Password=pwd;Database=rental;";
        public Movies GetById(int id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("Select title,year, m.movie_id, price, count (copy_id) totalcount from movies m join copies c On c.movie_id = m.movie_id where m.movie_id = @id group by title,year, m.movie_id, price; ", conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string title = (string)reader["title"];

                        int year = (int)reader["year"];

                        double price = Convert.ToDouble(reader["price"]);

                        int Numberofcopies = Convert.ToInt32(reader["totalcount"]);

                        return new Movies(title, year, id, price, Numberofcopies);
                    }
                    else { return null; }
                }
            }
        }
    }
}
