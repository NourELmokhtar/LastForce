using AutoMapper;
using Forces.Application.Extensions;
using Forces.Application.Features.Person.Queries.GetBySpecifics;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Person.Queries.GetAll
{
    public class GetAllPersonsQuery : IRequest<Result<List<GetAllPersonsResponse>>>
    {

    }
    internal class GetAllPersonsQueryHandler : IRequestHandler<GetAllPersonsQuery, Result<List<GetAllPersonsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetAllPersonsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<List<GetAllPersonsResponse>>> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
        {

            var PersonList = await _unitOfWork.Repository<Models.Person>().GetAllAsync();
            var MappedUnitList = PersonList.Select(x => new GetAllPersonsResponse()
            {
                Id = x.Id,
                Name = x.Name,
                NationalNumber = x.NationalNumber,
                RoomId = x.RoomId,
                OfficePhone = x.OfficePhone,
                Phone = x.Phone,
                Section = x.Section,
                RoomNumber = _unitOfWork.Repository<Models.Room>().GetAllAsync().Result.Where(y => y.Id == x.RoomId).FirstOrDefault().RoomNumber,
                BuildingName = _unitOfWork.Repository<Models.Building>().GetAllAsync().Result.Where(y => y.Id == (
                _unitOfWork.Repository<Models.Room>().GetAllAsync().Result.Where(y => y.Id == x.RoomId).FirstOrDefault().BuildingId)).FirstOrDefault().BuildingName,
                BaseName = _unitOfWork.Repository<Models.Bases>().GetAllAsync().Result.Where(k => k.Id == _unitOfWork.Repository<Models.Building>().GetAllAsync().Result.Where(r => r.Id == (
                _unitOfWork.Repository<Models.Room>().GetAllAsync().Result.Where(y => y.Id == x.RoomId).FirstOrDefault().BuildingId)).FirstOrDefault().BaseId).FirstOrDefault().BaseName


            }).ToList();

            return await Result<List<GetAllPersonsResponse>>.SuccessAsync(MappedUnitList);

        }
    }
}