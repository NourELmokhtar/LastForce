using Forces.Application.Features.PersonalItemOperations.Queries.Dto;
using Forces.Application.Features.PersonalItems.Queries.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Tailers.Queries
{
    public class TailerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int BaseId { get; set; }
        public string TailerCode { get; set; }
        public List<PersonalItemDto> PersonalItems { get; set; }
        public List<PersonalItemOperationDetail> Operations { get; set; }
    }
}
