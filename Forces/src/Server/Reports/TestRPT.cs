using DevExpress.XtraReports.UI;
using Forces.Server.Helper;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Forces.Server.Reports
{
    public partial class TestRPT : DevExpress.XtraReports.UI.XtraReport, IMyReport
    {
        public TestRPT()
        {
            InitializeComponent();
        }
    }
}
