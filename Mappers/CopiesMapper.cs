using Npgsql;
using Project_Dvd.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Dvd.Mappers
{
    class CopiesMapper
    {
        private readonly string connection_string = "Server=127.0.0.1;User Id=postgres;Password=pwd;Database=rental;";
        public Copies GetById(int copy_id)
        {
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM copies WHERE copy_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", copy_id);
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int movie_id = (int)reader["movie_id"];
                        bool available = (bool)reader["available"];
                        return new Copies(copy_id, movie_id, available);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public int GetLastId()
        {

            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT MAX(copy_id) FROM copies", conn))
                {
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return (int)reader["max"];
                    }
                    return 0;
                }

            }
        }

        public int GetNextId()
        {
            return GetLastId() + 1;
        }
        public List<Copies> GetAvailableCopies()
        {
            List<Copies> copies = new List<Copies>();
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM copies " +
                                                 "WHERE available = true " +
                                                 "ORDER BY copy_id", conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        copies.Add(new Copies((int)reader["copy_id"], (int)reader["movie_id"], true));
                    }
                }
            }

            return copies;
        }
        public void Save(Copies Copies)
        {
            var copy_id = Copies.Copy_id;
            var movie_id = Copies.Movie_id;
            var available = Copies.Available;

            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO copies (copy_id, available, movie_id) " +
                                                   "VALUES (@copy_id, @available, @movie_id) " +
                                                   "ON CONFLICT (copy_id) DO UPDATE " +
                                                   "SET available = @available, movie_id = @movie_id", conn))
                {
                    cmd.Parameters.AddWithValue("@copy_id", copy_id);
                    cmd.Parameters.AddWithValue("@available", available);
                    cmd.Parameters.AddWithValue("@movie_id", movie_id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Copies> GetUnreturnedCopies()
        {
            List<Copies> copies = new List<Copies>();
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM rentals " +
                                                   "JOIN copies ON copies.copy_id = rentals.copy_id " +
                                                   "WHERE date_of_return IS null " +
                                                   "ORDER BY date_of_rental", conn))
                {
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int copy_id = (int)reader["copy_id"];
                        int movie_id = (int)reader["movie_id"];
                        copies.Add(new Copies(copy_id, movie_id, false));
                    }
                }
            }

            return copies;
        }

    }
}
