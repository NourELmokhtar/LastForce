using AutoMapper;
using Forces.Application.Extensions;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Models;
using Forces.Application.Requests.VoteCodes;
using Forces.Application.Responses.VoteCodes;
using Forces.Infrastructure.Contexts;
using Forces.Infrastructure.Models;
using Forces.Infrastructure.Models.Identity;
using Forces.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Services.VoteCodes
{
    public class VoteCodeService : IVoteCodeService
    {
        private readonly UserManager<Appuser> _userManager;
        private readonly IStringLocalizer<VoteCodeService> _localizer;
        private readonly IExcelService _excelService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ForcesDbContext _context;
        private readonly IUnitOfWork<int> _unitOfWork;

        public VoteCodeService(UserManager<Appuser> userManager,
            IStringLocalizer<VoteCodeService> localizer,
            IExcelService excelService,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IUnitOfWork<int> unitOfWork,
            ForcesDbContext context)
        {
            _userManager = userManager;
            _localizer = localizer;
            _excelService = excelService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<int>> AddEditTransaction(AddEditVcodeTransactionRequest request)
        {
            MprRequest req = new();
            if (request.RequestId.HasValue)
            {
                req = _context.MprRequests.FirstOrDefault(x => x.Id == request.RequestId.Value);
                if (request != null)
                {
                    if (req.Paied == true)
                    {
                        return await Result<int>.FailAsync("Request Already Paied!");
                    }
                    if (req.RequestState != Application.Enums.RequestState.Completed)
                    {
                        return await Result<int>.FailAsync("Request Not Complete!");
                    }

                }
                else
                {
                    return await Result<int>.FailAsync("Request Not Found!");
                }
            }
            var AccountCredit = await GetVoteCodeCreditById(request.VoteCodeId);
            if (request.Transactiontype == Application.Enums.TransactionType.Debit && request.TransactionAmount > AccountCredit)
            {
                await Result.FailAsync(_localizer["There is No Enough Credit on This Vote Code"]);
            }
            if (request.LogId == 0)// Add New Transaction
            {
                Items item = new Items();
                if (request.ItemId.HasValue)
                {
                    item = await _unitOfWork.Repository<Items>().GetByIdAsync(request.ItemId.Value);
                }
                if (!string.IsNullOrEmpty(request.ItemNSN))
                {
                    item = await _unitOfWork.Repository<Items>().Entities.FirstOrDefaultAsync(x => x.ItemNsn == request.ItemNSN);
                }
                if (!string.IsNullOrEmpty(request.ItemCode))
                {
                    item = await _unitOfWork.Repository<Items>().Entities.FirstOrDefaultAsync(x => x.ItemCode == request.ItemCode);
                }

                Models.VoteCodeLog log = new VoteCodeLog();
                if (req != null)
                {
                    req.isDone = true;
                    await _unitOfWork.Repository<MprRequest>().UpdateAsync(req);
                }
                log.AmountBefore = AccountCredit;
                log.AmountAfter = request.Transactiontype == Application.Enums.TransactionType.Credit ? AccountCredit + request.TransactionAmount : AccountCredit - request.TransactionAmount;
                log.Note = request.Note;
                log.Reason = request.Reason;
                log.RequestId = request.RequestId;
                log.RequestRefrance = request.RequestRefrance;
                log.Requesttype = request.RequestType;
                log.TransactionAmount = request.TransactionAmount;
                log.TransactionType = request.Transactiontype;
                log.VoteCodeId = request.VoteCodeId;
                log.ItemCode = item?.ItemCode;
                log.ItemId = request.ItemId;
                log.ItemName = item?.ItemName;
                log.ItemNSN = item?.ItemNsn;
                await _unitOfWork.Repository<Models.VoteCodeLog>().AddAsync(log);
                await _unitOfWork.Commit(new System.Threading.CancellationToken());
                return await Result<int>.SuccessAsync(1, _localizer["Transcation Added!"]);
            }
            else // Edit Exist Transaction
            {
                return await Result<int>.FailAsync("Transaction Can not Edit");
            }
        }

        public async Task<IResult> AddEditVoteCode(AddEditVoteCodeRequest Request)
        {
            List<Appuser> HoldingUsers = new List<Appuser>();
            if (Request.Id == 0)
            {
                if (await _unitOfWork.Repository<Models.VoteCodes>().Entities.AnyAsync(x => x.VoteCode == Request.VoteCode))
                {
                    return await Result.FailAsync(string.Format(_localizer["Code {0} is already Exist."], Request.VoteCode));
                }
                if (await _unitOfWork.Repository<Models.VoteCodes>().Entities.AnyAsync(x => x.VoteShortcut == Request.VoteShortcut))
                {
                    return await Result.FailAsync(string.Format(_localizer["Code Caption {0} is already Exist."], Request.VoteShortcut));
                }
                if (Request.Holders != null)
                {
                    var AllUsers = await _context.Users.ToListAsync();
                    foreach (var TargetUser in Request.Holders)
                    {
                        var user = AllUsers.FirstOrDefault(x => x.Id == TargetUser);
                        if (user != null)
                        {
                            HoldingUsers.Add(user);
                        }
                    }
                }

                Models.VoteCodes Vcode = new Models.VoteCodes()
                {
                    ForceID = Request.ForceId,
                    VoteCode = Request.VoteCode,
                    VoteShortcut = Request.VoteShortcut,
                    CreditAmount = Request.CreditAmount,


                };
                Vcode.Logs = new List<VoteCodeLog>();
                var log = new VoteCodeLog();
                log.AmountAfter = Request.CreditAmount;
                log.AmountBefore = decimal.Zero;
                log.Note = "Amount Of Opening balance";
                log.Reason = "Amount Of Opening balance";
                log.TransactionAmount = Request.CreditAmount;
                log.TransactionType = Application.Enums.TransactionType.Credit;
                log.VoteCodeId = Vcode.Id;
                Vcode.Logs.Add(log);
                if (!string.IsNullOrEmpty(Request.DfaultHolderId))
                {
                    Vcode.DefaultHolderUserId = Request.DfaultHolderId;
                }
                if (HoldingUsers.Count > 0)
                {
                    Vcode.VoteCodeHolders = HoldingUsers;
                }

                try
                {

                    await _unitOfWork.Repository<Models.VoteCodes>().AddAsync(Vcode);
                    await _unitOfWork.Commit(new System.Threading.CancellationToken());

                    return await Result<int>.SuccessAsync(Vcode.Id, string.Format(_localizer["Vote Code {0} Added."], Vcode.VoteCode));
                }
                catch (Exception ex)
                {
                    await _unitOfWork.Rollback();
                    return await Result.FailAsync(string.Format(_localizer["Somthing Went Error: {0}"], ex.Message));
                }

            }
            else
            {

                var VoteCode = await _unitOfWork.Repository<Models.VoteCodes>().Entities.Include(x => x.VoteCodeHolders).FirstOrDefaultAsync(x => x.Id == Request.Id);
                var ExistHoldingUsers = VoteCode.VoteCodeHolders.Select(x => x.Id).ToList();
                if (VoteCode != null)
                {
                    if (VoteCode.VoteCode != Request.VoteCode)
                    {
                        if (await _unitOfWork.Repository<Models.VoteCodes>().Entities.AnyAsync(x => x.VoteCode == Request.VoteCode))
                        {
                            return await Result.FailAsync(string.Format(_localizer["Code {0} is already Exist."], Request.VoteCode));
                        }
                        VoteCode.VoteCode = Request.VoteCode;
                    }
                    if (VoteCode.VoteShortcut != Request.VoteShortcut)
                    {
                        if (await _unitOfWork.Repository<Models.VoteCodes>().Entities.AnyAsync(x => x.VoteShortcut == Request.VoteShortcut))
                        {
                            return await Result.FailAsync(string.Format(_localizer["Code Caption {0} is already Exist."], Request.VoteShortcut));
                        }
                        VoteCode.VoteShortcut = Request.VoteShortcut;
                    }

                    if (Request.Holders != null)
                    {
                        if (string.IsNullOrEmpty(Request.DfaultHolderId))
                        {
                            return await Result.FailAsync(string.Format(_localizer["Please Supply A Default Vote Controller First"]));
                        }
                        var SameUsers = ExistHoldingUsers.Intersect(Request.Holders).ToList();
                        var Changes = ExistHoldingUsers.Except(SameUsers).ToList();
                        if (Changes.Count > 0 || (ExistHoldingUsers.Count == 0 && Request.Holders.Count != 0))
                        {
                            var AllUsers = await _context.Users.ToListAsync();
                            VoteCode.VoteCodeHolders.Clear();
                            foreach (var TargetUser in Request.Holders)
                            {
                                var user = AllUsers.FirstOrDefault(x => x.Id == TargetUser);
                                if (user != null)
                                {
                                    HoldingUsers.Add(user);
                                }
                            }
                            VoteCode.VoteCodeHolders = HoldingUsers;
                        }
                    }

                    VoteCode.DefaultHolderUserId = Request.DfaultHolderId;
                    VoteCode.ForceID = Request.ForceId;
                    try
                    {
                        await _unitOfWork.Repository<Models.VoteCodes>().UpdateAsync(VoteCode);
                        await _unitOfWork.Commit(new System.Threading.CancellationToken());

                        return await Result<int>.SuccessAsync(VoteCode.Id, string.Format(_localizer["Vote Code {0} Updated."], VoteCode.VoteCode));
                    }
                    catch (Exception ex)
                    {
                        await _unitOfWork.Rollback();
                        return await Result.FailAsync(string.Format(_localizer["Somthing Went Error: {0}"], ex.Message));
                    }
                }
                return await Result.FailAsync(string.Format(_localizer["Vote Code Not Found"]));
            }

        }
        public async Task<IResult<List<VoteCodeResponse>>> GetAllCodes()
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            var ForceID = user?.ForceID;
            List<Models.VoteCodes> CodesList = new List<Models.VoteCodes>();
            List<VoteCodeResponse> MappedCodeList = new List<VoteCodeResponse>();
            if (ForceID.HasValue)
            {
                CodesList = await _context.VoteCodes.Include(x => x.DefaultHolder).Include(x => x.Logs).ThenInclude(x => x.User).Include(x => x.VoteCodeHolders).Where(x => x.ForceID == ForceID.Value).ToListAsync();
            }
            else
            {
                CodesList = await _context.VoteCodes.Include(x => x.Logs).ThenInclude(x => x.User).Include(x => x.DefaultHolder).Include(x => x.VoteCodeHolders).ToListAsync();
            }
            MappedCodeList = CodesList.Select
                (x => new VoteCodeResponse()
                {
                    DfaultHolderId = x.DefaultHolderUserId,
                    ForceId = x.ForceID,
                    Id = x.Id,
                    UserName = x.DefaultHolder?.UserName,
                    VoteCode = x.VoteCode,
                    VoteShortcut = x.VoteShortcut,
                    Holders = x.VoteCodeHolders.Select
               (y => new VoteCodeControllersResponse()
               {
                   UserId = y.Id,
                   UserName = y.UserName,
                   VoteCodeId = x.Id
               }
                   ).ToList(),
                    Logs = x.Logs.Select(l => new VoteCodeLogResponse()
                    {
                        LogId = l.Id,
                        AmountAfter = l.AmountAfter,
                        AmountBefore = l.AmountBefore,
                        TransactionAmount = l.TransactionAmount,
                        Note = l.Note,
                        Reason = l.Reason,
                        RequestId = l.RequestId,
                        RequestType = l.Requesttype,
                        TransactionBy = l.User?.UserName,
                        TransactionDate = l.CreatedOn,
                        Transactiontype = l.TransactionType,
                        VoteCodeId = l.VoteCodeId,
                        ItemCode = l.ItemCode,
                        ItemNSN = l.ItemNSN,
                        ItemName = l.ItemName,
                        RequestRefrance = l.RequestRefrance,
                        UserId = l.CreatedBy
                    }).OrderByDescending(x => x.TransactionDate).ToList()
                }).ToList();
            return await Result<List<VoteCodeResponse>>.SuccessAsync(MappedCodeList);
        }

        public async Task<VoteCodeResponse> GetCodeBy(string Code)
        {
            var vCode = await _context.VoteCodes.Include(x => x.VoteCodeHolders).FirstOrDefaultAsync(x => x.VoteCode == Code);
            return new VoteCodeResponse()
            {
                DfaultHolderId = vCode.DefaultHolderUserId,
                ForceId = vCode.ForceID,
                Id = vCode.Id,
                UserName = vCode.DefaultHolder?.UserName,
                VoteCode = vCode.VoteCode,
                VoteShortcut = vCode.VoteShortcut,
                Holders = vCode.VoteCodeHolders.Select
               (y => new VoteCodeControllersResponse()
               {
                   UserId = y.Id,
                   UserName = y.UserName,
                   VoteCodeId = vCode.Id
               }).ToList()
            };
        }

        public async Task<IResult<VoteCodeResponse>> RGetCodeBy(int Id)
        {
            var vCode = await _context.VoteCodes.Include(x => x.Logs).ThenInclude(x => x.User).Include(x => x.VoteCodeHolders).FirstOrDefaultAsync(x => x.Id == Id);
            var mapped = new VoteCodeResponse()
            {
                DfaultHolderId = vCode.DefaultHolderUserId,
                ForceId = vCode.ForceID,
                Id = vCode.Id,
                UserName = vCode.DefaultHolder?.UserName,
                VoteCode = vCode.VoteCode,
                VoteShortcut = vCode.VoteShortcut,
                Cridet = vCode.Logs.Where(x => x.TransactionType == Application.Enums.TransactionType.Credit).Sum(x => x.TransactionAmount) - vCode.Logs.Where(x => x.TransactionType == Application.Enums.TransactionType.Debit).Sum(x => x.TransactionAmount),

                Holders = vCode.VoteCodeHolders.Select
               (y => new VoteCodeControllersResponse()
               {
                   UserId = y.Id,
                   UserName = y.UserName,
                   VoteCodeId = vCode.Id
               }).ToList(),
                Logs = vCode.Logs.Select(l => new VoteCodeLogResponse()
                {
                    LogId = l.Id,
                    AmountAfter = l.AmountAfter,
                    AmountBefore = l.AmountBefore,
                    TransactionAmount = l.TransactionAmount,
                    Note = l.Note,
                    Reason = l.Reason,
                    RequestId = l.RequestId,
                    RequestType = l.Requesttype,
                    TransactionBy = l.User.UserName,
                    TransactionDate = l.CreatedOn,
                    Transactiontype = l.TransactionType,
                    VoteCodeId = l.VoteCodeId,
                    ItemCode = l.ItemCode,
                    ItemNSN = l.ItemNSN,
                    ItemName = l.ItemName,
                    RequestRefrance = l.RequestRefrance,
                    UserId = l.CreatedBy
                }).OrderByDescending(x => x.TransactionDate).ToList()
            };
            return await Result<VoteCodeResponse>.SuccessAsync(mapped);
        }
        public async Task<VoteCodeResponse> GetCodeBy(int Id)
        {
            var vCode = await _context.VoteCodes.Include(x => x.DefaultHolder).Include(x => x.Logs).ThenInclude(x => x.User).Include(x => x.VoteCodeHolders).FirstOrDefaultAsync(x => x.Id == Id);
            var mapped = new VoteCodeResponse()
            {
                DfaultHolderId = vCode.DefaultHolderUserId,
                ForceId = vCode.ForceID,
                Id = vCode.Id,
                UserName = vCode.DefaultHolder?.UserName,
                VoteCode = vCode.VoteCode,
                VoteShortcut = vCode.VoteShortcut,
                Cridet = vCode.Logs.Where(x => x.TransactionType == Application.Enums.TransactionType.Credit).Sum(x => x.TransactionAmount) - vCode.Logs.Where(x => x.TransactionType == Application.Enums.TransactionType.Debit).Sum(x => x.TransactionAmount),

                Holders = vCode.VoteCodeHolders.Select
               (y => new VoteCodeControllersResponse()
               {
                   UserId = y.Id,
                   UserName = y.UserName,
                   VoteCodeId = vCode.Id
               }).ToList(),
                Logs = vCode.Logs.Select(l => new VoteCodeLogResponse()
                {
                    LogId = l.Id,
                    AmountAfter = l.AmountAfter,
                    AmountBefore = l.AmountBefore,
                    TransactionAmount = l.TransactionAmount,
                    Note = l.Note,
                    Reason = l.Reason,
                    RequestId = l.RequestId,
                    RequestType = l.Requesttype,
                    TransactionBy = l.User.UserName,
                    TransactionDate = l.CreatedOn,
                    Transactiontype = l.TransactionType,
                    VoteCodeId = l.VoteCodeId,
                    ItemCode = l.ItemCode,
                    ItemNSN = l.ItemNSN,
                    ItemName = l.ItemName,
                    RequestRefrance = l.RequestRefrance,
                    UserId = l.CreatedBy
                }).OrderByDescending(x => x.TransactionDate).ToList()
            };
            return mapped;
        }

        public async Task<IResult<List<VoteCodeResponse>>> GetCodesByUserId(string userId)
        {
            var user = await _context.VoteCodes.Include(x => x.Requests).Include(x => x.DefaultHolder).Include(x => x.Logs).Where(x => x.VoteCodeHolders.Any(y => y.Id == userId)).ToListAsync();
            var VoteCodes = await _context.VoteCodes.Include(x => x.Requests).Include(x => x.DefaultHolder).Include(x => x.Logs).Where(x => x.DefaultHolderUserId == userId).ToListAsync();
            List<VoteCodeResponse> MappedCodeList = new List<VoteCodeResponse>();
            MappedCodeList = user.Select
                 (x => new VoteCodeResponse()
                 {
                     DfaultHolderId = x.DefaultHolderUserId,
                     ForceId = x.ForceID,
                     Id = x.Id,
                     UserName = x.DefaultHolder?.UserName,
                     VoteCode = x.VoteCode,
                     VoteShortcut = x.VoteShortcut,
                     IsPrimery = false,
                     Cridet = x.Logs.Where(x => x.TransactionType == Application.Enums.TransactionType.Credit).Sum(x => x.TransactionAmount) - x.Logs.Where(x => x.TransactionType == Application.Enums.TransactionType.Debit).Sum(x => x.TransactionAmount),
                     Holders = x.VoteCodeHolders.Select
               (y => new VoteCodeControllersResponse()
               {
                   UserId = y.Id,
                   UserName = y.UserName,
                   VoteCodeId = x.Id
               }
                   ).ToList()
                 }).ToList();
            List<VoteCodeResponse> MappedCodeList2 = new List<VoteCodeResponse>();
            MappedCodeList2 = VoteCodes.Select
                  (x => new VoteCodeResponse()
                  {
                      DfaultHolderId = x.DefaultHolderUserId,
                      ForceId = x.ForceID,
                      Id = x.Id,
                      UserName = x.DefaultHolder?.UserName,
                      VoteCode = x.VoteCode,
                      VoteShortcut = x.VoteShortcut,
                      IsPrimery = true,
                      AllRequestsCount = x.Requests.Where(x => x.RequestState == Application.Enums.RequestState.Pending || x.RequestState == Application.Enums.RequestState.Completed).Count(),
                      CompletedRequestsCount = x.Requests.Where(x => x.RequestState == Application.Enums.RequestState.Completed && x.CurrentStep == Application.Enums.RequestSteps.VoteCodeContreoller).Count(),
                      PendingRequestsCount = x.Requests.Where(x => x.RequestState == Application.Enums.RequestState.Pending && x.CurrentStep == Application.Enums.RequestSteps.VoteCodeContreoller).Count(),
                      Cridet = x.Logs.Where(x => x.TransactionType == Application.Enums.TransactionType.Credit).Sum(x => x.TransactionAmount) - x.Logs.Where(x => x.TransactionType == Application.Enums.TransactionType.Debit).Sum(x => x.TransactionAmount),
                      Holders = x.VoteCodeHolders.Select
               (y => new VoteCodeControllersResponse()
               {
                   UserId = y.Id,
                   UserName = y.UserName,
                   VoteCodeId = x.Id
               }
                   ).ToList()
                  }).ToList();
            var All = MappedCodeList2.Union(MappedCodeList).ToList();
            return await Result<List<VoteCodeResponse>>.SuccessAsync(All);
        }

        public async Task<IResult<VoteCodeLogResponse>> GetLogDetails(int logId)
        {
            var Result = await _unitOfWork.Repository<Models.VoteCodeLog>().GetByIdAsync(logId);
            var Mapped = new VoteCodeLogResponse()
            {
                LogId = Result.Id,
                AmountAfter = Result.AmountAfter,
                AmountBefore = Result.AmountBefore,
                TransactionAmount = Result.TransactionAmount,
                Note = Result.Note,
                Reason = Result.Reason,
                RequestId = Result.RequestId,
                RequestType = Result.Requesttype,
                TransactionBy = Result.User.UserName,
                TransactionDate = Result.CreatedOn,
                Transactiontype = Result.TransactionType,
                VoteCodeId = Result.VoteCodeId,
                ItemCode = Result.ItemCode,
                ItemNSN = Result.ItemNSN,
                ItemName = Result.ItemName,
                RequestRefrance = Result.RequestRefrance,
                UserId = Result.CreatedBy
            };
            return await Result<VoteCodeLogResponse>.SuccessAsync(Mapped);
        }

        public async Task<IResult<List<VoteCodeLogResponse>>> GetvCodeLogs(int VoteCodeId)
        {
            var Result = await _unitOfWork.Repository<Models.VoteCodeLog>().Entities.Where(x => x.VoteCodeId == VoteCodeId).ToListAsync();
            var Mapped = Result.Select(l => new VoteCodeLogResponse()
            {
                LogId = l.Id,
                AmountAfter = l.AmountAfter,
                AmountBefore = l.AmountBefore,
                TransactionAmount = l.TransactionAmount,
                Note = l.Note,
                Reason = l.Reason,
                RequestId = l.RequestId,
                RequestType = l.Requesttype,
                TransactionBy = l.User.UserName,
                TransactionDate = l.CreatedOn,
                Transactiontype = l.TransactionType,
                VoteCodeId = l.VoteCodeId,
                ItemCode = l.ItemCode,
                ItemNSN = l.ItemNSN,
                ItemName = l.ItemName,
                RequestRefrance = l.RequestRefrance,
                UserId = l.CreatedBy
            }).OrderByDescending(x => x.TransactionDate).ToList();
            return await Result<List<VoteCodeLogResponse>>.SuccessAsync(Mapped);
        }

        public async Task<int> GetVoteCodeCountAsync()
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (user.ForceID.HasValue)
            {
                return _context.VoteCodes.Where(x => x.ForceID == user.ForceID.Value).Count();
            }
            else
            {
                return _context.VoteCodes.Count();
            }
        }

        public async Task<IResult<decimal>> GetVoteCodeCredit(int VoteCodeId)
        {
            var Logs = await _unitOfWork.Repository<Models.VoteCodeLog>().Entities.Where(x => x.VoteCodeId == VoteCodeId).ToListAsync();
            var CreditAmount = Logs.Where(x => x.TransactionType == Application.Enums.TransactionType.Credit).Sum(x => x.TransactionAmount);
            var DebitAmount = Logs.Where(x => x.TransactionType == Application.Enums.TransactionType.Debit).Sum(x => x.TransactionAmount);
            var Total = CreditAmount - DebitAmount;
            return await Result<decimal>.SuccessAsync(Total);
        }
        public async Task<decimal> GetVoteCodeCreditById(int VoteCodeId)
        {
            var Logs = await _unitOfWork.Repository<Models.VoteCodeLog>().Entities.Where(x => x.VoteCodeId == VoteCodeId).ToListAsync();
            var CreditAmount = Logs.Where(x => x.TransactionType == Application.Enums.TransactionType.Credit).Sum(x => x.TransactionAmount);
            var DebitAmount = Logs.Where(x => x.TransactionType == Application.Enums.TransactionType.Debit).Sum(x => x.TransactionAmount);
            var Total = CreditAmount - DebitAmount;
            return Total;
        }
        public async Task<int> GetVoteCodeUsersCountAsync()
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (user.ForceID.HasValue)
            {
                return _context.Users.Where(x => x.ForceID == user.ForceID.Value && x.UserType == Application.Enums.UserType.VoteHolder).Count();
            }
            else
            {
                return _context.Users.Where(x => x.UserType == Application.Enums.UserType.VoteHolder).Count();
            }
        }

        public async Task<IResult<List<VoteCodeLogResponse>>> GetLogBySpecification(VoteCodeLogSpecificationRequest reuest)
        {
            Expression<Func<Models.VoteCodeLog, bool>> condition = x => x.VoteCodeId == reuest.VoteCodeId;
            if (reuest.DateFrom != null)
            {
                condition = condition.And(x => x.CreatedOn.Date >= reuest.DateFrom.Value.Date);
            }
            if (reuest.DateTo != null)
            {
                condition = condition.And(x => x.CreatedOn.Date <= reuest.DateTo.Value.Date);
            }
            if (!string.IsNullOrEmpty(reuest.ItemCode))
            {
                condition = condition.And(x => x.ItemCode == reuest.ItemCode);
            }
            if (!string.IsNullOrEmpty(reuest.RequestRef))
            {
                if (reuest.RequestRef.Contains('/'))
                {
                    condition = condition.And(x => x.RequestRefrance == reuest.RequestRef);
                }
                else
                {
                    condition = condition.And(x => x.RequestRefrance.StartsWith($"{reuest.RequestRef}/"));
                }

            }
            if (!string.IsNullOrEmpty(reuest.ItemName))
            {
                condition = condition.And(x => x.ItemName == reuest.ItemName);
            }
            if (!string.IsNullOrEmpty(reuest.UserName))
            {
                condition = condition.And(x => x.User.UserName == reuest.UserName);
            }
            if (!string.IsNullOrEmpty(reuest.ItemNSN))
            {
                condition = condition.And(x => x.ItemNSN == reuest.ItemNSN);
            }
            if (reuest.TransactionType != null)
            {
                condition = condition.And(x => x.TransactionType == reuest.TransactionType);
            }
            if (reuest.Operator != null && reuest.Value != null)
            {
                switch (reuest.Operator)
                {
                    case Application.Enums.MathOperation.GreaterThan:
                        condition = condition.And(x => x.TransactionAmount > reuest.Value);
                        break;
                    case Application.Enums.MathOperation.LessThan:
                        condition = condition.And(x => x.TransactionAmount < reuest.Value);
                        break;
                    case Application.Enums.MathOperation.Equal:
                        condition = condition.And(x => x.TransactionAmount == reuest.Value);
                        break;
                    case Application.Enums.MathOperation.GreaterThanOrEqual:
                        condition = condition.And(x => x.TransactionAmount >= reuest.Value);
                        break;
                    case Application.Enums.MathOperation.LessThanOrEqual:
                        condition = condition.And(x => x.TransactionAmount <= reuest.Value);
                        break;
                }
            }
            var Result = await _unitOfWork.Repository<Models.VoteCodeLog>().Entities.Include(x => x.VoteCode).Include(x => x.User).Where(condition).ToListAsync();
            var Mapped = Result.Select(l => new VoteCodeLogResponse()
            {
                LogId = l.Id,
                AmountAfter = l.AmountAfter,
                AmountBefore = l.AmountBefore,
                TransactionAmount = l.TransactionAmount,
                Note = l.Note,
                Reason = l.Reason,
                RequestId = l.RequestId,
                RequestType = l.Requesttype,
                TransactionBy = l.User.UserName,
                TransactionDate = l.CreatedOn,
                Transactiontype = l.TransactionType,
                VoteCodeId = l.VoteCodeId,
                ItemCode = l.ItemCode,
                ItemNSN = l.ItemNSN,
                ItemName = l.ItemName,
                RequestRefrance = l.RequestRefrance,
                UserId = l.CreatedBy
            }).OrderByDescending(x => x.TransactionDate).ToList();
            return await Result<List<VoteCodeLogResponse>>.SuccessAsync(Mapped);
        }

        public async Task<IResult<string>> ExportLog(List<VoteCodeLogResponse> Data)
        {
            if (Data.Count == 0)
            {
                return await Result<string>.FailAsync("No Data Found");
            }
            var vCode = await _unitOfWork.Repository<Models.VoteCodes>().Entities.FirstOrDefaultAsync(x => x.Id == Data.FirstOrDefault().VoteCodeId);
            var data = await _excelService.ExportAsync(Data, mappers: new Dictionary<string, Func<VoteCodeLogResponse, object>>
            {
                { _localizer["Id"], item => item.LogId },
                { _localizer["Vote Code"], item => vCode.VoteCode },
                { _localizer["Transaction Date"], item => item.TransactionDate.ToString("d/M/yyyy") },
                { _localizer["Transaction Time"], item => item.TransactionDate.ToString("h:mm tt") },
                { _localizer["Short"], item => vCode.VoteShortcut },
                { _localizer["Transaction Type"], item => item.Transactiontype.ToString()},
                { _localizer["Transaction Amount"], item => item.TransactionAmount },
                { _localizer["Before Transaction Amount"], item => item.AmountBefore },
                { _localizer["After Transaction Amount"], item => item.AmountAfter },
                { _localizer["Transaction Reason"], item => item.Reason },
                { _localizer["Transaction Note"], item => item.Note },
                { _localizer["Part Name"], item => item.ItemName },
                { _localizer["Part Code"], item => item.ItemCode },
                { _localizer["Part NSN"], item => item.ItemNSN },
                { _localizer["Request Type"], item => item.RequestType },
                { _localizer["Transaction By"], item => item.TransactionBy },

            }, sheetName: $"{vCode.VoteShortcut} Transactions");

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
