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

namespace Forces.Application.Features.Forces.Commands.AddEdit
{
    public class AddEditForceCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string ForceName { get; set; }
        [Required]
        public string ForceCode { get; set; }
    }
    internal class AddEditForceCommandHandler : IRequestHandler<AddEditForceCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditForceCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditForceCommandHandler(IMapper mapper, IStringLocalizer<AddEditForceCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _mapper = mapper;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AddEditForceCommand Command, CancellationToken cancellationToken)
        {
            if (Command.Id == 0)
            {
                var Force = _mapper.Map<Application.Models.Forces>(Command);
                var IsNameExist = _unitOfWork.Repository<Application.Models.Forces>().Entities.Any(x => x.ForceName == Force.ForceName);
                if (IsNameExist)
                {
                    return await Result<int>.FailAsync(_localizer["Force With This Name is Already Exist!"]);
                }
                var IsCodeExist = _unitOfWork.Repository<Application.Models.Forces>().Entities.Any(x => x.ForceCode == Force.ForceCode);
                if (IsCodeExist)
                {
                    return await Result<int>.FailAsync(_localizer["Force With This Code is Already Exist!"]);
                }
                await _unitOfWork.Repository<Application.Models.Forces>().AddAsync(Force);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Force.Id, _localizer["Force Added!"]);
            }
            else
            {
                var Force = await _unitOfWork.Repository<Application.Models.Forces>().GetByIdAsync(Command.Id);
                if (Force != null)
                {
                    Force.ForceName = Command.ForceName ?? Force.ForceName;
                    Force.ForceCode = Command.ForceCode ?? Force.ForceCode;
                    var Messages = new List<string>();
                    var IsNameExist = _unitOfWork.Repository<Application.Models.Forces>().Entities.Any(x => x.ForceName == Force.ForceName && x.Id != Force.Id);
                    var IsCodeExist = _unitOfWork.Repository<Application.Models.Forces>().Entities.Any(x => x.ForceCode == Force.ForceCode && x.Id != Force.Id);
                    if (IsNameExist) Messages.Add(_localizer["Force With This Name is Already Exist!"]);
                    if (IsCodeExist) Messages.Add(_localizer["Force With This Code is Already Exist!"]);
                    if (IsNameExist || IsCodeExist) return await Result<int>.FailAsync(Messages);
                    else
                    {
                        await _unitOfWork.Repository<Application.Models.Forces>().UpdateAsync(Force);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(Force.Id, _localizer["Force Updated"]);
                    }

                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Force Not Found!"]);
                }
            }
        }
    }
}
