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
    public class AddressRepo:
        Repository<Address,int>, IAddressRepo
    {
        //IAddressRepoyu burada kalıtmamızın sebebi farklı classların içinde rahatlıkla DI yapabilmek 
        public AddressRepo(AddressBookContext context):base(context)
        {

        }
    }
}
