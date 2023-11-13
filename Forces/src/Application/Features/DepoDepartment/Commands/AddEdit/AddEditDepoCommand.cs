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

namespace Forces.Application.Features.DepoDepartment.Commands.AddEdit
{
    public class AddEditDepoCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int ForceID { get; set; }

    }
    internal class AddEditDepoCommandHandler : IRequestHandler<AddEditDepoCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditDepoCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditDepoCommandHandler(IMapper mapper, IStringLocalizer<AddEditDepoCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _mapper = mapper;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AddEditDepoCommand Command, CancellationToken cancellationToken)
        {
            if (Command.Id == 0)
            {
                var Depo = _mapper.Map<Application.Models.DepoDepartment>(Command);
                var IsNameExist = _unitOfWork.Repository<Application.Models.DepoDepartment>().Entities.Any(x => x.Name == Depo.Name && x.ForceID == Depo.ForceID);
                if (IsNameExist)
                {
                    return await Result<int>.FailAsync(_localizer["Department With This Name is Already Exist!"]);
                }

                await _unitOfWork.Repository<Application.Models.DepoDepartment>().AddAsync(Depo);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Depo.Id, _localizer["Department Added!"]);
            }
            else
            {
                var Depo = await _unitOfWork.Repository<Application.Models.DepoDepartment>().GetByIdAsync(Command.Id);
                if (Depo != null)
                {
                    Depo.Name = Command.Name ?? Depo.Name;
                    var IsNameExist = _unitOfWork.Repository<Application.Models.DepoDepartment>().Entities.Any(x => x.Name == Depo.Name && x.ForceID == Depo.ForceID && x.Id != Depo.Id);
                    if (IsNameExist) return await Result<int>.FailAsync(_localizer["Department With This Name is Already Exist!"]);
                    else
                    {
                        await _unitOfWork.Repository<Application.Models.DepoDepartment>().UpdateAsync(Depo);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(Depo.Id, _localizer["Department Updated"]);
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
