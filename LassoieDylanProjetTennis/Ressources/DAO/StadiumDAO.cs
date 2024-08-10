using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    string query = "INSERT INTO Stadiums (NameOfStadium, Location, NbCourts) VALUES (@NameOfStadium, @Location, @NbCourts)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@NameOfStadium", stadium.NameOfStadium);
                    cmd.Parameters.AddWithValue("@Location", stadium.Location);
                    cmd.Parameters.AddWithValue("@NbCourts", stadium.NbCourts);

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

        public override bool Delete(Stadium stadium)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "DELETE FROM Stadiums WHERE NameOfStadium = @NameOfStadium";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@NameOfStadium", stadium.NameOfStadium);

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

        public override bool Update(Stadium stadium)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE Stadiums SET Location = @Location, NbCourts = @NbCourts WHERE NameOfStadium = @NameOfStadium";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@NameOfStadium", stadium.NameOfStadium);
                    cmd.Parameters.AddWithValue("@Location", stadium.Location);
                    cmd.Parameters.AddWithValue("@NbCourts", stadium.NbCourts);

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
