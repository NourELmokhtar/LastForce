using Forces.Application.Enums;

namespace Forces.Server.ReportModels
{
    public class MprRequestReportModel
    {
        public string RefCode { get; set; }
        public MprSteps Step { get; set; }
    }
}
