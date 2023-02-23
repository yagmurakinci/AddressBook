using AddresBookEL.Entities;
using AddresBookEL.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddresBookEL.Mappings
{
    public class Maps :Profile
    {
        public Maps()
        {
            //buraya geri döneceğiz...
            //modeli viewmodele çevireceğiz
            //createmap yazılacak
            CreateMap<City, CityVM>().ReverseMap();
            CreateMap<Address, AddressVM>().ReverseMap();
            CreateMap<District, DistrictVM>().ReverseMap();
            CreateMap<Neighborhood, NeighborhoodVM>()
                .ReverseMap();
        }
    }
}
