using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal class SetDAO : DAO<Set>
    {
        public SetDAO() : base()
        {
            //
        }

        public override bool Create(Set set)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "INSERT INTO Sets (ScoreOp1, ScoreOp2) VALUES (@ScoreOp1, @ScoreOp2)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ScoreOp1", set.ScoreOp1);
                    cmd.Parameters.AddWithValue("@ScoreOp2", set.ScoreOp2);

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

        public override bool Delete(Set set)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "DELETE FROM Sets WHERE IdSet = @IdSet";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdSet", set.IdSet); // Assuming IdSet is a unique identifier for the Set

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

        public override bool Update(Set set)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE Sets SET ScoreOp1 = @ScoreOp1, ScoreOp2 = @ScoreOp2 WHERE IdSet = @IdSet";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdSet", set.IdSet); // Assuming IdSet is a unique identifier for the Set
                    cmd.Parameters.AddWithValue("@ScoreOp1", set.ScoreOp1);
                    cmd.Parameters.AddWithValue("@ScoreOp2", set.ScoreOp2);

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

        public override Set Find(int id)
        {
            Set set = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Sets WHERE IdSet = @IdSet";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdSet", id);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            set = new Set
                            {
                                ScoreOp1 = reader.GetInt32(reader.GetOrdinal("ScoreOp1")),
                                ScoreOp2 = reader.GetInt32(reader.GetOrdinal("ScoreOp2")),
                                IdSet = reader.GetInt32(reader.GetOrdinal("IdSet")) // Assuming IdSet is a unique identifier for the Set
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return set;
        }

        public override List<Set> GetAll()
        {
            List<Set> sets = new List<Set>();
            return sets;
        }
    }
}
