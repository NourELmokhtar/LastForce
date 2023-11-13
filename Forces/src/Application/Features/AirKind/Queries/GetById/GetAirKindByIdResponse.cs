using Forces.Application.Features.AirType.Queries.GetById;

namespace Forces.Application.Features.AirKind.Queries.GetById
{
    public class GetAirKindByIdResponse
    {
        public int AirKindId { get; set; }
        public string AirKindName { get; set; }
        public string AirKindCode { get; set; }
        public int AirTypeId { get; set; }
        public GetAirTypeByIdResponse AirType { get; set; }

    }
}