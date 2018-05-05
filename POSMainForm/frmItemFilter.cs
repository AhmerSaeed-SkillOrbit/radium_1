using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace POSMainForm
{
    public partial class frmItemFilter : Form
    {


        public string returnValue;
        public string returnItem;

        public frmItemFilter()
        {
            InitializeComponent();
        }

        public void LoadProducts(string strSearch)
        {
            try
            {
                SQLConn.sqL = "SELECT ProductNo, ProductCOde FROM Product as P LEFT JOIN Category C ON P.CategoryNo = C.CategoryNo WHERE P.Description LIKE '" + strSearch + "%' ";
                SQLConn.ConnDB();
                DataTable dt = new DataTable();

                MySqlDataAdapter adpt = new MySqlDataAdapter(SQLConn.sqL, SQLConn.conn);
                adpt.Fill(dt);

                listBox1.DataSource = dt;
                listBox1.DisplayMember = "ProductCOde";
                listBox1.ValueMember = "ProductNo";
               
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
              
            }
        }



        private void frmItemFilter_Load(object sender, EventArgs e)
        {
            LoadProducts("");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.returnItem = listBox1.Text;
            this.returnValue = listBox1.SelectedValue.ToString();
            this.Hide();
        }
    }
}
