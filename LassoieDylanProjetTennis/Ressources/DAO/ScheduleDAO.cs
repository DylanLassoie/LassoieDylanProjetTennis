using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal class ScheduleDAO : DAO<Schedule>
    {
        public ScheduleDAO() : base()
        {
            //
        }

        public override bool Create(Schedule schedule)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "INSERT INTO Schedules (ScheduleType, ActualRound) VALUES (@ScheduleType, @ActualRound)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ScheduleType", schedule.ScheduleType.ToString());
                    cmd.Parameters.AddWithValue("@ActualRound", schedule.ActualRound);

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

        public override bool Delete(Schedule schedule)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "DELETE FROM Schedules WHERE ScheduleType = @ScheduleType AND ActualRound = @ActualRound";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ScheduleType", schedule.ScheduleType.ToString());
                    cmd.Parameters.AddWithValue("@ActualRound", schedule.ActualRound);

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

        public override bool Update(Schedule schedule)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE Schedules SET ActualRound = @ActualRound WHERE ScheduleType = @ScheduleType";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ScheduleType", schedule.ScheduleType.ToString());
                    cmd.Parameters.AddWithValue("@ActualRound", schedule.ActualRound);

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

        public override Schedule Find(int id)
        {
            Schedule schedule = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Schedules WHERE IdSchedule = @IdSchedule";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdSchedule", id);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            schedule = new Schedule
                            {
                                ScheduleType = (ScheduleType)Enum.Parse(typeof(ScheduleType), reader.GetString(reader.GetOrdinal("ScheduleType"))),
                                ActualRound = reader.GetInt32(reader.GetOrdinal("ActualRound"))
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return schedule;
        }

        // Optional: Add a method to find schedules by ScheduleType
        public Schedule FindByType(ScheduleType scheduleType)
        {
            Schedule schedule = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "SELECT * FROM Schedules WHERE ScheduleType = @ScheduleType";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ScheduleType", scheduleType.ToString());

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            schedule = new Schedule
                            {
                                ScheduleType = (ScheduleType)Enum.Parse(typeof(ScheduleType), reader.GetString(reader.GetOrdinal("ScheduleType"))),
                                ActualRound = reader.GetInt32(reader.GetOrdinal("ActualRound"))
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred!", ex);
            }
            return schedule;
        }
    }
}
