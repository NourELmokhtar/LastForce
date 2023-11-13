using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Forces.Server.Helper
{
    public static class ReportsPathHelper
    {
        internal static IApplicationBuilder InitializeReports(this IApplicationBuilder app, Microsoft.Extensions.Configuration.IConfiguration _configuration)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                DevExpress.Utils.DeserializationSettings.RegisterTrustedAssembly(assembly);
            }

            var FilesPath = Path.Combine(Directory.GetCurrentDirectory(), @"Files");
            var ReportsPath = Path.Combine(FilesPath, "Reports");
            if (!Directory.Exists(ReportsPath))
            {
                Directory.CreateDirectory(ReportsPath);
            }
            var ReportType =  typeof(IMyReport);
            var Reports = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => ReportType.IsAssignableFrom(p) && !p.IsInterface);
            foreach (var rpt in Reports)
            {
                var ReportPath = Path.Combine(ReportsPath, rpt.Name);
 
                    var FullFilePath = Path.Combine(ReportsPath, $"{rpt.Name}.REPX");
                    if (!File.Exists(FullFilePath))
                    {
                    XtraReport report = (XtraReport)Activator.CreateInstance(rpt);
                        report.Name = rpt.Name;
                        report.DisplayName = rpt.Name;
                        report.SaveLayoutToXml(FullFilePath);
                        report.Dispose();
                    }
                
            }
            return app;
        }
    }
}
