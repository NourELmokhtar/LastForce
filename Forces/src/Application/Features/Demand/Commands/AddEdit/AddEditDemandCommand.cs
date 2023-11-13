using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Demand.Commands.AddEdit
{
    public class AddEditDemandCommand : IRequest<Result<int>>
    {
        public string Id { get; set; }
        public string DemandNo { get; set; }
    }

    internal class AddEditDemandCommandHandler : IRequestHandler<AddEditDemandCommand, Result<int>>
    {

        private readonly IStringLocalizer<AddEditDemandCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditDemandCommandHandler(IStringLocalizer<AddEditDemandCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {

            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public Task<Result<int>> Handle(AddEditDemandCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
