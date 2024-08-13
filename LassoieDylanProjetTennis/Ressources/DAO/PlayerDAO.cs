using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal class PlayerDAO : DAO<Player>
    {
        public PlayerDAO() : base()
        {
            //
        }

        public override bool Create(Player player)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "INSERT INTO Players (FirstName, LastName, Nationality, GenderType, Rank) " +
                                   "VALUES (@FirstName, @LastName, @Nationality, @GenderType, @Rank)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@FirstName", player.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", player.LastName);
                    cmd.Parameters.AddWithValue("@Nationality", player.Nationality);
                    cmd.Parameters.AddWithValue("@GenderType", player.GenderType.ToString());
                    cmd.Parameters.AddWithValue("@Rank", player.Rank);

                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
        }

        public override bool Delete(Player player)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "DELETE FROM Players WHERE FirstName = @FirstName AND LastName = @LastName";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@FirstName", player.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", player.LastName);

                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
        }

        public override bool Update(Player player)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE Players SET Nationality = @Nationality, GenderType = @GenderType, Rank = @Rank " +
                                   "WHERE FirstName = @FirstName AND LastName = @LastName";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@FirstName", player.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", player.LastName);
                    cmd.Parameters.AddWithValue("@Nationality", player.Nationality);
                    cmd.Parameters.AddWithValue("@GenderType", player.GenderType.ToString());
                    cmd.Parameters.AddWithValue("@Rank", player.Rank);

                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
        }

        public override Player Find(int id)
        {
            Player player = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Players WHERE IdPlayer = @IdPlayer";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdPlayer", id);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            player = new Player
                            {
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Nationality = reader.GetString(reader.GetOrdinal("Nationality")),
                                GenderType = (GenderType)Enum.Parse(typeof(GenderType), reader.GetString(reader.GetOrdinal("GenderType"))),
                                Rank = reader.GetInt32(reader.GetOrdinal("Rank"))
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return player;
        }

        public override List<Player> GetAll()
        {
            List<Player> players = new List<Player>();
            return players;
        }

        // Optional: Add a method to find players by name
        public Player FindByName(string firstName, string lastName)
        {
            Player player = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Players WHERE FirstName = @FirstName AND LastName = @LastName";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            player = new Player
                            {
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Nationality = reader.GetString(reader.GetOrdinal("Nationality")),
                                GenderType = (GenderType)Enum.Parse(typeof(GenderType), reader.GetString(reader.GetOrdinal("GenderType"))),
                                Rank = reader.GetInt32(reader.GetOrdinal("Rank"))
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return player;
        }
    }
}
