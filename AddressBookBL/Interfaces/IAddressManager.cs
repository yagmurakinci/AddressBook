using AddresBookEL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookBL.Interfaces
{
    public interface IAddressManager:
        IManager<AddressVM,int>
    {
    }
}
