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
    public partial class FrmViewSales : Form
    {
        Microsoft.Office.Interop.Excel.Application excel;
        Microsoft.Office.Interop.Excel.Workbook excelworkBook;
        Microsoft.Office.Interop.Excel.Worksheet excelSheet;
        Microsoft.Office.Interop.Excel.Range excelCellrange;
        public FrmViewSales()
        {
            InitializeComponent();
        }

        DataTable dt;
        public void LoadProducts(string strSearch)
        {
            try
            {
                SQLConn.sqL = "SELECT `InvoiceNo`, `TDate`, Customer, `TotalAmount` FROM `transactions`";
                SQLConn.ConnDB();
                MySqlDataAdapter adpt = new MySqlDataAdapter(SQLConn.sqL, SQLConn.conn);
                dt = new DataTable();
                adpt.Fill(dt);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
            finally
            {
                SQLConn.cmd.Dispose();
                SQLConn.conn.Close();
            }
        }
        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmViewSales_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        
    }
}
