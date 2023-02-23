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
    public class CityManager:
        Manager<CityVM,City,sbyte>, ICityManager

    {
        public CityManager(IMapper mapper, ICityRepo repo) :base(includeRelationalTables:"",
            repo:repo,mapper:mapper)
        {
            //parametreleri istediğiniz sırayla göndermek için iki nokta üst üsteyi kullanarak o parametreye değer atayabilirsiniz

        }
    }
}
