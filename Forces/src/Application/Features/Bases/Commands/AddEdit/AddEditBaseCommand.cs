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

namespace Forces.Application.Features.Bases.Commands.AddEdit
{
    public class AddEditBaseCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string BaseName { get; set; }
        [Required]
        public string BaseCode { get; set; }
        [Required]
        public int ForceId { get; set; }
    }
    internal class AddEditBaseCommandHandler : IRequestHandler<AddEditBaseCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditBaseCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IBaseRepository _repository;

        public AddEditBaseCommandHandler(IMapper mapper, IStringLocalizer<AddEditBaseCommandHandler> localizer, IUnitOfWork<int> unitOfWork, IBaseRepository repository)
        {
            _mapper = mapper;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Result<int>> Handle(AddEditBaseCommand Command, CancellationToken cancellationToken)
        {
            if (Command.Id == 0)
            {
                var Base = _mapper.Map<Models.Bases>(Command);

                if (await _repository.IsNameExist(Command.BaseName, Command.ForceId))
                {
                    return await Result<int>.FailAsync(_localizer["Base Name is Already Exist In This Force!"]);
                }

                if (await _repository.IsCodeExist(Command.BaseCode))
                {
                    return await Result<int>.FailAsync(_localizer["Base With This Code is Already Exist!"]);
                }
                await _unitOfWork.Repository<Models.Bases>().AddAsync(Base);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Base.Id, _localizer["Base Added!"]);
            }
            else
            {
                var Base = await _unitOfWork.Repository<Models.Bases>().GetByIdAsync(Command.Id);
                if (Base != null)
                {
                    Base.BaseName = Command.BaseName ?? Base.BaseName;
                    Base.BaseCode = Command.BaseCode ?? Base.BaseCode;
                    var Messages = new List<string>();
                    var IsNameExist = _unitOfWork.Repository<Models.Bases>().Entities.Any(x => x.BaseName == Base.BaseName && x.Id != Base.Id && x.ForceId == Base.ForceId);
                    var IsCodeExist = _unitOfWork.Repository<Models.Bases>().Entities.Any(x => x.BaseCode == Base.BaseCode && x.Id != Base.Id);
                    if (IsNameExist) Messages.Add(_localizer["Base With This Name is Already Exist!"]);
                    if (IsCodeExist) Messages.Add(_localizer["Base With This Code is Already Exist!"]);
                    if (IsNameExist || IsCodeExist) return await Result<int>.FailAsync(Messages);
                    else
                    {
                        await _unitOfWork.Repository<Models.Bases>().UpdateAsync(Base);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(Base.Id, _localizer["Base Updated"]);
                    }

                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Base Not Found!"]);
                }
            }
        }
    }
}
