using AddresBookEL.Entities;
using AddresBookEL.ViewModels;
using AddressBookBL.Interfaces;
using AddressBookDL.InterfacesOfRepo;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookBL.Implementations
{
    public class AddressManager : Manager<AddressVM, Address, int>, IAddressManager
    {
        public AddressManager(IAddressRepo repo, IMapper mapper) : base(repo,mapper, "Neighborhood,AppUser")
        {

        }
    }
}
