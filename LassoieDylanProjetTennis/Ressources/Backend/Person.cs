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

    class Person
    {
        private String FirstName { get; set; }
        private String LastName { get; set; }
        private String Nationality { get; set; }
        private GenderType GenderType { get; set; }
    }
}
