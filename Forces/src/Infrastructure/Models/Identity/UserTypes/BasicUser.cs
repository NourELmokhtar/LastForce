using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Models.Identity.UserTypes
{
    public class BasicUser : Appuser
    {
        public string IdNumber { get; set; }
        public int? ForceID { get; set; }
        public int? BaseId { get; set; }
        public int? BaseSectionId { get; set; }
        [ForeignKey("ForceID")]
        public Application.Models.Forces Force { get; set; }
        [ForeignKey("BaseId")]
        public Application.Models.Bases Base { get; set; }
        [ForeignKey("BaseSectionId")]
        public Application.Models.BasesSections BaseSection { get; set; }
    }
}
