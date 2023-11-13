using AutoMapper;
using Forces.Application.Features.VehicleRequest.Dto;
using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Mappings
{
    public class VehicleRequestProfile : Profile
    {
        public VehicleRequestProfile()
        {
            CreateMap<AddRequestDto, VehicleRequest>().ReverseMap();
            CreateMap<VehicleRequestPackageDto, VehicleRequestPackage>().ReverseMap();
        }
    }
}
