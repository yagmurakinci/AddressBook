using AddresBookEL.Entities;
using AddressBookDAL.InterfacesOfRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookDL.InterfacesOfRepo
{
    public interface IDistrictRepo:IRepository<District,short>
    {
    }
}
