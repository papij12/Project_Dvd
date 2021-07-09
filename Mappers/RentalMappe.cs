using Npgsql;
using Project_Dvd.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static Project_Dvd.Mappers.IMapper;

namespace Project_Dvd.Mappers
{
    class RentalMappe : IMapper<Rentals>
    {
        private string connection_string = "Server=127.0.0.1;User Id=postgres;Password=pwd;Database=rental;";
        public Rentals GetByID(int id)
        {
            throw new NotImplementedException();
        }
        public void Delete(Rentals Rentals)
        {
            throw new NotImplementedException();
        }
        public void Save(Rentals Rentals)
        {
            var rental_id = Rentals.RentalId;
            var copy_id = Rentals.CopyId;
            var client_id = Rentals.ClientId;
            var date_of_rental = Rentals.DateOfRental;
            var date_of_return = Rentals.DateOfReturn;

            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO rentals (rental_id, copy_id, client_id, date_of_rental, date_of_return)" +
                                                   "VALUES (@rental_id, @copy_id, @client_id, @date_of_rental, @date_of_return)" +
                                                   "ON CONFLICT (rental_id) DO UPDATE " +
                                                   "SET copy_id = @copy_id, client_id = @client_id, date_of_rental = @date_of_rental, date_of_return = @date_of_return", conn))
                {
                    cmd.Parameters.AddWithValue("@rental_id", rental_id);
                    cmd.Parameters.AddWithValue("@copy_id", copy_id);
                    cmd.Parameters.AddWithValue("@client_id", client_id);
                    cmd.Parameters.AddWithValue("@date_of_rental", date_of_rental);
                    cmd.Parameters.AddWithValue("@date_of_return", date_of_return);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Rentals> GetOverdueRentals()
        {
            List<Rentals> rentals = new List<Rentals>();
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT rental_id FROM rentals " +
                                                   "WHERE date_of_return IS null AND (NOW() - date_of_rental) > '14 day'", conn))
                {
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int rental_id = (int)reader["rental_id"];
                        rentals.Add(GetByID(rental_id));
                    }
                }
            }

            return rentals;
        }
        public int GetNumberOfRentalsByClientId(int id)
        {
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT COUNT(rental_id) FROM rentals " +
                                                   "WHERE client_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return (int)(long)reader["count"];
                    }
                    return 0;
                }

            }
        }
        public string GetStatistics()
        {
            string stats = "";
            string total;
            double totalsum = 0;
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT clients.client_id, first_name, last_name, SUM(price) FROM rentals " +
                                                   "JOIN copies ON copies.copy_id = rentals.copy_id " +
                                                   "JOIN movies ON movies.movie_id = copies.movie_id " +
                                                   "JOIN clients ON clients.client_id = rentals.client_id " +
                                                   "GROUP BY clients.client_id, first_name, last_name " +
                                                   "ORDER BY clients.client_id", conn))
                {
                    var reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        double sum = (Single)reader["sum"];
                        totalsum += sum;
                        stats += $"\nID {reader["client_id"]}: {reader["first_name"]} {reader["last_name"]}; " +
                            $"Money spent: {sum.ToString()}, avg per rental: " +
                            $"{String.Format("{0:0.00}", sum / GetNumberOfRentalsByClientId((int)reader["client_id"]))}";
                    }
                    total = $"Total rentals: {GetLastId()}, total money spent: {totalsum}.";
                }
            }

            return total + stats;
        }
        public int GetLastId()
        {
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT MAX(rental_id) FROM rentals", conn))
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
        public Rentals GetByCopyId(int id)
        {
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM rentals " +
                                                   "JOIN copies ON copies.copy_id = rentals.copy_id " +
                                                   "WHERE rentals.copy_id = @id AND date_of_return IS null", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int rental_id = (int)reader["rental_id"];
                        int copy_id = (int)reader["copy_id"];
                        int client_id = (int)reader["client_id"];
                        DateTime date_of_rental = (DateTime)reader["date_of_rental"];
                        var date_of_return = reader["date_of_return"] as DateTime?;
                        return new Rentals(rental_id, copy_id, client_id, date_of_rental, date_of_return);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public int GetOverdueInDays(int id)
        {
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT (NOW() - date_of_rental) AS overdue " +
                                                   "FROM rentals WHERE rental_id = @id ", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        var overdue = (TimeSpan)reader["overdue"];
                        return overdue.Days;
                    }
                    return 0;
                }

            }
        }
    }
}
