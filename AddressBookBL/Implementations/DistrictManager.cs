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
    public class DistrictManager:
        Manager<DistrictVM,District,short>, IDistrictManager
    {
        public DistrictManager(IMapper mapper,IDistrictRepo repo):base(repo,mapper,"City")
        {
            //District classına gidip virtual tanımlı propertynin adını "ilişkili tablo alanına" yazdım.Böylece District verisi gelirken City bilgisiyle gelecek
        }
    }
}
