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
                    string query = "INSERT INTO Courts (CourtType, NbSpectators, Covered) VALUES (@CourtType, @NbSpectators, @Covered)";
                    SqlCommand cmd = new SqlCommand(query, connection);
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
    }
}
