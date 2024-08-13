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
    internal class RefereeDAO : DAO<Referee>
    {
        public RefereeDAO() : base()
        {
            //
        }

        public override bool Create(Referee referee)
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
                        personCmd.Parameters.AddWithValue("@FirstName", referee.FirstName);
                        personCmd.Parameters.AddWithValue("@LastName", referee.LastName);
                        personCmd.Parameters.AddWithValue("@Nationality", referee.Nationality);
                        personCmd.Parameters.AddWithValue("@GenderType", referee.GenderType.ToString());

                        personCmd.ExecuteNonQuery();

                        // Then, insert into the Referee table
                        string insertRefereeQuery = @"
                    INSERT INTO Referee (FirstName, LastName, League) 
                    VALUES (@FirstName, @LastName, @League)";

                        SqlCommand refereeCmd = new SqlCommand(insertRefereeQuery, connection, transaction);
                        refereeCmd.Parameters.AddWithValue("@FirstName", referee.FirstName);
                        refereeCmd.Parameters.AddWithValue("@LastName", referee.LastName);
                        refereeCmd.Parameters.AddWithValue("@League", referee.League);

                        refereeCmd.ExecuteNonQuery();

                        // Commit the transaction if both inserts succeed
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if any error occurs
                        transaction.Rollback();
                        throw new Exception("An error occurred while creating the Referee.", ex);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
        }

        public override bool Delete(Referee referee)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // First, delete from the Referee table
                        string deleteRefereeQuery = "DELETE FROM Referee WHERE FirstName = @FirstName AND LastName = @LastName";
                        SqlCommand deleteRefereeCmd = new SqlCommand(deleteRefereeQuery, connection, transaction);
                        deleteRefereeCmd.Parameters.AddWithValue("@FirstName", referee.FirstName);
                        deleteRefereeCmd.Parameters.AddWithValue("@LastName", referee.LastName);

                        int resultReferee = deleteRefereeCmd.ExecuteNonQuery();

                        if (resultReferee > 0)
                        {
                            // If successful, delete from the Person table
                            string deletePersonQuery = "DELETE FROM Person WHERE FirstName = @FirstName AND LastName = @LastName";
                            SqlCommand deletePersonCmd = new SqlCommand(deletePersonQuery, connection, transaction);
                            deletePersonCmd.Parameters.AddWithValue("@FirstName", referee.FirstName);
                            deletePersonCmd.Parameters.AddWithValue("@LastName", referee.LastName);

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
                            // Rollback if deleting the Referee failed
                            transaction.Rollback();
                            MessageBox.Show("Failed to delete the Referee.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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


        public override bool Update(Referee referee)
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
                        updatePersonCmd.Parameters.AddWithValue("@FirstName", referee.FirstName);
                        updatePersonCmd.Parameters.AddWithValue("@LastName", referee.LastName);
                        updatePersonCmd.Parameters.AddWithValue("@Nationality", referee.Nationality);
                        updatePersonCmd.Parameters.AddWithValue("@GenderType", referee.GenderType.ToString());

                        int resultPerson = updatePersonCmd.ExecuteNonQuery();

                        // Update the Referee table
                        string updateRefereeQuery = "UPDATE Referee SET League = @League " +
                                                    "WHERE FirstName = @FirstName AND LastName = @LastName";
                        SqlCommand updateRefereeCmd = new SqlCommand(updateRefereeQuery, connection, transaction);
                        updateRefereeCmd.Parameters.AddWithValue("@FirstName", referee.FirstName);
                        updateRefereeCmd.Parameters.AddWithValue("@LastName", referee.LastName);
                        updateRefereeCmd.Parameters.AddWithValue("@League", referee.League);

                        int resultReferee = updateRefereeCmd.ExecuteNonQuery();

                        if (resultPerson > 0 && resultReferee > 0)
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


        public override Referee Find(int id)
        {
            Referee referee = null;
            
            return referee;
        }

        public override List<Referee> GetAll()
        {
            List<Referee> referees = new List<Referee>();

            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    // SQL query to join Referee with Person based on FirstName and LastName
                    string query = @"
                SELECT 
                    p.FirstName,
                    p.LastName,
                    p.Nationality,
                    p.GenderType,
                    r.League
                FROM 
                    Person p
                INNER JOIN 
                    Referee r 
                ON 
                    p.FirstName = r.FirstName AND 
                    p.LastName = r.LastName";  // Joining on both FirstName and LastName

                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Referee referee = new Referee
                            {
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Nationality = reader.GetString(reader.GetOrdinal("Nationality")),
                                GenderType = (GenderType)Enum.Parse(typeof(GenderType), reader.GetString(reader.GetOrdinal("GenderType"))),
                                League = reader.GetString(reader.GetOrdinal("League"))
                            };

                            referees.Add(referee);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }

            return referees;
        }


        // Optional: Add a method to find referees by name
        public Referee FindByName(string firstName, string lastName)
        {
            Referee referee = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Referees WHERE FirstName = @FirstName AND LastName = @LastName";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            referee = new Referee
                            {
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Nationality = reader.GetString(reader.GetOrdinal("Nationality")),
                                GenderType = (GenderType)Enum.Parse(typeof(GenderType), reader.GetString(reader.GetOrdinal("GenderType"))),
                                League = reader.GetString(reader.GetOrdinal("League"))
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return referee;
        }
    }
}
