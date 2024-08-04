using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.Backend
{
    class Court
    {
        public String IdCourt { get; set; }

        public enum CourtType
        {
            Hard,
            Grass,
            Clay,
            Artificial
        }

        public String NbSpectators { get; set; }
        public String Covered { get; set; }

        
    }
}
