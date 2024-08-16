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
                    cmd.Parameters.AddWithValue("@StadiumName", court.StadiumName); 

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
            return true;
        }

        public override bool Update(Court court)
        {
            return true;
        }

        public override Court Find(int id)
        {
            Court court = null;
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
                        string insertCourtQuery = @"
                            INSERT INTO Court (CourtType, NbSpectators, Covered, StadiumName) 
                            VALUES (@CourtType, @NbSpectators, @Covered, @StadiumName)";

                        SqlCommand cmd = new SqlCommand(insertCourtQuery, connection, transaction);
                        cmd.Parameters.AddWithValue("@CourtType", court.CourtType.ToString());
                        cmd.Parameters.AddWithValue("@NbSpectators", court.NbSpectators);
                        cmd.Parameters.AddWithValue("@Covered", court.Covered);
                        cmd.Parameters.AddWithValue("@StadiumName", stadiumName);

                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
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
