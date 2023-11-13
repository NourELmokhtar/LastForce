using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.MPRDashboard.Query.GetMPRData
{
    public class GetMPRDashboardDataQuery : IRequest<IResult<GetMPRDashboardDataResponse>>
    {
        public int BaseId { get; set; }
        
    }
   
}
