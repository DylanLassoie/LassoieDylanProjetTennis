using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.Backend
{
    public enum ScheduleType
    {
        GentlemenSingle,
        LadiesSingle,
        GentlemenDouble,
        LadiesDouble,
        MixedDouble
    }

    public class Schedule
    {
        public string TournamentName { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public int ActualRound { get; set; }

        public int NbWinningSets() 
        {
            return 0;
        }

        public void PlayNextRound()
        {
           
        }

        public string GetWinner()
        {
            return "Winner";
        }
    }
}
