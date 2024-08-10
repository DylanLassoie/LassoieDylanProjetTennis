using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    string query = "INSERT INTO Referees (FirstName, LastName, Nationality, GenderType, League) " +
                                   "VALUES (@FirstName, @LastName, @Nationality, @GenderType, @League)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@FirstName", referee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", referee.LastName);
                    cmd.Parameters.AddWithValue("@Nationality", referee.Nationality);
                    cmd.Parameters.AddWithValue("@GenderType", referee.GenderType.ToString());
                    cmd.Parameters.AddWithValue("@League", referee.League);

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

        public override bool Delete(Referee referee)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "DELETE FROM Referees WHERE FirstName = @FirstName AND LastName = @LastName";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@FirstName", referee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", referee.LastName);

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

        public override bool Update(Referee referee)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE Referees SET Nationality = @Nationality, GenderType = @GenderType, League = @League " +
                                   "WHERE FirstName = @FirstName AND LastName = @LastName";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@FirstName", referee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", referee.LastName);
                    cmd.Parameters.AddWithValue("@Nationality", referee.Nationality);
                    cmd.Parameters.AddWithValue("@GenderType", referee.GenderType.ToString());
                    cmd.Parameters.AddWithValue("@League", referee.League);

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

        public override Referee Find(int id)
        {
            Referee referee = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Referees WHERE IdReferee = @IdReferee";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdReferee", id);

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
