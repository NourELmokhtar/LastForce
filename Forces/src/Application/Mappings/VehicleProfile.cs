using AutoMapper;
using Forces.Application.Features.Vehicle.Commands.AddEdit;
using Forces.Application.Features.Vehicle.Queries.GetAll;
using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Mappings
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<AddEditVehicleCommand, Vehicle>().ReverseMap();
            CreateMap<Vehicle,GetAllVehicleResponse>().ForMember(x=>x.ColorName,x=>x.MapFrom(x=>x.VehicleColor.ColorName));
        }
    }
}
