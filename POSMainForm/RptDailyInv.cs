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
    public partial class RptDailyInv : Form
    {

        DateTime ReportDate;
        
        public RptDailyInv(DateTime reportDate)
        {
            InitializeComponent();

            ReportDate = reportDate;
        }

        private void RptDailyInv_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
            LoadReport();
        }


        private void LoadReport()
        {
            try
            {
                //SQLConn.sqL = "SELECT T.InvoiceNo, ProductCode, P.Description,TDate, TTime,TD.ItemPrice, SUM(TD.Quantity) as totalQuantity, (TD.ItemPrice * SUM(TD.Quantity)) as TotalPrice  FROM Product as P, Transactions as T, TransactionDetails as TD WHERE P.ProductNo = TD.ProductNo AND TD.InvoiceNo = T.InvoiceNo AND  TDate = '" + ReportDate.ToString("MM/dd/yyyy") + "' GROUP BY T.InvoiceNo, P.ProductNo, TDate ORDER By TDate" +"";
                //SQLConn.sqL = "SELECT T.invoiceNo ,T.Tdate ,TD.itemName,TD.Quantity,TD.ItemPrice, (TD.ItemPrice*TD.quantity) as TotalAmount ,T.customer FROM Transactions as T, TransactionDetails as TD where T.Tdate = '" + ReportDate.ToString("MM/dd/yyyy") + "'  Order By T.InvoiceNo asc  ";

                SQLConn.sqL = "SELECT `InvoiceNo`, `TDate`, `TotalAmount`, `Customer` FROM `transactions` WHERE TDate = '" + ReportDate.ToString("MM/dd/yyyy") + "' Order By InvoiceNo asc";

                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.da = new MySqlDataAdapter(SQLConn.cmd);

                this.DataSet1.DailyInv.Clear();
                SQLConn.da.Fill(this.DataSet1.DailyInv);

                this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                this.reportViewer1.ZoomPercent = 90;
                this.reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;

                this.reportViewer1.RefreshReport();

                SQLConn.sqL = "";
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
        }

        private void DailyInvBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
