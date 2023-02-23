using AddresBookEL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookBL.Interfaces
{
    public interface ICityManager :
        IManager<CityVM,sbyte>
    {
        //Business katmanı genellikle VM/DTO alır/returnler.
    }
}
