using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Enums
{
    public enum MprSteps
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
        DlogStep = 13,
        DengStep = 14,
        CommanderStep = 15
    }

    /// <summary>
    /// For Printing 
    /// Step Must be Grater than 3 
    /// sigs 1 , 3 
    /// For Step more than 11 
    /// signs 1 ,3 ,9 , 12,13 | 14
    /// </summary>
    public enum Scope
    {
        Force,Base,Section
    }
   
    public enum StepActions
    {
         [Description("Submit Request")] Submit,
         [Description("Esclate Request")] Esclate,
         [Description("Reject Request")] Reject,
         [Description("Redirect Request")] Redirect,
         [Description("Select Qutation")] SelectQutation,
         [Description("Edit Request")] Edit,
         [Description("Cancel Request")] Cancel,
    }
    public enum RedirectAction
    {
       [Description("To User")] ToUser,
        [Description("To User Step")]ToUserType,
       [Description("To Department")] ToDepartment
    }
    public class MprStepTypes
    {
        public MprSteps GetUserStep(UserType userType)
        {
            var Dict = UsersSteps;
            foreach (var step in Dict)
            {
                if (step.Value == userType)
                {
                    return step.Key;
                   
                }
            }
            return MprSteps.CreationStep;
        }
        public Dictionary<UserType, Scope> userTypeScope = new Dictionary<UserType, Scope>()
        {
            {UserType.Regular,Scope.Section },
            {UserType.RegularAdmin,Scope.Section },
            {UserType.OCLog,Scope.Base },
            {UserType.OCLogAdmin,Scope.Base },
            {UserType.Department,Scope.Force },
            {UserType.DepartmentAdmin,Scope.Force },
            {UserType.HQ,Scope.Force },
            {UserType.Depot,Scope.Force },
            {UserType.VoteHolder,Scope.Force },
            {UserType.DFINANCE,Scope.Force },
            {UserType.Commander,Scope.Force },
            {UserType.BaseAdmin,Scope.Base },
            {UserType.ForceAdmin,Scope.Force },
            {UserType.SuperAdmin,Scope.Force },
            {UserType.ICMT,Scope.Base },
            {UserType.Driver,Scope.Section },
            {UserType.OCSecurity,Scope.Base },
            {UserType.WKLFinance,Scope.Force },
            {UserType.OCFinance,Scope.Force },
            {UserType.FASection,Scope.Base },
            {UserType.OCPOdepo,Scope.Force },
            {UserType.OCLogdepo,Scope.Force },
            {UserType.Dlog,Scope.Force },
            {UserType.DGenlog,Scope.Force },
            {UserType.ICPOSection,Scope.Force },
        };
         public  Dictionary<MprSteps, UserType> UsersSteps = new Dictionary<MprSteps, UserType>()
        {
            {MprSteps.CreationStep, UserType.Regular },
            {MprSteps.OCLogStep, UserType.OCLog },
            {MprSteps.FASection, UserType.FASection },
            {MprSteps.OCPOdepo, UserType.OCPOdepo },
            {MprSteps.OCLogdepo, UserType.OCLogdepo },
            {MprSteps.ICPOSection, UserType.ICPOSection },
            {MprSteps.DepartmentStep, UserType.Department },
            {MprSteps.VoteCodeContreoller, UserType.VoteHolder },
            {MprSteps.DlogStep, UserType.Dlog },
            {MprSteps.WKLFinance, UserType.WKLFinance },
            {MprSteps.DFinanceStep, UserType.DFINANCE },
            {MprSteps.DengStep, UserType.DGenlog },
            {MprSteps.OCFinance, UserType.OCFinance },

        };
        public Dictionary<MprSteps, StepActions[]> StepsActions = new Dictionary<MprSteps, StepActions[]>()
        {
             {MprSteps.CreationStep, new StepActions[]{ StepActions.Submit, StepActions.SelectQutation , StepActions.Edit } },
             {MprSteps.FASection, new StepActions[]{ StepActions.Esclate,StepActions.Reject, StepActions.Edit } },
             {MprSteps.OCLogStep, new StepActions[]{StepActions.Redirect, StepActions.Esclate,StepActions.Reject } },
             {MprSteps.OCPOdepo, new StepActions[] { StepActions.Redirect, StepActions.Reject , StepActions.Esclate } },
             {MprSteps.OCLogdepo, new StepActions[] { StepActions.Reject , StepActions.Esclate } },
             {MprSteps.ICPOSection, new StepActions[] { StepActions.Reject , StepActions.Esclate, StepActions.Edit } },
             {MprSteps.DepartmentStep, new StepActions[] { StepActions.Redirect, StepActions.Reject, StepActions.Esclate , StepActions.Edit } },
             {MprSteps.OCDepartment, new StepActions[] { StepActions.Redirect, StepActions.Reject, StepActions.Esclate , StepActions.SelectQutation } },
             {MprSteps.VoteCodeContreoller, new StepActions[]{ StepActions.Redirect, StepActions.Esclate,  StepActions.Submit,StepActions.Reject } },
             {MprSteps.OCFinance, new StepActions[]{ StepActions.Redirect, StepActions.Esclate,StepActions.Reject, StepActions.SelectQutation } },
             {MprSteps.WKLFinance, new StepActions[]{ StepActions.Redirect, StepActions.Esclate,StepActions.Reject , StepActions.Edit } },
             {MprSteps.DFinanceStep, new StepActions[]{StepActions.Redirect, StepActions.Esclate, StepActions.Submit,StepActions.Reject } },
             {MprSteps.DlogStep, new StepActions[]{  StepActions.Submit,StepActions.Reject , StepActions.Edit } },
             {MprSteps.CommanderStep, new StepActions[]{  StepActions.Submit,StepActions.Reject } },

        };
    }
    public static class MprStepTypesExtention
    {
       

        public static T KeyByValue<T, W>(this Dictionary<T, W> dict, W val)
        {
            T key = default;
            W value = default;
           
            foreach (KeyValuePair<T, W> pair in dict)
            {

                if (EqualityComparer<W>.Default.Equals(pair.Value, val))
                {
                    key = pair.Key;
                    break;
                }
            }
            return key;
        }
    }
    public static class StepActionsHelper
    {


    }

}
