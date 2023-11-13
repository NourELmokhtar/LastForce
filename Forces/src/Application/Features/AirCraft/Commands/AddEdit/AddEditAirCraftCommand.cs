using AutoMapper;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.AirCraft.Commands.AddEdit
{
    public class AddEditAirCraftCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int AirCraftCode { get; set; }
        [Required]
        public string AirCraftName { get; set; }
        [Required]
        public int BaseId { get; set; }
        public int SectionId { get; set; }
        public DateTime? DateOfEnter { get; set; }
        [Required]
        public string MadeIN { get; set; }
        public DateTime? LastServes { get; set; }
        public int? Hours { get; set; }
        public int ServesType { get; set; }
        public int AirKindId { get; set; }
        
    }
    internal class AddEditAirCraftCommandHandler : IRequestHandler<AddEditAirCraftCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditAirCraftCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAirCraftRepository _repository;

        public AddEditAirCraftCommandHandler(IMapper mapper, IStringLocalizer<AddEditAirCraftCommandHandler> localizer, IUnitOfWork<int> unitOfWork, IAirCraftRepository repository)
        {
            _mapper = mapper;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Result<int>> Handle(AddEditAirCraftCommand Command, CancellationToken cancellationToken)
        {
            var AirCraft = _mapper.Map<Models.AirCraft>(Command);
            if (Command.Id == 0)
            {
               
                if (await _repository.IsNameExist(Command.AirCraftName, Command.AirKindId))
                {
                    return await Result<int>.FailAsync(_localizer["Air Craft Name is Already Exist In This AirKind!"]);
                }

                if (await _repository.IsCodeExist(Command.AirCraftCode))
                {
                    return await Result<int>.FailAsync(_localizer["Air Craft With This Code is Already Exist!"]);
                }
                await _unitOfWork.Repository<Models.AirCraft>().AddAsync(AirCraft);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(AirCraft.Id, _localizer["Air Craft Added!"]);
            }
            else
            {
                var isNameExist = await _unitOfWork.Repository<Models.AirCraft>().Entities.AnyAsync(x => x.AirCraftName == Command.AirCraftName
                && x.Id != Command.Id);
                var isCodeExist = await _unitOfWork.Repository<Models.AirCraft>().Entities.AnyAsync(x => x.AirCraftCode == Command.AirCraftCode
                && x.Id != Command.Id);
                if (isNameExist)
                {
                    return await Result<int>.FailAsync(_localizer["Air Craft With this Name is Already Exist!"]);

                }
                if (isCodeExist)
                {
                    return await Result<int>.FailAsync(_localizer["Air Craft With this Code is Already Exist!"]);

                }
                await _unitOfWork.Repository<Models.AirCraft>().UpdateAsync(AirCraft);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(AirCraft.Id, _localizer["Air Craft Updated!"]);
            }
        }
    }
}
