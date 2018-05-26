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
using System.Drawing.Printing;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

namespace POSMainForm
{
    public partial class SaleReturn : Form
    {
        int _id;
        public float ProductQty;
        public float ProductUnitPrice;
        public float productTotalAmount; // qty * product unit price
        public float totalPaymentDue;
        public float totalAmountPaid;

        public SaleReturn(int id)
        {
            InitializeComponent();
            _id = id;
        }

        private void SaleReturn_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DB d = new DB();

            try
            {
                //GetCatFilter();
                SQLConn.sqL = "SELECT `StaffID`, `Firstname`, `Username`, `Role` FROM `staff` where StaffID =" + _id + " ";
                dt = d.GetData(SQLConn.sqL);
                txtPosition.Text = dt.Rows[0][3].ToString();
                txtName.Text = dt.Rows[0][2].ToString();
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //public float ProductQty;
            dataGridView1.Rows.Clear();

            DataTable dt = new DataTable();
            DB d = new DB();

            SQLConn.sqL = "SELECT " +
                   " td.TDetailNo AS TransactionDetailNo, " +
                   " t.InvoiceNo AS InvoiceNo, " +
                   " td.ProductNo AS ProductNo, " +
                   " p.ProductCode AS ProductName, " +
                   " pu.UnitName AS ProductUnit, " +
                   " td.Quantity AS ProductQuantity, " +
                   " td.ItemPrice AS ProductUnitPrice,	" +
                   " t.TotalAmount AS TotalPaymentDue, " +
                   " t.discount AS TotalDiscount, " +
                   " t.paidAmount AS TotalPaidAmount " +
                   " FROM transactions AS t " +
                   " INNER JOIN transactiondetails AS td ON t.InvoiceNo = td.InvoiceNo" +
                   " INNER JOIN Product AS p ON p.ProductNo = td.ProductNo " +
                   " INNER JOIN productunit AS pu ON pu.id = p.ProductUnitId " +
                   " WHERE t.InvoiceNo = '" + txtTransactionId.Text + "' ";

            dt = d.GetData(SQLConn.sqL);

            if (dt.Rows.Count > 0)
            {
                Console.WriteLine("Inovice No. is Exist");
                Console.WriteLine(Int32.Parse(dt.Rows[0]["ProductNo"].ToString()));

                //Binding master value

                totalAmountPaid = float.Parse(dt.Rows[0]["TotalPaidAmount"].ToString());
                totalPaymentDue = float.Parse(dt.Rows[0]["TotalPaymentDue"].ToString());

                txtTotal.Text = totalPaymentDue.ToString();
                txtDisc.Text = dt.Rows[0]["TotalDiscount"].ToString();
                txtReceive.Text = dt.Rows[0]["TotalPaidAmount"].ToString();
                paidamount.Text = totalAmountPaid.ToString();
                txtReturn.Text = (totalAmountPaid - totalPaymentDue).ToString();

                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine("row");
                    Console.WriteLine(row["ProductNo"].ToString());

                    //Binding child values

                    ProductQty = float.Parse(row["ProductQuantity"].ToString());
                    ProductUnitPrice = float.Parse(row["ProductUnitPrice"].ToString());
                    productTotalAmount = ProductQty * ProductUnitPrice;

                    dataGridView1.Rows.Add(row["ProductNo"].ToString(), row["ProductName"].ToString(), ProductUnitPrice, ProductQty, row["ProductUnit"].ToString(), productTotalAmount);
                }

                //itemId = Int32.Parse(dt.Rows[0][0].ToString());
                //currentItemName = dt.Rows[0][1].ToString();
                //price = decimal.Parse(dt.Rows[0][2].ToString());
                //itemUnit = dt.Rows[0][5].ToString();
                //itemUnitId = Int32.Parse(dt.Rows[0][6].ToString());

            }
            else
            {
                Console.WriteLine("Inovice No not Exist");
            }
            //txtProductUnit.Text = price.ToString();
            //labelProductUnit.Text = itemUnit;
        }

        private void FormPos_Click(object sender, EventArgs e)
        {
            this.Close();
            frmPOS1 pos = new frmPOS1();
            pos.Show();
        }

        private bool CustomUpdateQuery(string query)
        {
            var affectedRows = 0;
            try
            {
                SQLConn.sqL = query;
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);

                affectedRows = SQLConn.cmd.ExecuteNonQuery();

                Console.WriteLine("affectedRows");
                Console.WriteLine(affectedRows);
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

            if (affectedRows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnSaleReturnAll_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to Return this Sale ?", "Return Sale", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                if (CustomUpdateQuery("DELETE FROM transactiondetails WHERE InvoiceNo = '" + txtTransactionId.Text + "';"))
                {
                    CustomUpdateQuery("DELETE FROM transactions WHERE InvoiceNo = '" + txtTransactionId.Text + "';");

                    Interaction.MsgBox("Sale Return successfully completed");
                }
                else
                {
                    Interaction.MsgBox("Sorry !!! Failed to complete the Sale Return");
                }

                ClearSaleReturnForm();
            }
        }
      
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to Return this Sale ?", "Return Sale", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                if (e.ColumnIndex == 6)
                {
                    //i-e Return button is clicked

                    //returing indivdiual item 
                    Console.WriteLine(dataGridView1.Rows[e.RowIndex].Cells);

                    decimal totalAmount = 0;
                    decimal cellItemPrice = Decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                    decimal cellQty = Decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                    decimal individualtemTotalAmount = cellQty * cellItemPrice;
                    decimal calculatedAmount = 0;

                    //first fetching the TotalAmount from transaction table 
                    //so we can subtract this TotalAmount from individual itemTotalAmount
                    DataTable dt = new DataTable();
                    DB d = new DB();

                    try
                    {
                        //Now updating the transaction
                        SQLConn.sqL = "SELECT `InvoiceNo`, `TotalAmount`, `discount`, `paidAmount` FROM `transactions` WHERE InvoiceNo=" + txtTransactionId.Text + " ";
                        dt = d.GetData(SQLConn.sqL);

                        totalAmount = decimal.Parse(dt.Rows[0]["TotalAmount"].ToString());
                        calculatedAmount = totalAmount - individualtemTotalAmount;

                        CustomUpdateQuery("UPDATE transactions SET TotalAmount = '" + calculatedAmount + "' WHERE InvoiceNo = '" + txtTransactionId.Text + "';");

                        Interaction.MsgBox("Sale Return successfully completed");

                    }
                    catch (Exception)
                    {
                        ClearSaleReturnForm();
                        Interaction.MsgBox("Sorry !!! Failed to complete the Sale Return");
                        throw;
                    }

                    ClearSaleReturnForm();
                }
            }
        }

        private void txtReceive_TextChanged(object sender, EventArgs e)
        {

        }

        private void ClearSaleReturnForm()
        {
            dataGridView1.Rows.Clear();

            txtTransactionId.Clear();
            txtTotal.Clear();
            txtDisc.Clear();
            txtReceive.Clear();
            paidamount.Clear();
        }

        private void SaleReturn_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to exit and move to POS screen ?", "Exit Sale Return Screen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                if (txtPosition.Text == "Admin")
                {
                    this.Close();

                    frmMain frm = new frmMain(txtName.Text, _id);
                    frm.Show();
                }
                else
                {
                    this.Close();

                    frmPOS1 frm = new frmPOS1(_id);
                    frm.Show();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtTransactionId.Clear();
            dataGridView1.Rows.Clear();
        }
    }
}
