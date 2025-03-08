using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Entities.Orders;

namespace Ecom.API.MappingProfiles
{
    public class MappingAdderss: Profile
    {
        public MappingAdderss()
        {
            CreateMap<AdderssDto, Address>().ReverseMap();
            CreateMap<ShipAddress, AdderssDto>().ReverseMap();
        }
    }
}
