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
    internal class StadiumDAO : DAO<Stadium>
    {
        public StadiumDAO() : base()
        {
            //
        }

        public override bool Create(Stadium stadium)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Insertion dans la table Stadium
                        string insertStadiumQuery = @"
                INSERT INTO Stadium (NameOfStadium, Location, NbCourts) 
                VALUES (@NameOfStadium, @Location, @NbCourts)";

                        SqlCommand stadiumCmd = new SqlCommand(insertStadiumQuery, connection, transaction);
                        stadiumCmd.Parameters.AddWithValue("@NameOfStadium", stadium.NameOfStadium);
                        stadiumCmd.Parameters.AddWithValue("@Location", stadium.Location);
                        stadiumCmd.Parameters.AddWithValue("@NbCourts", stadium.NbCourts);

                        stadiumCmd.ExecuteNonQuery();

                        // Commit la transaction si l'insertion réussit
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Rollback de la transaction en cas d'erreur
                        transaction.Rollback();
                        throw new Exception("An error occurred while creating the Stadium.", ex);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
        }


        public override bool Delete(Stadium stadium)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Supprimer de la table Stadium
                        string deleteStadiumQuery = "DELETE FROM Stadium WHERE NameOfStadium = @NameOfStadium AND Location = @Location";
                        SqlCommand deleteStadiumCmd = new SqlCommand(deleteStadiumQuery, connection, transaction);
                        deleteStadiumCmd.Parameters.AddWithValue("@NameOfStadium", stadium.NameOfStadium);
                        deleteStadiumCmd.Parameters.AddWithValue("@Location", stadium.Location);

                        int resultStadium = deleteStadiumCmd.ExecuteNonQuery();

                        if (resultStadium > 0)
                        {
                            // Commit la transaction si la suppression a réussi
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            // Rollback si la suppression échoue
                            transaction.Rollback();
                            MessageBox.Show("Failed to delete the Stadium.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }
                    catch (SqlException ex)
                    {
                        // Rollback et afficher le message d'erreur SQL
                        transaction.Rollback();
                        MessageBox.Show($"SQL Error: {ex.Message}", "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Capturer et afficher toute autre exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }


        public override bool Update(Stadium stadium)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Mettre à jour la table Stadium
                        string updateStadiumQuery = @"
                UPDATE Stadium 
                SET Location = @Location, NbCourts = @NbCourts 
                WHERE NameOfStadium = @NameOfStadium";

                        SqlCommand updateStadiumCmd = new SqlCommand(updateStadiumQuery, connection, transaction);
                        updateStadiumCmd.Parameters.AddWithValue("@NameOfStadium", stadium.NameOfStadium);
                        updateStadiumCmd.Parameters.AddWithValue("@Location", stadium.Location);
                        updateStadiumCmd.Parameters.AddWithValue("@NbCourts", stadium.NbCourts);

                        int resultStadium = updateStadiumCmd.ExecuteNonQuery();

                        if (resultStadium > 0)
                        {
                            // Commit la transaction si la mise à jour a réussi
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            // Rollback si la mise à jour échoue
                            transaction.Rollback();
                            return false;
                        }
                    }
                    catch (SqlException ex)
                    {
                        // Rollback et afficher le message d'erreur SQL
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


        public override Stadium Find(int id)
        {
            Stadium stadium = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Stadiums WHERE IdStadium = @IdStadium";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdStadium", id);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stadium = new Stadium
                            {
                                NameOfStadium = reader.GetString(reader.GetOrdinal("NameOfStadium")),
                                Location = reader.GetString(reader.GetOrdinal("Location")),
                                NbCourts = reader.GetInt32(reader.GetOrdinal("NbCourts"))
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return stadium;
        }

        public override List<Stadium> GetAll()
        {
            List<Stadium> stadiums = new List<Stadium>();

            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    // SQL query to select all columns from the Stadium table
                    string query = @"
            SELECT 
                NameOfStadium,
                Location,
                NbCourts
            FROM 
                Stadium";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Stadium stadium = new Stadium
                            {
                                NameOfStadium = reader.GetString(reader.GetOrdinal("NameOfStadium")),
                                Location = reader.GetString(reader.GetOrdinal("Location")),
                                NbCourts = reader.GetInt32(reader.GetOrdinal("NbCourts"))
                            };

                            stadiums.Add(stadium);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }

            return stadiums;
        }


        // Optional: Add a method to find stadiums by name
        public Stadium FindByName(string name)
        {
            Stadium stadium = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Stadiums WHERE NameOfStadium = @NameOfStadium";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@NameOfStadium", name);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stadium = new Stadium
                            {
                                NameOfStadium = reader.GetString(reader.GetOrdinal("NameOfStadium")),
                                Location = reader.GetString(reader.GetOrdinal("Location")),
                                NbCourts = reader.GetInt32(reader.GetOrdinal("NbCourts"))
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return stadium;
        }
    }
}
