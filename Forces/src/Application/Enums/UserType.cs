using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Enums
{
    
    public enum UserType  
    {
        [Description("Regular")] Regular = 0,
        [Description("Regular Admin")] RegularAdmin = 1,
        [Description("OC LOG")] OCLog = 2,
        [Description("OC LOG Admin")] OCLogAdmin ,
        [Description("Department")] Department,
        [Description("Department Admin")] DepartmentAdmin,
        [Description("HQ")] HQ,
        [Description("Depo")] Depot,
        [Description("Vote Code Controller")] VoteHolder,
        [Description("DFINANCE")] DFINANCE  ,
        [Description("Commander")] Commander,
        [Description("OC Commander")] OCCommander,
        [Description("Base Section Admin")] BaseSectionAdmin  ,
        [Description("Base Admin")] BaseAdmin,
        [Description("Force Admin")] ForceAdmin  ,
        [Description("Super Admin")] SuperAdmin  ,
        [Description("ICMT")] ICMT,
        [Description("Driver")] Driver,
        [Description("OC Security")] OCSecurity,
        [Description("WKL Finance")] WKLFinance,
        [Description("OC Finance")] OCFinance,
        [Description("FA Section")] FASection,
        [Description("OC PO depot")] OCPOdepo,
        [Description("OC Log depo")] OCLogdepo,
        [Description("DLOG")] Dlog,
        [Description("DG-ENG&LOG")] DGenlog,
        [Description("IC PO Section")] ICPOSection
            

    }
}
