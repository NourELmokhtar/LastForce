
using Forces.Domain.Contracts;
using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forces.Application.Models
{

    public class Demand : AuditableEntity<int>
    {

        public int Id { get; set; }
        [Required]
        public int DemandSequence { get; set; }
        public string DemandNo { get; set; }
        public string PartNo { get; set; }
        [Required]
        public int DemandedQty { get; set; }
        [Required]
        public string Priority { get; set; }
        public string Category { get; set; }
        public string Consignee { get; set; }
        public string SpecialInstructions { get; set; }
        public string Description { get; set; }
        public string RAFOReference { get; set; }
        [Required]
        public string DofQ { get; set; }
        public DateTime DateRequired { get; set; }
        //-------
        public string PartName { get; set; }
        public string StationIV { get; set; } // only for form1
        public string VoteCode { get; set; }
        public int PhysicalStockBalance { get; set; }
        public int DuesIn { get; set; }
        public int DuesOut { get; set; }
        public string ClassofStore { get; set; }
        public int IssuedQty { get; set; }
        public string BaseId { get; set; }
        public string SectionId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public string SectionDemandState { get; set; }
        public string SupplyDemandState { get; set; }
        public string DemandState { get; set; }
        public bool Custody { get; set; }
        public string VoucherNo { get; set; }
        public string FullIPCdetail { get; set; }
        public string DetailedPFWR { get; set; }
        public string Sourceofsupply { get; set; }
        public bool IsitemRFNT { get; set; }
        public string IfNoGDOPIUARFC { get; set; }
        public string EstimatedFRPA { get; set; }
        public int SupplySectionId { get; set; }
        //---------------
        public string SerialNumber1 { get; set; }
        public string Recurring { get; set; }
        public int QtyReqdRetd { get; set; }
        public int QtyIssuedRetd { get; set; } //23/8/2021
        public string Condition { get; set; }
        public string SerialNumber2 { get; set; }
        public string StockRecord { get; set; }
        public string FurtherAccount { get; set; }
        public string FinalScrutiny { get; set; }
        public string Location { get; set; }
        public string MajorEquipment { get; set; }
        public string Purposeforwhichrequired { get; set; }
        public string PublicationDetails { get; set; }
        public string InventoryNO { get; set; }
        public string NameOfHolder { get; set; }
        public string DemandedofReturnedby { get; set; }
        public string CompetentSection { get; set; }
        public string UpdateUrl { get; set; }
        public string DemandType { get; set; }
        public bool OCSupplyApprovalExternalDemand { get; set; } //new 8/9/2021
        public bool ICSupplySendApprovalExternalDemand { get; set; } //new 8/13/2021
        public int ICSupplyApprovalQty { get; set; } //new 8/9/2021
        public bool EmpSupplyDemandProcessing { get; set; } //new 8/9/2021
        public DateTime DateofUserSectionLastMovement { get; set; } //new 8/9/2021
        public bool HaveStock { get; set; } //new 8/9/2021
        public bool ICSupplyApprovalToIssue { get; set; }  //new 8/11/2021
        public string Vocab { get; set; }  //new 8/24/2021
        public string ExternalDemandNo { get; set; }   //new 8/24/2021
        public DateTime ExternalDemandDate { get; set; }   //new 8/24/2021
        public string IssueVoucher { get; set; }   //new 9/3/2021

        [ForeignKey("SectionId")]
        public virtual BaseSection BaseSection { get; set; }
        // public virtual ICollection<DemandAction> DemandAction { get; set; }
    }
}