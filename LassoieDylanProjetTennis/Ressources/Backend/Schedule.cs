using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.Backend
{
    class Schedule
    {
        public enum ScheduleType
        {
            GentlemenSingle,
            LadiesSingle,
            GentlemenDouble,
            LadiesDouble,
            MixedDouble
        }
        public String ActualRound { get; set; }
    }
}
