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
            //
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
    }
}
