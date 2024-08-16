using LassoieDylanProjetTennis.Ressources.Backend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.DAO
{
    internal class OpponentDAO : DAO<Opponent>
    {
        public OpponentDAO() : base()
        {

        }

        public override bool Create(Opponent opponent)
        {
            return true;
        }

        public override bool Delete(Opponent opponent)
        {
            return true;
        }

        public override bool Update(Opponent opponent)
        {
           return true ;
        }

        public override Opponent Find(int id)
        {
            Opponent opponent = null;
            return opponent;
        }

        public override List<Opponent> GetAll()
        {
            List<Opponent> opponents = new List<Opponent>();
            return opponents;
        }

        public Opponent FindByName(string name)
        {
            Opponent opponent = null;
           
            return opponent;
        }
    }
}
