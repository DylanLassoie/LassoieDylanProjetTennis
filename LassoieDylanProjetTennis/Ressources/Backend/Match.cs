using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.Backend
{
    class Match
    {
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public int Round { get; set; }

        public string GetWinner()
        {
            //Implementation
            return "Winner";
        }

        public void Play()
        {
            //Implementation
        }
    }
}
