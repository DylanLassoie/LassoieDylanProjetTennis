using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal interface IRefereeDAO
    {
        void Add(Referee referee);
        Referee Get(string firstName, string lastName);
        void Update(Referee referee);
        void Delete(string firstName, string lastName);
        IEnumerable<Referee> GetAll();
    }
}
