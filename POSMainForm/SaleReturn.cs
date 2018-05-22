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
    public partial class SaleReturn : Form
    {
        public SaleReturn(int StaffId)
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnSale_Click(object sender, EventArgs e)
        {

        }

        private void FormPos_Click(object sender, EventArgs e)
        {
            this.Close();
            frmPOS1 pos = new frmPOS1();
            pos.Show();
        }
    }
}
