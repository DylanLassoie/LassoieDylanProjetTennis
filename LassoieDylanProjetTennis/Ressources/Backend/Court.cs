using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.Backend
{
    public enum CourtType
    {
        Hard,
        Grass,
        Clay,
        Artificial
    }

    class Court
    {
        private int IdCourt { get; set; }
        private CourtType CourtType { get; set; }
        private int NbSpectators { get; set; }
        private bool Covered { get; set; }

        public bool Available()
        {
            //Implementation
            return true;
        }

        public void Release() 
        {
            //Implementation
        }
    }
}
