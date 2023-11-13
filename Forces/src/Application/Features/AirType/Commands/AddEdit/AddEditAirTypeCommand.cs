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

namespace Forces.Application.Features.AirType.Commands.AddEdit
{
    public class AddEditAirTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string AirTypeName { get; set; }
        [Required]
        public string AirTypeCode { get; set; }


        internal class AddEditAirTypeCommandHandler : IRequestHandler<AddEditAirTypeCommand, Result<int>>
        {
            private readonly IMapper _mapper;
            private readonly IStringLocalizer<AddEditAirTypeCommandHandler> _localizer;
            private readonly IUnitOfWork<int> _unitOfWork;

            public AddEditAirTypeCommandHandler(IMapper mapper, IStringLocalizer<AddEditAirTypeCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
            {
                _mapper = mapper;
                _localizer = localizer;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result<int>> Handle(AddEditAirTypeCommand Command, CancellationToken cancellationToken)
            {
                if (Command.Id == 0)
                {
                    var AirType = _mapper.Map<Application.Models.AirType>(Command);
                    var IsNameExist = _unitOfWork.Repository<Application.Models.AirType>().Entities.Any(x => x.AirTypeName == AirType.AirTypeName);
                    if (IsNameExist)
                    {
                        return await Result<int>.FailAsync(_localizer["AirType With This Name is Already Exist!"]);
                    }
                    var IsCodeExist = _unitOfWork.Repository<Application.Models.AirType>().Entities.Any(x => x.AirTypeCode == AirType.AirTypeCode);
                    if (IsCodeExist)
                    {
                        return await Result<int>.FailAsync(_localizer["AirType With This Code is Already Exist!"]);
                    }
                    await _unitOfWork.Repository<Application.Models.AirType>().AddAsync(AirType);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(AirType.Id, _localizer["AirType Added!"]);
                }
                else
                {
                    var AirType = await _unitOfWork.Repository<Application.Models.AirType>().GetByIdAsync(Command.Id);
                    if (AirType != null)
                    {
                        AirType.AirTypeName = Command.AirTypeName ?? AirType.AirTypeName;
                        AirType.AirTypeCode = Command.AirTypeCode ?? AirType.AirTypeCode;
                        var Messages = new List<string>();
                        var IsNameExist = _unitOfWork.Repository<Application.Models.AirType>().Entities.Any(x => x.AirTypeName == AirType.AirTypeName && x.Id != AirType.Id);
                        var IsCodeExist = _unitOfWork.Repository<Application.Models.AirType>().Entities.Any(x => x.AirTypeCode == AirType.AirTypeCode && x.Id != AirType.Id);
                        if (IsNameExist) Messages.Add(_localizer["AirType With This Name is Already Exist!"]);
                        if (IsCodeExist) Messages.Add(_localizer["AirType With This Code is Already Exist!"]);
                        if (IsNameExist || IsCodeExist) return await Result<int>.FailAsync(Messages);
                        else
                        {
                            await _unitOfWork.Repository<Application.Models.AirType>().UpdateAsync(AirType);
                            await _unitOfWork.Commit(cancellationToken);
                            return await Result<int>.SuccessAsync(AirType.Id, _localizer["AirType Updated"]);
                        }
                    }

                    else
                    {
                        return await Result<int>.FailAsync(_localizer["AirType Not Found!"]);
                    }
                }
            }
        }
    }
}