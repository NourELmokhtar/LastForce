using AutoMapper;
using Forces.Application.Features.Vehicle.Commands.AddEdit;
using Forces.Application.Features.Vehicle.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Mappings
{
    public class VehiclesProfile : Profile
    {
        public VehiclesProfile()
        {
            CreateMap<GetAllVehicleResponse, AddEditVehicleCommand>().ReverseMap();
        }
    }
}
