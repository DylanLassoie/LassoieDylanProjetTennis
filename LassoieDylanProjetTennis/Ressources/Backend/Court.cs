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

    public class Court
    {
        public string StadiumName { get; set; }
        public int IdCourt { get; set; }
        public CourtType CourtType { get; set; }
        public int NbSpectators { get; set; }
        public bool Covered { get; set; }

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
