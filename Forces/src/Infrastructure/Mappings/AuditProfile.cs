using AutoMapper;
using Forces.Application.Responses.Audit;
using Forces.Infrastructure.Models.Audit;

namespace Forces.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}