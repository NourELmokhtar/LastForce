using System.Collections.Generic;

namespace Forces.Application.Features.Dashboards.Queries.GetData
{
    public class DashboardDataResponse
    {
        public int ProductCount { get; set; }
        public int BrandCount { get; set; }
        public int DocumentCount { get; set; }
        public int DocumentTypeCount { get; set; }
        public int DocumentExtendedAttributeCount { get; set; }
        public int UserCount { get; set; }
        public int RoleCount { get; set; }
        public int ForcesCount { get; set; }
        public int BasesCount { get; set; }
        public int BasesectionsCount { get; set; }
        public int ItemsCount { get; set; }
        public int MeasureUnitsCount { get; set; }
        public int HQCount { get; set; }
        public int DepoCount { get; set; }
        public int VoteCodesCount { get; set; }
        public int VoteCodeUsersCount { get; set; }
        public List<ChartSeries> DataEnterBarChart { get; set; } = new();
        public Dictionary<string, double> ProductByBrandTypePieChart { get; set; }
    }

    public class ChartSeries
    {
        public ChartSeries() { }

        public string Name { get; set; }
        public double[] Data { get; set; }
    }

}