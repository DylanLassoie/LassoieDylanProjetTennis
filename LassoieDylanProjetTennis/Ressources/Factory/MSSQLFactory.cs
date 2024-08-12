using LassoieDylanProjetTennis.Ressources.Backend;
using LassoieDylanProjetTennis.Ressources.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.Factory
{
    class MSSQLFactory : AbstractDAOFactory
    {
        public override DAO<Court> GetCourtDAO()
        {
            return new CourtDAO();
        }

        public override DAO<Match> GetMatchDAO()
        {
            return new MatchDAO();
        }

        public override DAO<Opponent> GetOpponentDAO() 
        { 
            return new OpponentDAO(); 
        }

        public override DAO<Person> GetPersonDAO()
        {
            return new PersonDAO();
        }

        public override DAO<Player> GetPlayerDAO() 
        { 
            return new PlayerDAO(); 
        }

        public override DAO<Referee> GetRefereeDAO()
        {
            return new RefereeDAO();
        }

        public override DAO<Schedule> GetScheduleDAO() 
        { 
            return new ScheduleDAO();
        }

        public override DAO<Set> GetSetDAO() 
        { 
            return new SetDAO(); 
        }

        public override DAO<Stadium> GetStadiumDAO() 
        { 
            return new StadiumDAO(); 
        }

        public override DAO<SuperTieBreak> GetSuperTieBreakDAO()
        {
            return new SuperTieBreakDAO();
        }

        public override DAO<Tournament> GetTournamentDAO() 
        {
            return new TournamentDAO(); 
        }
    }
}
