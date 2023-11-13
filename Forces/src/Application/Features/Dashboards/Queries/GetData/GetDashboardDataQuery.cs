using Forces.Application.Extensions;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Domain.Entities.Catalog;
using Forces.Domain.Entities.ExtendedAttributes;
using Forces.Domain.Entities.Misc;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Dashboards.Queries.GetData
{
    public class GetDashboardDataQuery : IRequest<Result<DashboardDataResponse>>
    {
        public string UserID { get; set; }
    }

    internal class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, Result<DashboardDataResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IStringLocalizer<GetDashboardDataQueryHandler> _localizer;
        private readonly IVoteCodeService _voteCodeService;

        public GetDashboardDataQueryHandler(IUnitOfWork<int> unitOfWork, IUserService userService, IRoleService roleService, IStringLocalizer<GetDashboardDataQueryHandler> localizer, IVoteCodeService voteCodeService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _roleService = roleService;
            _localizer = localizer;
            _voteCodeService = voteCodeService;
        }

        public async Task<Result<DashboardDataResponse>> Handle(GetDashboardDataQuery query, CancellationToken cancellationToken)
        {

            var userTypeResponse = await _userService.GetCurrentUserTypeAsync(query.UserID);
            var ForceID = await _userService.GetForceID(query.UserID);
            var BaseID = await _userService.GetBaseID(query.UserID);
            var BaseSectionID = await _userService.GetBaseSectionID(query.UserID);
            var userType = userTypeResponse.Data;
            Expression<Func<Models.Bases, bool>> BaseCreteria = x => x.Id != 0;
            Expression<Func<Models.Forces, bool>> ForceCreteria = x => x.Id != 0;
            Expression<Func<Models.BasesSections, bool>> BaseSectionCreteria = x => x.Id != 0;
            Expression<Func<Models.DepoDepartment, bool>> DepoCreteria = x => x.Id != 0;
            Expression<Func<Models.HQDepartment, bool>> HQCreteria = x => x.Id != 0;

            if (ForceID.HasValue)
            {
                ForceCreteria = ForceCreteria.And(x => x.Id == ForceID.Value);
                BaseCreteria = BaseCreteria.And(x => x.ForceId == ForceID.Value);
                BaseSectionCreteria = BaseSectionCreteria.And(x => x.Base.ForceId == ForceID.Value);
                DepoCreteria = DepoCreteria.And(x => x.ForceID == ForceID.Value);
                HQCreteria = HQCreteria.And(x => x.ForceID == ForceID.Value);
            }
            if (BaseID.HasValue)
            {
                BaseCreteria = BaseCreteria.And(x => x.Id == BaseID.Value);
                BaseSectionCreteria = BaseSectionCreteria.And(x => x.BaseId == BaseID.Value);
            }
            if (BaseSectionID.HasValue)
            {
                BaseSectionCreteria = BaseSectionCreteria.And(x => x.Id == BaseSectionID.Value);
            }
            var response = new DashboardDataResponse
            {
                ProductCount = await _unitOfWork.Repository<Product>().Entities.CountAsync(cancellationToken),
                BrandCount = await _unitOfWork.Repository<Brand>().Entities.CountAsync(cancellationToken),
                DocumentCount = await _unitOfWork.Repository<Document>().Entities.CountAsync(cancellationToken),
                DocumentTypeCount = await _unitOfWork.Repository<DocumentType>().Entities.CountAsync(cancellationToken),
                DocumentExtendedAttributeCount = await _unitOfWork.Repository<DocumentExtendedAttribute>().Entities.CountAsync(cancellationToken),
                UserCount = await _userService.GetCountAsync(),
                RoleCount = await _roleService.GetCountAsync(),
                BasesCount = await _unitOfWork.Repository<Models.Bases>().Entities.Where(BaseCreteria).CountAsync(cancellationToken),
                ForcesCount = await _unitOfWork.Repository<Models.Forces>().Entities.Where(ForceCreteria).CountAsync(cancellationToken),
                BasesectionsCount = await _unitOfWork.Repository<Models.BasesSections>().Entities.Include(x => x.Base).Where(BaseSectionCreteria).CountAsync(cancellationToken),
                ItemsCount = await _unitOfWork.Repository<Models.Items>().Entities.CountAsync(cancellationToken),
                MeasureUnitsCount = await _unitOfWork.Repository<Models.MeasureUnits>().Entities.CountAsync(cancellationToken),
                VoteCodesCount = await _voteCodeService.GetVoteCodeCountAsync(),
                VoteCodeUsersCount = await _voteCodeService.GetVoteCodeUsersCountAsync(),
                DepoCount = await _unitOfWork.Repository<Models.DepoDepartment>().Entities.Where(DepoCreteria).CountAsync(cancellationToken),
                HQCount = await _unitOfWork.Repository<Models.HQDepartment>().Entities.Where(HQCreteria).CountAsync(cancellationToken),

            };

            var selectedYear = DateTime.Now.Year;
            double[] productsFigure = new double[13];
            double[] brandsFigure = new double[13];
            double[] documentsFigure = new double[13];
            double[] documentTypesFigure = new double[13];
            double[] documentExtendedAttributesFigure = new double[13];
            double[] ForcesFigure = new double[13];
            double[] BasesFigure = new double[13];
            double[] BasesSectionsFigure = new double[13];
            for (int i = 1; i <= 12; i++)
            {
                var month = i;
                var filterStartDate = new DateTime(selectedYear, month, 01);
                var filterEndDate = new DateTime(selectedYear, month, DateTime.DaysInMonth(selectedYear, month), 23, 59, 59); // Monthly Based

                productsFigure[i - 1] = await _unitOfWork.Repository<Product>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                brandsFigure[i - 1] = await _unitOfWork.Repository<Brand>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentsFigure[i - 1] = await _unitOfWork.Repository<Document>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentTypesFigure[i - 1] = await _unitOfWork.Repository<DocumentType>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentExtendedAttributesFigure[i - 1] = await _unitOfWork.Repository<DocumentExtendedAttribute>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                ForcesFigure[i - 1] = await _unitOfWork.Repository<Models.Forces>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                BasesFigure[i - 1] = await _unitOfWork.Repository<Models.Bases>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                BasesSectionsFigure[i - 1] = await _unitOfWork.Repository<Models.BasesSections>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
            }

            //  response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Products"], Data = productsFigure });
            // response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Brands"], Data = brandsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Documents"], Data = documentsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Document Types"], Data = documentTypesFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Forces"], Data = ForcesFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Bases"], Data = BasesFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Base Sections"], Data = BasesSectionsFigure });
            //  response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Document Extended Attributes"], Data = documentExtendedAttributesFigure });

            return await Result<DashboardDataResponse>.SuccessAsync(response);
        }
    }
}