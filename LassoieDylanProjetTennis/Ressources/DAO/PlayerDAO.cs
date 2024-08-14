using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // First, insert into the Person table
                        string insertPersonQuery = @"
            INSERT INTO Person (FirstName, LastName, Nationality, GenderType) 
            VALUES (@FirstName, @LastName, @Nationality, @GenderType)";

                        SqlCommand personCmd = new SqlCommand(insertPersonQuery, connection, transaction);
                        personCmd.Parameters.AddWithValue("@FirstName", player.FirstName);
                        personCmd.Parameters.AddWithValue("@LastName", player.LastName);
                        personCmd.Parameters.AddWithValue("@Nationality", player.Nationality);
                        personCmd.Parameters.AddWithValue("@GenderType", player.GenderType.ToString());

                        personCmd.ExecuteNonQuery();

                        // Then, insert into the Player table
                        string insertPlayerQuery = @"
            INSERT INTO Player (FirstName, LastName, Rank) 
            VALUES (@FirstName, @LastName, @Rank)";

                        SqlCommand playerCmd = new SqlCommand(insertPlayerQuery, connection, transaction);
                        playerCmd.Parameters.AddWithValue("@FirstName", player.FirstName);
                        playerCmd.Parameters.AddWithValue("@LastName", player.LastName);
                        playerCmd.Parameters.AddWithValue("@Rank", player.Rank);

                        playerCmd.ExecuteNonQuery();

                        // Commit the transaction if both inserts succeed
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if any error occurs
                        transaction.Rollback();
                        throw new Exception("An error occurred while creating the Player.", ex);
                    }
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
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // First, delete from the Player table
                        string deletePlayerQuery = "DELETE FROM Player WHERE FirstName = @FirstName AND LastName = @LastName";
                        SqlCommand deletePlayerCmd = new SqlCommand(deletePlayerQuery, connection, transaction);
                        deletePlayerCmd.Parameters.AddWithValue("@FirstName", player.FirstName);
                        deletePlayerCmd.Parameters.AddWithValue("@LastName", player.LastName);

                        int resultPlayer = deletePlayerCmd.ExecuteNonQuery();

                        if (resultPlayer > 0)
                        {
                            // If successful, delete from the Person table
                            string deletePersonQuery = "DELETE FROM Person WHERE FirstName = @FirstName AND LastName = @LastName";
                            SqlCommand deletePersonCmd = new SqlCommand(deletePersonQuery, connection, transaction);
                            deletePersonCmd.Parameters.AddWithValue("@FirstName", player.FirstName);
                            deletePersonCmd.Parameters.AddWithValue("@LastName", player.LastName);

                            int resultPerson = deletePersonCmd.ExecuteNonQuery();

                            if (resultPerson > 0)
                            {
                                // Commit the transaction if both deletions succeed
                                transaction.Commit();
                                return true;
                            }
                            else
                            {
                                // Rollback if deleting the Person failed
                                transaction.Rollback();
                                MessageBox.Show("Failed to delete the corresponding Person.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }
                        }
                        else
                        {
                            // Rollback if deleting the Player failed
                            transaction.Rollback();
                            MessageBox.Show("Failed to delete the Player.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }
                    catch (SqlException ex)
                    {
                        // Rollback and display SQL error message
                        transaction.Rollback();
                        MessageBox.Show($"SQL Error: {ex.Message}", "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Capture and display any other exceptions
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }


        public override bool Update(Player player)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Update the Person table
                        string updatePersonQuery = "UPDATE Person SET Nationality = @Nationality, GenderType = @GenderType " +
                                                   "WHERE FirstName = @FirstName AND LastName = @LastName";
                        SqlCommand updatePersonCmd = new SqlCommand(updatePersonQuery, connection, transaction);
                        updatePersonCmd.Parameters.AddWithValue("@FirstName", player.FirstName);
                        updatePersonCmd.Parameters.AddWithValue("@LastName", player.LastName);
                        updatePersonCmd.Parameters.AddWithValue("@Nationality", player.Nationality);
                        updatePersonCmd.Parameters.AddWithValue("@GenderType", player.GenderType.ToString());

                        int resultPerson = updatePersonCmd.ExecuteNonQuery();

                        // Update the Player table
                        string updatePlayerQuery = "UPDATE Player SET Rank = @Rank " +
                                                   "WHERE FirstName = @FirstName AND LastName = @LastName";
                        SqlCommand updatePlayerCmd = new SqlCommand(updatePlayerQuery, connection, transaction);
                        updatePlayerCmd.Parameters.AddWithValue("@FirstName", player.FirstName);
                        updatePlayerCmd.Parameters.AddWithValue("@LastName", player.LastName);
                        updatePlayerCmd.Parameters.AddWithValue("@Rank", player.Rank);

                        int resultPlayer = updatePlayerCmd.ExecuteNonQuery();

                        if (resultPerson > 0 && resultPlayer > 0)
                        {
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An SQL error occurred!", ex);
                    }
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

            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    // SQL query to join Player with Person based on FirstName and LastName
                    string query = @"
        SELECT 
            p.FirstName,
            p.LastName,
            p.Nationality,
            p.GenderType,
            pl.Rank
        FROM 
            Person p
        INNER JOIN 
            Player pl 
        ON 
            p.FirstName = pl.FirstName AND 
            p.LastName = pl.LastName";  // Joining on both FirstName and LastName

                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Player player = new Player
                            {
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Nationality = reader.GetString(reader.GetOrdinal("Nationality")),
                                GenderType = (GenderType)Enum.Parse(typeof(GenderType), reader.GetString(reader.GetOrdinal("GenderType"))),
                                Rank = reader.GetInt32(reader.GetOrdinal("Rank"))
                            };

                            players.Add(player);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }

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
