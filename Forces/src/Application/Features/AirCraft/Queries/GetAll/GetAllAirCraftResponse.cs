using Forces.Application.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forces.Application.Features.AirCraft.Queries.GetAll
{
    public class GetAllAirCraftResponse
    {
        public int AirCraftCode { get; set; }

        public string AirCraftName { get; set; }

        public int BaseId { get; set; }
        public int SectionId { get; set; }
        public DateTime? DateOfEnter { get; set; }
        public string MadeIN { get; set; }
        public DateTime? LastServes { get; set; }
        public int? Hours { get; set; }
        public int ServesType { get; set; }
        public int AirKindId { get; set; }
        public string AirKind { get; set; }
        public string Base { get; set; }
        public string BaseSection { get; set; }
    }
}