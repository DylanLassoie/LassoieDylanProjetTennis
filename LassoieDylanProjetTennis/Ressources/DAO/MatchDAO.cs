using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal class MatchDAO : DAO<Match>
    {
        public MatchDAO() : base()
        {
            //
        }

        public override bool Create(Match match)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "INSERT INTO Matches (Date, Duration, Round) VALUES (@Date, @Duration, @Round)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Date", match.Date);
                    cmd.Parameters.AddWithValue("@Duration", match.Duration);
                    cmd.Parameters.AddWithValue("@Round", match.Round);

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

        public override bool Delete(Match match)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "DELETE FROM Matches WHERE Date = @Date AND Round = @Round";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Date", match.Date);
                    cmd.Parameters.AddWithValue("@Round", match.Round);

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

        public override bool Update(Match match)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE Matches SET Duration = @Duration WHERE Date = @Date AND Round = @Round";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Date", match.Date);
                    cmd.Parameters.AddWithValue("@Round", match.Round);
                    cmd.Parameters.AddWithValue("@Duration", match.Duration);

                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result >  0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
        }

        public override Match Find(int id)
        {
            Match match = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Matches WHERE IdMatch = @IdMatch";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdMatch", id);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            match = new Match
                            {
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetTimeSpan(reader.GetOrdinal("Duration")),
                                Round = reader.GetInt32(reader.GetOrdinal("Round"))
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return match;
        }
    }
}
