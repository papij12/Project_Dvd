using Npgsql;
using Project_Dvd.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static Project_Dvd.Mappers.IMapper;

namespace Project_Dvd.Mappers
{
    class MovieMapper : IMapper<Movies>
    {
        private readonly string connection_string = "Server=127.0.0.1;User Id=postgres;Password=pwd;Database=rental;";


        public Movies GetByID(int id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM movies WHERE movie_id = @id ", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string title = (string)reader["title"];

                        int year = (int)reader["year"];

                       // int id = (int)reader["movie_id"];

                        float price = (float)reader["price"];

                        return new Movies(title, year, id, price);
                    }
                    return null;
                }
            }

        }

        public void Save(Movies t)
        {
            throw new NotImplementedException();
        }
        public void Delete(Movies t)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            string output = "";
            using (NpgsqlConnection conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("Select title,year, m.movie_id, price, count (copy_id) as totalcount from movies m join copies c On c.movie_id = m.movie_id group by title,year, m.movie_id, price; ", conn))
                {


                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        output += $" |{reader["title"]}| produced in {reader["year"]}| movie-Id {reader["movie_id"]}|  cost {reader["price"]}| Numberofcopies {reader["totalcount"]}|\n";
                    }

                    return output;
                }
                
            }  

        }
        public int GetLastId()
        {
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT MAX(movie_id) FROM movies", conn))
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
            return GetLastId() +1;
        }
        public void SaveNewAndAddCopy(Movies movie)
        {
            var cm = new CopiesMapper();
            var title = movie.Title;
            var year = movie.YearOfProduction;
            var id = movie.MovieId;
            var price = movie.PriceOfMovie;

            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();
                var cmd = new NpgsqlCommand();
                cmd.Transaction = transaction;
                try
                {
                    cmd = new NpgsqlCommand("INSERT INTO movies (title, year, movie_id, price)" +
                                            "VALUES (@title, @year, @id, @price)", conn);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@price", price);

                    cmd.ExecuteNonQuery();

                    cmd = new NpgsqlCommand("INSERT INTO copies (copy_id, available, movie_id)" +
                                            "VALUES (@copy_id, @available, @movie_id)", conn);
                    cmd.Parameters.AddWithValue("@copy_id", cm.GetNextId());
                    cmd.Parameters.AddWithValue("@available", true);
                    cmd.Parameters.AddWithValue("@movie_id", GetNextId());

                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine(e.ToString());
                }
            }
        }
        public string GetStatistics()
        {
            string stats = "";
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT movies.movie_id, title, SUM(price), COUNT(rental_id) FROM rentals " +
                                                   "JOIN copies ON copies.copy_id = rentals.copy_id " +
                                                   "JOIN movies ON movies.movie_id = copies.movie_id " +
                                                   "JOIN clients ON clients.client_id = rentals.client_id GROUP BY movies.movie_id, title " +
                                                   "ORDER BY movies.movie_id", conn))
                {
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        double sum = (Single)reader["sum"];
                        stats += $"\nID {reader["movie_id"]}: {reader["title"]}; Rentals: {reader["count"]}, Money earned: {sum.ToString()}";
                    }
                }
            }

            return $"Total movies: {GetLastId()}. Those which were rented:" + stats;
        }
        public Movies GetByCopyId(int id)
        {
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM movies " +
                                                   "JOIN copies ON copies.movie_id = movies.movie_id " +
                                                   "WHERE copy_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string title = (string)reader["title"];
                        int year = (int)reader["year"];
                        int ageRestriction = (int)reader["age_restriction"];
                        double price = Convert.ToDouble(reader["price"]);
                        return new Movies( title, year, id, price);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
