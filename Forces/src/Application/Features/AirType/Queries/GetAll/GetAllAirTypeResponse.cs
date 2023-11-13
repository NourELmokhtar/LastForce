using System.ComponentModel.DataAnnotations;

namespace Forces.Application.Features.AirType.Queries.GetAll
{
    public class GetAllAirTypeResponse
    {
        public int Id { get; set; }
        
        public string AirTypeName { get; set; }
       
        public string AirTypeCode { get; set; }
    }
}