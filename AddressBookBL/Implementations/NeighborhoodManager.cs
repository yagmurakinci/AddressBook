using AddresBookEL.Entities;
using AddresBookEL.ViewModels;
using AddressBookBL.Interfaces;
using AddressBookDAL.InterfacesOfRepo;
using AddressBookDL.InterfacesOfRepo;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookBL.Implementations
{
    public class NeighborhoodManager
        : Manager<NeighborhoodVM, Neighborhood, int>,INeighborhoodManager
    {
        public NeighborhoodManager(INeighborhoodRepo repo, IMapper mapper) : base(repo, mapper, "District")
        {
        }
    }
}
