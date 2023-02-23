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
    public class DistrictRepo:
        Repository<District,short>, IDistrictRepo
    {

        public DistrictRepo(AddressBookContext context):base(context)
        {

        }
    }
}
