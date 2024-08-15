using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.Backend
{
    public class Match
    {
        public int IdMatch { get; set; }
        public string ScheduleType { get; set; }
        public string TournamentName { get; set; }
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
