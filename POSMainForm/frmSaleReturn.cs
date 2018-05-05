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
    public partial class frmSaleReturn : Form
    {
        public frmSaleReturn()
        {
            InitializeComponent();
        }


        private void LoadInvoice()
        {
            try
            {
                SQLConn.sqL = "SELECT InvoiceNo,TDate,TotalAmount FROM transactions WHERE InvoiceNo LIKE '" + txtSearch.Text + "%' ORDER BY InvoiceNo ";
                DB d = new DB();
                listBox1.DataSource = d.GetData(SQLConn.sqL);
                listBox1.DisplayMember = "InvoiceNo";
                listBox1.ValueMember = "InvoiceNo";
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                SQLConn.cmd.Dispose();
                SQLConn.conn.Close();
            }
        }

        private void frmSaleReturn_Load(object sender, EventArgs e)
        {
            LoadInvoice();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                frmPOS1.rtrnInv = listBox1.SelectedValue.ToString();
                
            }
            catch (Exception)
            {
                
                //throw;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadInvoice();

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmPOS1.rtrnInv = listBox1.SelectedValue.ToString();
            this.Close();
        }
    }
}
