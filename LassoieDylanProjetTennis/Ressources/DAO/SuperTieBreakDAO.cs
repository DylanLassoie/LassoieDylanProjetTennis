using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal class SuperTieBreakDAO : DAO<SuperTieBreak>
    {
        public SuperTieBreakDAO() : base()
        {
            //
        }

        public override bool Create(SuperTieBreak superTieBreak)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "INSERT INTO SuperTieBreaks (ScoreOp1, ScoreOp2) VALUES (@ScoreOp1, @ScoreOp2)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ScoreOp1", superTieBreak.ScoreOp1);
                    cmd.Parameters.AddWithValue("@ScoreOp2", superTieBreak.ScoreOp2);

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

        public override bool Delete(SuperTieBreak superTieBreak)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "DELETE FROM SuperTieBreaks WHERE IdSuperTieBreak = @IdSuperTieBreak";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdSuperTieBreak", superTieBreak.IdSuperTieBreak); // Assuming IdSuperTieBreak is a unique identifier for the SuperTieBreak

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

        public override bool Update(SuperTieBreak superTieBreak)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE SuperTieBreaks SET ScoreOp1 = @ScoreOp1, ScoreOp2 = @ScoreOp2 WHERE IdSuperTieBreak = @IdSuperTieBreak";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdSuperTieBreak", superTieBreak.IdSuperTieBreak); // Assuming IdSuperTieBreak is a unique identifier for the SuperTieBreak
                    cmd.Parameters.AddWithValue("@ScoreOp1", superTieBreak.ScoreOp1);
                    cmd.Parameters.AddWithValue("@ScoreOp2", superTieBreak.ScoreOp2);

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

        public override SuperTieBreak Find(int id)
        {
            SuperTieBreak superTieBreak = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM SuperTieBreaks WHERE IdSuperTieBreak = @IdSuperTieBreak";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdSuperTieBreak", id);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            superTieBreak = new SuperTieBreak
                            {
                                ScoreOp1 = reader.GetInt32(reader.GetOrdinal("ScoreOp1")),
                                ScoreOp2 = reader.GetInt32(reader.GetOrdinal("ScoreOp2")),
                                IdSuperTieBreak = reader.GetInt32(reader.GetOrdinal("IdSuperTieBreak")) // Assuming IdSuperTieBreak is a unique identifier for the SuperTieBreak
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return superTieBreak;
        }

        public override List<SuperTieBreak> GetAll()
        {
            List<SuperTieBreak> superTieBreaks = new List<SuperTieBreak>();
            return superTieBreaks;
        }
    }
}
