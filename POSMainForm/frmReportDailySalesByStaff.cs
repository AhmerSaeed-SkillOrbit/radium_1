using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;

namespace POSMainForm
{
    public partial class frmReportDailySalesByStaff : Form
    {
        DateTime _reportDate;

        public frmReportDailySalesByStaff(DateTime reportDate)
        {
            _reportDate = reportDate;

            InitializeComponent();
        }

        private void frmReportDailySalesByStaff_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
            LoadReport();
        }

        private void LoadReport()
        {
            try
            {
                SQLConn.sqL = "SELECT `Staff`, `InvoiceNo`, `TDate`, `TotalAmount`, `Customer` FROM `transactions` WHERE TDate = '" + _reportDate.ToString("MM/dd/yyyy") + "' Order By Staff asc";

                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.da = new MySqlDataAdapter(SQLConn.cmd);

                this.dataSet1.DailyInv.Clear();
                SQLConn.da.Fill(this.dataSet1.DailyInv);

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

        private void frmReportDailySalesByStaff_Load_1(object sender, EventArgs e)
        {

        }
    }
}
