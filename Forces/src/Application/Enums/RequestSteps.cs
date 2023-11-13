using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Enums
{
    public enum RequestSteps
    {
        CreationStep = 1,
        FASection = 2, // to do check and Edit
        OCLogStep = 3,
        OCPOdepo = 4,
        OCLogdepo = 5,
        ICPOSection = 6, // for send to section 
        DepartmentStep = 7,
        OCDepartment = 8,
        VoteCodeContreoller = 9,
        OCFinance = 10,
        WKLFinance = 11,
        DFinanceStep = 12,
        CommanderStep = 13
    }
}
