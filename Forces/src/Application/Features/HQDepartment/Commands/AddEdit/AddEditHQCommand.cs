using AutoMapper;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.HQDepartment.Commands.AddEdit
{
    public class AddEditHQCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int ForceID { get; set; }
    }
    internal class AddEditHQCommandHandler : IRequestHandler<AddEditHQCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditHQCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditHQCommandHandler(IMapper mapper, IStringLocalizer<AddEditHQCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _mapper = mapper;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AddEditHQCommand Command, CancellationToken cancellationToken)
        {
            if (Command.Id == 0)
            {
                var HQ = _mapper.Map<Application.Models.HQDepartment>(Command);
                var IsNameExist = _unitOfWork.Repository<Application.Models.HQDepartment>().Entities.Any(x => x.Name == HQ.Name && x.ForceID == Command.ForceID);
                if (IsNameExist)
                {
                    return await Result<int>.FailAsync(_localizer["Department With This Name is Already Exist!"]);
                }

                await _unitOfWork.Repository<Application.Models.HQDepartment>().AddAsync(HQ);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(HQ.Id, _localizer["Department Added!"]);
            }
            else
            {
                var HQ = await _unitOfWork.Repository<Application.Models.HQDepartment>().GetByIdAsync(Command.Id);
                if (HQ != null)
                {
                    HQ.Name = Command.Name ?? HQ.Name;
                    var IsNameExist = _unitOfWork.Repository<Application.Models.HQDepartment>().Entities.Any(x => x.Name == HQ.Name && x.ForceID == Command.ForceID && x.Id != HQ.Id);
                    if (IsNameExist) return await Result<int>.FailAsync(_localizer["Department With This Name is Already Exist!"]);
                    else
                    {
                        await _unitOfWork.Repository<Application.Models.HQDepartment>().UpdateAsync(HQ);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(HQ.Id, _localizer["Department Updated"]);
                    }

                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Department Not Found!"]);
                }
            }
        }
    }
}
