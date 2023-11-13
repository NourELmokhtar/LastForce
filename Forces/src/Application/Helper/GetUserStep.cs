using Forces.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Helper
{
    public static class GetUserStep
    {
        public static RequestSteps GetStepByUserType(UserType userType)
        {
            switch (userType)
            {
                case UserType.Regular:
                    return RequestSteps.CreationStep;
                case UserType.RegularAdmin:
                    return RequestSteps.CreationStep;
                case UserType.OCLog:
                    return RequestSteps.OCLogStep;
                case UserType.OCLogAdmin:
                    return RequestSteps.OCLogStep;
                case UserType.Department:
                    return RequestSteps.DepartmentStep;
                case UserType.DepartmentAdmin:
                    return RequestSteps.OCDepartment;
                case UserType.HQ:
                    return RequestSteps.DepartmentStep;
                case UserType.Depot:
                    return RequestSteps.DepartmentStep;
                case UserType.VoteHolder:
                    return RequestSteps.VoteCodeContreoller;
                case UserType.DFINANCE:
                    return RequestSteps.DFinanceStep;
                case UserType.Commander:
                    return RequestSteps.CommanderStep;
                case UserType.BaseSectionAdmin:
                    return RequestSteps.OCLogStep;
                case UserType.BaseAdmin:
                    return RequestSteps.OCLogStep;
                case UserType.ForceAdmin:
                    return RequestSteps.OCLogStep;
                case UserType.SuperAdmin:
                    return RequestSteps.CreationStep;
                default:
                    return RequestSteps.CreationStep;
            }
        }
    }
}
