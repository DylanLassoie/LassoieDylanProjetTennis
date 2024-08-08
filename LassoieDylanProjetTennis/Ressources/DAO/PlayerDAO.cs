using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal class PlayerDAO : IPlayerDAO
    {
        private readonly string _connectionString;

        public PlayerDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Player player)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Player (FirstName, LastName, Rank) VALUES (@FirstName, @LastName, @Rank)", connection);
                command.Parameters.AddWithValue("@FirstName", player.FirstName);
                command.Parameters.AddWithValue("@LastName", player.LastName);
                command.Parameters.AddWithValue("@Rank", player.Rank);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Player Get(string firstName, string lastName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Player WHERE FirstName = @FirstName AND LastName = @LastName", connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Player
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Rank = Convert.ToInt32(reader["Rank"])
                        };
                    }
                }
            }
            return null;
        }

        public void Update(Player player)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Player SET Rank = @Rank WHERE FirstName = @FirstName AND LastName = @LastName", connection);
                command.Parameters.AddWithValue("@FirstName", player.FirstName);
                command.Parameters.AddWithValue("@LastName", player.LastName);
                command.Parameters.AddWithValue("@Rank", player.Rank);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(string firstName, string lastName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Player WHERE FirstName = @FirstName AND LastName = @LastName", connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Player> GetAll()
        {
            var players = new List<Player>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Player", connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        players.Add(new Player
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Rank = Convert.ToInt32(reader["Rank"])
                        });
                    }
                }
            }
            return players;
        }
    }
}
