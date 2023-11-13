using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.XtraReports.Services;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.Compatibility.System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web;
using System;
using DevExpress.DataAccess.Sql;
using System.Collections.Generic;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.Web.ReportDesigner.Native.Services;
using System.Reflection;

namespace Forces.Server.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ReportingController : WebDocumentViewerController
    {
        public ReportingController(IWebDocumentViewerMvcControllerService controllerService) : base(controllerService)
        {
        }
    }

    // This controller is required for the Report Designer.
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomReportDesignerController : ReportDesignerController
    {
        public CustomReportDesignerController(IReportDesignerMvcControllerService controllerService) : base(controllerService)
        {
        }
        

        [HttpPost("[action]")]
        public object GetReportDesignerModel(
            [FromForm] string reportUrl,
            [FromForm] ReportDesignerSettingsBase designerModelSettings,
            [FromServices] IReportDesignerClientSideModelGenerator designerClientSideModelGenerator)
        {
            Dictionary<string, object> dataSources = new Dictionary<string, object>();
            SqlDataSource ds = new SqlDataSource("DefaultConnection");
            dataSources.Add("ForcesDb", ds);
           // designerClientSideModelGenerator = new ReportDesignerClientSideModelGenerator(HttpContext.RequestServices);
           
            ReportDesignerModel model;
            if (string.IsNullOrEmpty(reportUrl))
                model = designerClientSideModelGenerator.GetModel(new XtraReport(), dataSources, "/DXXRD", "/DXXRDV", "/DXXQB");
            else
                model = designerClientSideModelGenerator.GetModel(reportUrl, dataSources, "/DXXRD", "/DXXRDV", "/DXXQB");
            model.WizardSettings.EnableSqlDataSource = true;
            model.WizardSettings.EnableObjectDataSource = true;
            model.WizardSettings.EnableFederationDataSource = true;
            model.WizardSettings.EnableJsonDataSource = true;
            model.AllowMDI = true;

            model.Assign(designerModelSettings);
            var modelJsonScript = designerClientSideModelGenerator.GetJsonModelScript(model);
            return Content(modelJsonScript, "application/json");

        }
    }

    // This controller is required for the Report Designer.
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomQueryBuilderController : QueryBuilderController
    {
        public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService) : base(controllerService)
        {
        }
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomReportProvider : IReportProvider
    {
        public XtraReport GetReport(string id, ReportProviderContext context)
        {
            // Parse the string with the report name and parameter values.
            string[] parts = id.Split('?');
            string reportName = parts[0];
            string parametersQueryString = parts.Length > 1 ? parts[1] : String.Empty;

            // Create a report instance.
            XtraReport report = null;


            // Apply the parameter values to the report.
            var parameters = HttpUtility.ParseQueryString(parametersQueryString);

            foreach (string parameterName in parameters.AllKeys)
            {
                report.Parameters[parameterName].Value = Convert.ChangeType(
                    parameters.Get(parameterName), report.Parameters[parameterName].Type);
            }

            // Disable the Visible property for all report parameters
            // to hide the Parameters Panel in the viewer.
            foreach (var parameter in report.Parameters)
            {
                parameter.Visible = false;
            }

            // If you do not hide the panel, disable the report's RequestParameters property.
            // report.RequestParameters = false;

            return report;
        }
    }
}