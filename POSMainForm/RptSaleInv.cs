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
    public partial class RptSaleInv : Form
    {
        public RptSaleInv()
        {
            InitializeComponent();
        }

        private void RptSaleInv_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
            LoadReport();

            this.reportViewer1.RefreshReport();
        }

        private void LoadReport()
        {
            try
            {
                //SQLConn.sqL = @"SELECT T.InvoiceNo, ProductCode, P.Description,TDate, TTime,TD.ItemPrice, SUM(TD.Quantity) as totalQuantity, (TD.ItemPrice * SUM(TD.Quantity)) as TotalPrice FROM Transactions";
                SQLConn.sqL = @"SELECT t.Customer,t.InvoiceNo, TD.ProductNo,TD.ItemName,TD.ItemPrice,TD.Quantity,T.TotalAmount FROM transactions as t INNER JOIN transactiondetails as td ON T.InvoiceNo = TD.InvoiceNo WHERE T.InvoiceNo = '1'";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);  
                SQLConn.da = new MySqlDataAdapter(SQLConn.cmd);

                this.DataSet1.DataTable1.Clear();
                SQLConn.da.Fill(this.DataSet1.DataTable1);

                DataTable dt = new DataTable();
                SQLConn.da.Fill(dt);

                ReportParameter invNo = new ReportParameter("lblInvoice", dt.Rows[0][1].ToString());
                ReportParameter customer = new ReportParameter("lblCustomer", dt.Rows[0][0].ToString());
                this.reportViewer1.LocalReport.SetParameters(invNo);
                this.reportViewer1.LocalReport.SetParameters(customer); 

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

        private void DataTable1BindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

    }
}
