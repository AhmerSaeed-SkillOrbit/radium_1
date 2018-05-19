using Microsoft.Reporting.WinForms;
using Microsoft.VisualBasic;
using MySql.Data;
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
    public partial class RptSaleByStaff : Form
    {
        DateTime ReportDate;

        public RptSaleByStaff(DateTime reportDate)
        {
            InitializeComponent();
            ReportDate = reportDate;
        }

        private void RptSaleByStaff_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
            LoadReport();
        }

        private void LoadReport()
        {
            try
            {
                SQLConn.sqL = "SELECT `Staff`.`Username` as `Staff`, `TDate`, `TotalAmount`, `Customer` FROM `transactions` INNER JOIN Staff ON Staff.StaffID = `transactions`.StaffID WHERE TDate = '" + ReportDate.ToString("MM/dd/yyyy") + "' Order By Staff.StaffID asc";

                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.da = new MySqlDataAdapter(SQLConn.cmd);

                this.DataSet2.DataTable2.Clear();
                SQLConn.da.Fill(this.DataSet2.DataTable2);

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
    }
}
