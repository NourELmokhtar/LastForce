using AutoMapper;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Items.Queries.GetAll
{
    public class GetAllItemsQuery : IRequest<IResult<List<GetAllItemsResponse>>>
    {
        public GetAllItemsQuery()
        {

        }
    }
    internal class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, IResult<List<GetAllItemsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IVoteCodeService _voteCodeService;

        public GetAllItemsQueryHandler(IUnitOfWork<int> unitOfWork, IVoteCodeService voteCodeService)
        {
            _unitOfWork = unitOfWork;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<List<GetAllItemsResponse>>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            var ItemsList = await _unitOfWork.Repository<Models.Items>().Entities.Include(x => x.MeasureUnit).ToListAsync();
            var vCodes = await _voteCodeService.GetAllCodes();
            var MappedItems = (from item in ItemsList
                               join voteCode in vCodes.Data on item.VoteCodesId equals voteCode.Id
                               select new GetAllItemsResponse()
                               {
                                   ItemArName = item.ItemArName,
                                   Id = item.Id,
                                   ItemCode = item.ItemCode,
                                   ItemDescription = item.ItemDescription,
                                   ItemName = item.ItemName,
                                   ItemNsn = item.ItemNsn,
                                   MeasureUnitId = item.MeasureUnitId,
                                   MeasureName = item.MeasureUnit.Name,
                                   VoteCodesId = item.VoteCodesId,
                                   VoteCode = voteCode.VoteCode,
                                   ItemClass = item.ItemClass,
                                   SerialNumber = item.SerialNumber
                                   ,
                                   DateOfEnter = item.DateOfEnter
                                   ,
                                   EndOfServiceDate = item.EndOfServiceDate
                                   ,
                                   MadeIn = item.MadeIn
                                   ,
                                   FirstUseDate = item.FirstUseDate
                               }
                         ).ToList();

            //var MappedItems = ItemsList.Select(x => new GetAllItemsResponse()
            //{
            //    ItemArName = x.ItemArName,
            //    Id = x.Id,
            //    ItemCode = x.ItemCode,
            //    ItemDescription = x.ItemDescription,
            //    ItemName = x.ItemName,
            //    ItemNsn = x.ItemNsn,
            //    MeasureUnitId = x.MeasureUnitId,
            //    MeasureName = x.MeasureUnit.Name,
            //    VoteCodesId = x.VoteCodesId
            //}).ToList();
            return await Result<List<GetAllItemsResponse>>.SuccessAsync(MappedItems);
        }
    }
}
