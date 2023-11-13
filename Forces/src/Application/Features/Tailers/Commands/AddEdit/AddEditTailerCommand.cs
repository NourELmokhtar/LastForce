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

namespace Forces.Application.Features.Tailers.Commands.AddEdit
{
    public class AddEditTailerCommand : IRequest<IResult<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        public int BaseId { get; set; }
        public string TailerCode { get; set; }
    }
    internal class AddEditTailerCommandHandler : IRequestHandler<AddEditTailerCommand, IResult<int>>
    {
        private protected IUnitOfWork<int> _unitOfWork;
        private protected IMapper _mapper;
        private readonly IStringLocalizer<AddEditTailerCommandHandler> _localizer;

        public AddEditTailerCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditTailerCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IResult<int>> Handle(AddEditTailerCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var Tailer = _mapper.Map<Models.Tailers>(command);
                await _unitOfWork.Repository<Models.Tailers>().AddAsync(Tailer);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Tailer.Id, _localizer["Tailer Added!"]);
            }
            else
            {
                var _tailer = _mapper.Map<Models.Tailers>(command);
                var Tailer = await _unitOfWork.Repository<Models.Tailers>().GetByIdAsync(command.Id);
                if (Tailer != null)
                {
                    await _unitOfWork.Repository<Models.Tailers>().UpdateAsync(_tailer);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(Tailer.Id, _localizer["Tailer Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Tailer Not Found!"]);
                }

            }
        }
    }
}
