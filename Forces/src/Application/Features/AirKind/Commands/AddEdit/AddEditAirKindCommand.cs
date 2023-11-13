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

namespace Forces.Application.Features.AirKind.Commands.AddEdit
{
   public class AddEditAirKindCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string AirKindName { get; set; }
        [Required]
        public string AirKindCode { get; set; }
        [Required]
        public int AirTypeId { get; set; }

    }
    internal class AddEditAirKindCommandHandler : IRequestHandler<AddEditAirKindCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditAirKindCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAirKindRepository _repository;

        public AddEditAirKindCommandHandler(IMapper mapper, IStringLocalizer<AddEditAirKindCommandHandler> localizer, IUnitOfWork<int> unitOfWork, IAirKindRepository repository)
        {
            _mapper = mapper;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Result<int>> Handle(AddEditAirKindCommand Command, CancellationToken cancellationToken)
        {
            if (Command.Id == 0)
            {
                var AirKind = _mapper.Map<Models.AirKind>(Command);

                if (await _repository.IsNameExist(Command.AirKindName))
                {
                    return await Result<int>.FailAsync(_localizer["AirKind Name is Already Exist In This AirType!"]);
                }

                if (await _repository.IsCodeExist(Command.AirKindCode))
                {
                    return await Result<int>.FailAsync(_localizer["AirKind With This Code is Already Exist!"]);
                }
                await _unitOfWork.Repository<Models.AirKind>().AddAsync(AirKind);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(AirKind.Id, _localizer["AirKind Added!"]);
            }
            else
            {
                var AirKind = await _unitOfWork.Repository<Models.AirKind>().GetByIdAsync(Command.Id);
                if (AirKind != null)
                {
                    AirKind.AirKindName = Command.AirKindName ?? AirKind.AirKindName;
                    AirKind.AirKindCode = Command.AirKindCode ?? AirKind.AirKindCode;
                    AirKind.AirTypeId = Command.AirTypeId;
                    var Messages = new List<string>();
                    var IsNameExist = await _repository.IsNameExist(Command.AirKindName,Command.Id);
                    var IsCodeExist = await _repository.IsCodeExist(Command.AirKindCode, Command.Id);
                    if (IsNameExist) Messages.Add(_localizer["AirKind With This Name is Already Exist!"]);
                    if (IsCodeExist) Messages.Add(_localizer["AirKind With This Code is Already Exist!"]);
                    if (IsNameExist || IsCodeExist) return await Result<int>.FailAsync(Messages);
                    else
                    {
                        await _unitOfWork.Repository<Models.AirKind>().UpdateAsync(AirKind);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(AirKind.Id, _localizer["AirKind Updated"]);
                    }

                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["AirKind Not Found!"]);
                }
            }
        }
    }
}