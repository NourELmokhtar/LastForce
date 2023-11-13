using Forces.Application.Features.Dashboards.Queries.GetData;
using Forces.Shared.Wrapper;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Dashboard
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<DashboardDataResponse>> GetDataAsync();
    }
}