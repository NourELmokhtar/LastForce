using Forces.Application.Features.AirType.Queries.GetAll;

namespace Forces.Application.Features.AirKind.Queries.GetAll
{
    public class GetAllAirKindResponse
    {
        public int Id { get; set; }
        public string AirKindName { get; set; }
        public string AirKindCode { get; set; }
        public int AirTypeId { get; set; }
        public string AirTypeName { get; set; }
        // public GetAllAirTypeResponse AirType { get; set; }
        // public List<GetAllSectionsByBaseIdQueryResponse> Sections { get; set; } 
    }
}