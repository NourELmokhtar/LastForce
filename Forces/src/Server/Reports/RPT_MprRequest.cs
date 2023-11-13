using DevExpress.XtraReports.UI;
using Forces.Application.Enums;
using Forces.Application.Extensions;
using Forces.Application.Features.MprRequest.Dto.Response;
using Forces.Application.Models;
using Forces.Server.Helper;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Forces.Server.Reports
{
    public partial class Rpt_MPRRequest : DevExpress.XtraReports.UI.XtraReport,IMyReport
    {
        public Rpt_MPRRequest()
        {
            InitializeComponent();
        }

        private void VerticalDetail_BeforePrint(object sender, CancelEventArgs e)
        {

        }

        private void RankResolver_GetValue(object sender, GetValueEventArgs e)
        {
            RequestActions row = e.Row as RequestActions;
            e.Value = ((RankType)row.Rank).ToEnDescriptionString();
        }
    }
}
