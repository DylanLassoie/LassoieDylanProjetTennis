using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal class PersonDAO : DAO<Person>
    {
        public PersonDAO() : base()
        {

        }

        public override bool Create(Person person)
        {
            return true;
        }

        public override bool Delete(Person person)
        {
            return true;
        }

        public override bool Update(Person person)
        {
            return true;
        }

        public override Person Find(int id)
        {
            Person person = null;
           
            return person;
        }

        public override List<Person> GetAll()
        {
            List<Person> persons = new List<Person>();
            return persons;
        }

        public bool UpdateParticipation(Person person)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Person SET Participation = @Participation WHERE FirstName = @FirstName AND LastName = @LastName";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Participation", person.Participation ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", person.LastName);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An SQL error occurred while updating the Participation!", ex);
            }
        }

    }
}
