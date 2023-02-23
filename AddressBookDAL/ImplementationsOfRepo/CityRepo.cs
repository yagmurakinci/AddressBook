using AddresBookEL.Entities;
using AddressBookDAL.ContextInfo;
using AddressBookDL.InterfacesOfRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookDL.ImplementationsOfRepo
{
    public class CityRepo:
        Repository<City,sbyte>, ICityRepo
    {
        public CityRepo(AddressBookContext context):base(context)
        {

        }
    }
}
