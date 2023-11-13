using System.Threading.Tasks;

namespace Forces.Application.Features.Color.Commands.Delete
{
    internal interface IColorRepository
    {
        Task<bool> IsColorInused(int ColorID);
        Task IsColorInuse(object id);
    }
}