using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.Backend
{
    public enum GenderType
    {
        Male,
        Female
    }

    public class Person
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Nationality { get; set; }
        public GenderType GenderType { get; set; }
        public string Participation {  get; set; }
    }
}
