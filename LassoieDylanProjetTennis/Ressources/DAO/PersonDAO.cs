using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal class PersonDAO : IPersonDAO
    {
        private readonly string _connectionString;

        public PersonDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Person person)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Person (FirstName, LastName, Nationality, Gender) VALUES (@FirstName, @LastName, @Nationality, @Gender)", connection);
                command.Parameters.AddWithValue("@FirstName", person.FirstName);
                command.Parameters.AddWithValue("@LastName", person.LastName);
                command.Parameters.AddWithValue("@Nationality", person.Nationality);
                command.Parameters.AddWithValue("@Gender", person.Gender.ToString());

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Person Get(string firstName, string lastName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Person WHERE FirstName = @FirstName AND LastName = @LastName", connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Person
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Nationality = reader["Nationality"].ToString(),
                            Gender = Enum.Parse<Gender>(reader["Gender"].ToString())
                        };
                    }
                }
            }
            return null;
        }

        public void Update(Person person)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Person SET Nationality = @Nationality, Gender = @Gender WHERE FirstName = @FirstName AND LastName = @LastName", connection);
                command.Parameters.AddWithValue("@FirstName", person.FirstName);
                command.Parameters.AddWithValue("@LastName", person.LastName);
                command.Parameters.AddWithValue("@Nationality", person.Nationality);
                command.Parameters.AddWithValue("@Gender", person.Gender.ToString());

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(string firstName, string lastName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Person WHERE FirstName = @FirstName AND LastName = @LastName", connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Person> GetAll()
        {
            var people = new List<Person>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Person", connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        people.Add(new Person
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Nationality = reader["Nationality"].ToString(),
                            Gender = Enum.Parse<Gender>(reader["Gender"].ToString())
                        });
                    }
                }
            }
            return people;
        }
    }
}
