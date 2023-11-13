using DevExpress.XtraReports.UI;
using Forces.Server.Helper;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Forces.Server.Reports
{
    public partial class rpt_Requests : DevExpress.XtraReports.UI.XtraReport, IMyReport
    {
        public rpt_Requests()
        {
            InitializeComponent();
        }
    }
}
