using Microsoft.Reporting.WinForms;
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            SQLConn.sqL = "SELECT FirstName as StaffFirstName,Lastname as StaffLastName FROM staff";
            SQLConn.ConnDB();
            SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
            SQLConn.da = new MySqlDataAdapter(SQLConn.cmd);

            this.DataSet4.DataTable1.Clear();
            SQLConn.da.Fill(this.DataSet4.DataTable1);

            this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            this.reportViewer1.ZoomPercent = 90;
            this.reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;

            this.reportViewer1.RefreshReport();
        }
    }
}
