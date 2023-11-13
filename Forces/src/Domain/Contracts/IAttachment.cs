using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Domain.Contracts
{
    public interface IAttachment<TRequestModel> : IAttachment
         where TRequestModel : IRequestModel
    {
        public string FileUrl { get; set; }
        public int? RequestID { get; set; }
        [ForeignKey("RequestID")]
        public TRequestModel Request { get; set; }
        public int? ActionId { get; set; }
        public string AttachmentType { get; set; }
    }
    public interface IAttachment
    {
    }
}
