using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.Backend
{
    public class Opponent
    {
        public int IdOpponentId { get; set; }
        public int MatchId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
