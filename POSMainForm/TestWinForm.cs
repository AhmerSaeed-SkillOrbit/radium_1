using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSMainForm
{
    public partial class TestWinForm : Form
    {
        public TestWinForm()
        {
            InitializeComponent();
        }

        private void TestWinForm_Load(object sender, EventArgs e)
        {
            // Set the processing mode for the ReportViewer to Remote  
            reportViewer1.ProcessingMode = ProcessingMode.Remote;

            ServerReport serverReport = reportViewer1.ServerReport;

            // Get a reference to the default credentials  
            System.Net.ICredentials credentials =
                System.Net.CredentialCache.DefaultCredentials;

            // Get a reference to the report server credentials  
            ReportServerCredentials rsCredentials =
                serverReport.ReportServerCredentials;

            // Set the credentials for the server report  
            rsCredentials.NetworkCredentials = credentials;

            // Set the report server URL and report path  
            serverReport.ReportServerUrl =
                new Uri("http:// <Server Name>/reportserver");
            serverReport.ReportPath =
                "/AdventureWorks Sample Reports/Sales Order Detail";

            // Create the sales order number report parameter  
            ReportParameter salesOrderNumber = new ReportParameter();
            salesOrderNumber.Name = "SalesOrderNumber";
            salesOrderNumber.Values.Add("SO43661");

            // Set the report parameters for the report  
            reportViewer1.ServerReport.SetParameters(
                new ReportParameter[] { salesOrderNumber });

            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
