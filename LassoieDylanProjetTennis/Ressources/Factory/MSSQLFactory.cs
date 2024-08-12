using LassoieDylanProjetTennis.Ressources.Backend;
using LassoieDylanProjetTennis.Ressources.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.Factory
{
    class MSSQLFactory : AbstractDAOFactory
    {
        public override DAO<Person> GetPersonDAO()
        {
            return new PersonDAO();
        }

        public override DAO<Referee> GetRefereeDAO()
        {
            return new RefereeDAO();
        }
    }
}
