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

namespace Forces.Application.Features.BaseSections.Commands.AddEdit
{
    public class AddEditBaseSectionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string SectionName { get; set; }
        [Required]
        public string SectionCode { get; set; }
        [Required]
        public int BaseId { get; set; }
    }
    internal class AddEditBaseSectionCommandHandler : IRequestHandler<AddEditBaseSectionCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditBaseSectionCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IBaseSectionRepository _repository;

        public AddEditBaseSectionCommandHandler(IMapper mapper, IStringLocalizer<AddEditBaseSectionCommandHandler> localizer, IUnitOfWork<int> unitOfWork, IBaseSectionRepository repository)
        {
            _mapper = mapper;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Result<int>> Handle(AddEditBaseSectionCommand Command, CancellationToken cancellationToken)
        {
            if (Command.Id == 0)
            {
                var BaseSection = _mapper.Map<Models.BasesSections>(Command);

                if (await _repository.IsNameExist(Command.SectionName, Command.BaseId))
                {
                    return await Result<int>.FailAsync(_localizer["Base Section Name is Already Exist In This Force!"]);
                }

                if (await _repository.IsCodeExist(Command.SectionCode))
                {
                    return await Result<int>.FailAsync(_localizer["Base Section With This Code is Already Exist!"]);
                }
                await _unitOfWork.Repository<Models.BasesSections>().AddAsync(BaseSection);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(BaseSection.Id, _localizer["Base Added!"]);
            }
            else
            {
                var BaseSection = await _unitOfWork.Repository<Models.BasesSections>().GetByIdAsync(Command.Id);
                if (BaseSection != null)
                {
                    BaseSection.SectionName = Command.SectionName ?? BaseSection.SectionName;
                    BaseSection.SectionCode = Command.SectionCode ?? BaseSection.SectionCode;
                    var Messages = new List<string>();
                    var IsNameExist = _unitOfWork.Repository<Models.BasesSections>().Entities.Any(x => x.SectionName == BaseSection.SectionName && x.Id != BaseSection.Id && x.BaseId == BaseSection.BaseId);
                    var IsCodeExist = _unitOfWork.Repository<Models.BasesSections>().Entities.Any(x => x.SectionCode == BaseSection.SectionCode && x.Id != BaseSection.Id);
                    if (IsNameExist) Messages.Add(_localizer["Base Section With This Name is Already Exist!"]);
                    if (IsCodeExist) Messages.Add(_localizer["Base Section With This Code is Already Exist!"]);
                    if (IsNameExist || IsCodeExist) return await Result<int>.FailAsync(Messages);
                    else
                    {
                        await _unitOfWork.Repository<Models.BasesSections>().UpdateAsync(BaseSection);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(BaseSection.Id, _localizer["Base Section Updated"]);
                    }

                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Base Section Not Found!"]);
                }
            }
        }
    }
}
