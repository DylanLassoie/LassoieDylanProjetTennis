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

    class Schedule
    {
        public ScheduleType ScheduleType { get; set; }
        public int ActualRound { get; set; }

        public int NbWinningSets() 
        {
            //Implementation
            return 0;
        }

        public void PlayNextRound()
        {
            //Implementation
        }

        public string GetWinner()
        {
            //Implementation
            return "Winner";
        }
    }
}
