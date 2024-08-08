using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal interface IPersonDAO
    {
        void Add(Person person);
        Person Get(string firstName, string lastName);
        void Update(Person person);
        void Delete(string firstName, string lastName);
        IEnumerable<Person> GetAll();
    }
}
