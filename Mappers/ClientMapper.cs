using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using Project_Dvd.Models;
using static Project_Dvd.Mappers.IMapper;

namespace Project_Dvd.Mappers
{
    class ClientMapper : IMapper<Client>
    {
        private readonly string connection_string = "Server=127.0.0.1;User Id=postgres;Password=pwd;Database=rental;";


        public Client GetByID(int id)
        {
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM movies WHERE client_id = @id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string firstName = (string)reader["first_name"];
                        string lastName = (string)reader["last_name"];
                        DateTime birthday = (DateTime)reader["birthday"];
                        return new Client(id, firstName, lastName, birthday);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }

        public void Save(Client t)
        {
            throw new NotImplementedException();
        }
        public void Delete(Client t)
        {
            throw new NotImplementedException();
        }


        public string Clienthistoric(int id)
        {
            string output = "";
            using (NpgsqlConnection conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var command1 = new NpgsqlCommand("Select c.client_id,rental_id,first_name,last_name,birthday,case when available = false then 'Active rental' else 'returned rental' end as availability from clients c join rentals r on r.client_id = c.client_id join copies o on o.copy_id = r.copy_id where c.client_id = @id", conn))
                {
                    command1.Parameters.AddWithValue("@id", id);

                    NpgsqlDataReader reader = command1.ExecuteReader();

                    while (reader.Read())
                    {
                        
                        output += $"|rental id: {reader["rental_id"]}| firstname: {reader["first_name"]}|lastname: {reader["last_name"]}| birthday: {reader["birthday"]}| rental history: {reader["availability"]}|\n";
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
                using (var cmd = new NpgsqlCommand("SELECT MAX(client_id) FROM clients", conn))
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
        public override string ToString()
        {
            string result = "";
            using (var conn = new NpgsqlConnection(connection_string))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM clients", conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result += $"{reader["client_id"]}: {reader["first_name"]} {reader["last_name"]}\n";
                    }
                    return result;
                }
            }
        }
    }
}