using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LassoieDylanProjetTennis.Ressources.Backend
{
    class Person
    {
        public String Firstname { get; set; }
        public String Lastname { get; set; }
        public String Nationality { get; set; }
        public enum Gender
        {
            Male,
            Female
        }
    }
}
