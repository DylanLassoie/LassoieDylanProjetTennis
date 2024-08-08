using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal class RefereeDAO : IRefereeDAO
    {
        private readonly string _connectionString;

        public RefereeDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Referee referee)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Referee (FirstName, LastName, League) VALUES (@FirstName, @LastName, @League)", connection);
                command.Parameters.AddWithValue("@FirstName", referee.FirstName);
                command.Parameters.AddWithValue("@LastName", referee.LastName);
                command.Parameters.AddWithValue("@League", referee.League);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Referee Get(string firstName, string lastName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Referee WHERE FirstName = @FirstName AND LastName = @LastName", connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Referee
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            League = reader["League"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Update(Referee referee)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Referee SET League = @League WHERE FirstName = @FirstName AND LastName = @LastName", connection);
                command.Parameters.AddWithValue("@FirstName", referee.FirstName);
                command.Parameters.AddWithValue("@LastName", referee.LastName);
                command.Parameters.AddWithValue("@League", referee.League);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(string firstName, string lastName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Referee WHERE FirstName = @FirstName AND LastName = @LastName", connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Referee> GetAll()
        {
            var referees = new List<Referee>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Referee", connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        referees.Add(new Referee
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            League = reader["League"].ToString()
                        });
                    }
                }
            }
            return referees;
        }
    }
}
