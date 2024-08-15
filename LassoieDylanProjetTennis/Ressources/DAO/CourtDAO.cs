using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal class CourtDAO : DAO<Court>
    {
        public CourtDAO() : base()
        {
            //
        }

        public override bool Create(Court court)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "INSERT INTO Courts (CourtType, NbSpectators, Covered, StadiumName) VALUES (@CourtType, @NbSpectators, @Covered, @StadiumName)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@CourtType", court.CourtType.ToString());
                    cmd.Parameters.AddWithValue("@NbSpectators", court.NbSpectators);
                    cmd.Parameters.AddWithValue("@Covered", court.Covered);
                    cmd.Parameters.AddWithValue("@StadiumName", court.StadiumName); // Assuming StadiumName is part of the Court model

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


        public override bool Delete(Court court)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "DELETE FROM Courts WHERE IdCourt = @IdCourt";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCourt", court.IdCourt);

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

        public override bool Update(Court court)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE Courts SET CourtType = @CourtType, NbSpectators = @NbSpectators, Covered = @Covered WHERE IdCourt = @IdCourt";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCourt", court.IdCourt);
                    cmd.Parameters.AddWithValue("@CourtType", court.CourtType.ToString());
                    cmd.Parameters.AddWithValue("@NbSpectators", court.NbSpectators);
                    cmd.Parameters.AddWithValue("@Covered", court.Covered);

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

        public override Court Find(int id)
        {
            Court court = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Courts WHERE IdCourt = @IdCourt";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCourt", id);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            court = new Court
                            {
                                IdCourt = reader.GetInt32(reader.GetOrdinal("IdCourt")),
                                CourtType = (CourtType)Enum.Parse(typeof(CourtType), reader.GetString(reader.GetOrdinal("CourtType"))),
                                NbSpectators = reader.GetInt32(reader.GetOrdinal("NbSpectators")),
                                Covered = reader.GetBoolean(reader.GetOrdinal("Covered"))
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return court;
        }

        public override List<Court> GetAll()
        {
            List<Court> courts = new List<Court>();
            return courts;
        }

        public bool CreateCourt(Court court, string stadiumName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Insert the court into the Court table
                        string insertCourtQuery = @"
                            INSERT INTO Court (CourtType, NbSpectators, Covered, StadiumName) 
                            VALUES (@CourtType, @NbSpectators, @Covered, @StadiumName)";

                        SqlCommand cmd = new SqlCommand(insertCourtQuery, connection, transaction);
                        cmd.Parameters.AddWithValue("@CourtType", court.CourtType.ToString());
                        cmd.Parameters.AddWithValue("@NbSpectators", court.NbSpectators);
                        cmd.Parameters.AddWithValue("@Covered", court.Covered);
                        cmd.Parameters.AddWithValue("@StadiumName", stadiumName);

                        cmd.ExecuteNonQuery();

                        // Commit the transaction if successful
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an error occurs
                        transaction.Rollback();
                        throw new Exception("An error occurred while creating the court.", ex);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
        }

        // Get the number of courts associated with a specific stadium
        public int GetNbCourtsForStadium(string stadiumName)
        {
            int nbCourts = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT NbCourts 
                FROM Stadium 
                WHERE NameOfStadium = @StadiumName";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@StadiumName", stadiumName);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        nbCourts = (int)result;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }

            return nbCourts;
        }
        public int GetCourtCountForStadium(string stadiumName)
        {
            int courtCount = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT COUNT(*) 
                        FROM Court 
                        WHERE StadiumName = @StadiumName";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@StadiumName", stadiumName);

                    courtCount = (int)cmd.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }

            return courtCount;
        }



    }
}
