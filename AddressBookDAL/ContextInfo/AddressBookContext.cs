using AddresBookEL.Entities;
using AddresBookEL.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookDAL.ContextInfo
{
    public class AddressBookContext:IdentityDbContext<AppUser,AppRole, string>
    {
        public AddressBookContext(DbContextOptions<AddressBookContext> options):base(options)
        {

        } // ctor bitti

        public DbSet<City> CityTable { get; set; }
        public DbSet<District> DistrictTable { get; set; }
        public DbSet<Address> AddressTable { get; set; }

        public DbSet<Neighborhood> NeighborhoodTable { get; set; }


    }
}
