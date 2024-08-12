using LassoieDylanProjetTennis.Ressources.Backend;
using LassoieDylanProjetTennis.Ressources.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.Factory
{
    public enum DAOFactoryType
    {
        MS_SQL_FACTORY,
        XML_FACTORY
    }

    abstract class AbstractDAOFactory
    {
        public abstract DAO<Court> GetCourtDAO();
        public abstract DAO<Match> GetMatchDAO();
        public abstract DAO<Opponent> GetOpponentDAO();
        public abstract DAO<Person> GetPersonDAO();
        public abstract DAO<Player> GetPlayerDAO();
        public abstract DAO<Referee> GetRefereeDAO();
        public abstract DAO<Schedule> GetScheduleDAO();
        public abstract DAO<Set> GetSetDAO();
        public abstract DAO<Stadium> GetStadiumDAO();
        public abstract DAO<SuperTieBreak> GetSuperTieBreakDAO();
        public abstract DAO<Tournament> GetTournamentDAO();

        public static AbstractDAOFactory GetFactory(DAOFactoryType type)
        {
            switch (type)
            {
                case DAOFactoryType.MS_SQL_FACTORY:
                    return new MSSQLFactory();
                case DAOFactoryType.XML_FACTORY:
                    // Return XML factory when implemented
                    return null;
                default:
                    return null;
            }
        }
    }
}
