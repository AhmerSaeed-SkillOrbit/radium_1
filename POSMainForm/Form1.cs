using Microsoft.Reporting.WinForms;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(datasource);
            this.reportViewer1.RefreshReport();

            this.reportViewer1.RefreshReport();
        }

        private void LoadReport()
        {
            try
            {
                //without product unit
                //SQLConn.sqL = "SELECT ProductCode, P.Description, STR_TO_DATE(REPLACE(TDate, '-', '/'), '%m/%d/%Y') as DateOut, SUM(TD.Quantity) as Quantity, TD.ItemPrice as Price, (SUM(TD.Quantity) * TD.ItemPrice) as TotalAmount FROM Product as P, Transactions as T, TransactionDetails as TD WHERE P.ProductNo = TD.ProductNo AND TD.InvoiceNo = T.InvoiceNo AND STR_TO_DATE(REPLACE(TDate, '-', '/'), '%m/%d/%Y') BETWEEN '" + StartDate.ToString("yyyy-MM-dd") + "' AND '" + EndDate.ToString("yyyy-MM-dd") + "' GROUP BY P.ProductNo, TDate ORDER By TDate, Description";

                //with product unit
                //SQLConn.sqL = "SELECT ProductCode, P.Description, STR_TO_DATE(REPLACE(TDate, '-', '/'), '%m/%d/%Y') as DateOut, SUM(TD.Quantity) as Quantity, TD.ItemPrice as Price, (SUM(TD.Quantity) * TD.ItemPrice) as TotalAmount,punit.UnitName as ProductUnit FROM Product as P, Transactions as T, TransactionDetails as TD, ProductUnit as punit WHERE P.ProductNo = TD.ProductNo AND TD.InvoiceNo = T.InvoiceNo AND P.ProductUnitId = punit.Id AND STR_TO_DATE(REPLACE(TDate, '-', '/'), '%m/%d/%Y') BETWEEN '" + StartDate.ToString("yyyy-MM-dd") + "' AND '" + EndDate.ToString("yyyy-MM-dd") + "' GROUP BY P.ProductNo, TDate ORDER By TDate, Description";
                SQLConn.sqL = "SELECT * FROM staff";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.da = new MySqlDataAdapter(SQLConn.cmd);

                this.DataSet3.DataTable1.Clear();
                SQLConn.da.Fill(this.DataSet3.DataTable1);

                //ReportParameter startDate = new ReportParameter("StartDate", StartDate.ToString());
                //ReportParameter endDate = new ReportParameter("EndDate", EndDate.ToString());
                //this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { startDate, endDate });

                this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                this.reportViewer1.ZoomPercent = 90;
                this.reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;

                this.reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
        }
    }
}
