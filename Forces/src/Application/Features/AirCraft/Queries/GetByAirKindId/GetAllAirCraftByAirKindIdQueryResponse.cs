using System;
using System.ComponentModel.DataAnnotations;

namespace Forces.Application.Features.AirCraft.Queries.GetByAirKindId
{
    public class GetAllAirCraftByAirKindIdQueryResponse
    {
        public int AirCraftId { get; set; }
        public int AirCraftCode { get; set; }
        [Required]
        public string AirCraftName { get; set; }
        [Required]
        public int BaseId { get; set; }

        public int SectionId { get; set; }

        public DateTime? DateOfEnter { get; set; }
        [Required]
        public int MadeIN { get; set; }
        [Required]
        public int AirKindId { get; set; }
        public int AirTypeId { get; set; }
        [Required]
        public DateTime? LastServes { get; set; }
        public int Hours { get; set; }
        public string BaseName { get; set; }
        public string SectionName { get; set; }
        public int ServesType { get; set; }
    }
}