using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal interface IPlayerDAO
    {
        void Add(Player player);
        Player Get(string firstName, string lastName);
        void Update(Player player);
        void Delete(string firstName, string lastName);
        IEnumerable<Player> GetAll();
    }
}
