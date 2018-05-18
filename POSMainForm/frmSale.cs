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
    public partial class frmSale : Form
    {

        public decimal price { get; set; }
        public decimal total { get; set; }
        public decimal quantity { get; set; }

        public static string userID { get; set; }


        public frmSale()
        {
            InitializeComponent();
        }
        public frmSale(int id)
        {
            InitializeComponent();
            _id = id;
        }

        int _id;

        public decimal grand_total { get; private set; }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DB d = new DB();

            try
            {
                GetCatFilter();
                SQLConn.sqL = "SELECT `StaffID`, `Firstname`, `Username`, `Role` FROM `staff` where StaffID=" + _id + " ";
                dt = d.GetData(SQLConn.sqL);
                txtPosition.Text = dt.Rows[0][3].ToString();
                txtName.Text = dt.Rows[0][2].ToString();
            }
            catch (Exception)
            {

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {


        }


        void GetCatFilter()
        {
            //=-====
            SQLConn.sqL = "SELECT CategoryNo,CategoryName FROM category";
            DB d = new DB();
            DataTable dt = new DataTable();
            dt = d.GetData(SQLConn.sqL);

            // Load department Intelisence
            AutoCompleteStringCollection cat = new AutoCompleteStringCollection();
            foreach (var item in dt.Rows)
            {
                cat.Add(item.ToString());
            }
            cmbCat.AutoCompleteCustomSource = cat;
            cmbCat.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCat.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbCat.DataSource = dt;
            cmbCat.DisplayMember = "CategoryName";
            cmbCat.ValueMember = "CategoryNo";

        }


        private void AutoSuggest(AutoCompleteStringCollection col)
        {
            try
            {
                string query = "select ProductCode from product";
                //string query = "select ProductCode from product where CategoryNo = " + cmbCat.SelectedValue + " ";
                DataTable dt = new DataTable();
                DB d = new DB();
                dt = d.GetData(query);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    col.Add(dt.Rows[i][0].ToString());
                }

            }
            catch (Exception)
            {

            }


        }
        void GetItemFilter()
        {
            txtProductName.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtProductName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection DataCollection = new AutoCompleteStringCollection();
            AutoSuggest(DataCollection);
            txtProductName.AutoCompleteCustomSource = DataCollection;
        }

        private void cmbCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetItemFilter();

            }
            catch (Exception)
            {

            }

        }

        private void txtProductUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQty.Focus();
            }
        }

        DataTable dtItem = new DataTable();
        int itemId;
        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {

            DB d = new DB();
            if (e.KeyCode == Keys.Enter)
            {
                SQLConn.sqL = "SELECT `ProductNo`, `ProductCode`,`UnitPrice`, `StocksOnHand`, `ReorderLevel` FROM `product` WHERE CategoryNo = " + cmbCat.SelectedValue + " && ProductCode = '" + txtProductName.Text + "'";
                dtItem = d.GetData(SQLConn.sqL);

                if (dtItem.Rows.Count > 0)
                {
                    itemId = Int32.Parse(dtItem.Rows[0][0].ToString());
                    price = decimal.Parse(dtItem.Rows[0][2].ToString());

                }
                txtProductUnit.Text = price.ToString();

            }
        }

        private void txtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
            }
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //-----------
                //-----------
                //-----------
                //-----------
            }
        }

        private void cmbCat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtProductName.Focus();
            }
        }

        System.Text.RegularExpressions.Regex reg;

        private void button1_Click(object sender, EventArgs e)
        {
            reg = new System.Text.RegularExpressions.Regex("[0-9]");

            if (txtProductName.Text == string.Empty)
            {
                MessageBox.Show("Please insert product.");
                return;
            }

            if (txtQty.Text == string.Empty)
            {

                MessageBox.Show("Please insert quantity.");
                return;
            }
            else if (!reg.IsMatch(txtQty.Text))
            {

                MessageBox.Show("Please write only numeric quantity.");
                return;
            }

            if (txtProductUnit.Text == string.Empty)
            {
                MessageBox.Show("Please insert price.");
                return;
            }
            else if (!reg.IsMatch(txtProductUnit.Text))
            {
                MessageBox.Show("Please write only numeric amount.");
                return;
            }



            price = decimal.Parse(txtProductUnit.Text);
            quantity = decimal.Parse(txtQty.Text);
            total = price * quantity;
            dataGridView1.Rows.Add(itemId, txtProductName.Text, price, quantity, total);

            txtItemQty.Text = dataGridView1.Rows.Count.ToString();
            grand_total = total + grand_total;
            txtTotal.Text = grand_total.ToString();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {

            }
            else if (e.ColumnIndex == 5)
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            SaveSale();

            DialogResult dr = MessageBox.Show("Do You Want to Print?", "Print Recipt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if(dr== System.Windows.Forms.DialogResult.Yes)
            {
                RptSaleInv r = new RptSaleInv();
                r.ShowDialog();
            }
            else{
                this.Close();
                frmSale frm = new frmSale();
                frm.Show();
            }
        }

        private void SaveSale()
        {
            try
            {
                SQLConn.sqL = @"INSERT INTO transactions(TDate,Customer, NonVatTotal,TotalAmount,StaffID) values ('" + dtpDate.Text + "','" + textBox2.Text + "'," + txtTotal.Text + "," + txtTotal.Text + "," + _id + " )";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
            }
            finally
            {
                SQLConn.cmd.Dispose();
                SQLConn.conn.Close();
            }
            SaveSaleDetails();
        }

        string invId;
        private void SaveSaleDetails()
        {
            DB d = new DB();
            DataTable dt = new DataTable();
            dt = d.GetData("SELECT InvoiceNo FROM transactions order by InvoiceNo DESC");
            invId = dt.Rows[0][0].ToString();
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    SQLConn.sqL = @"insert into transactiondetails(InvoiceNo,ProductNo,ItemName,ItemPrice,Quantity) 
                        values ('" + invId + "', " + dataGridView1.Rows[i].Cells[0].Value.ToString() + ",'" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "',  '" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "')";
                    SQLConn.ConnDB();
                    SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                    SQLConn.cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
            }
            finally
            {
                SQLConn.cmd.Dispose();
                SQLConn.conn.Close();
            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            txtProductName.Clear();
            txtQty.Clear();
            txtProductUnit.Clear();
        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //private void ReciptPrint()
        //{
        //    PrintDialog printDialog = new PrintDialog();

        //    System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
        //    printDialog.Document = printDocument;

        //    printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument_PrintPage);
        //    //StoreDetails SD = new StoreDetails();
        //    //SD.device_Settings();
        //    //printDocument.PrinterSettings.PrinterName = SD.Receipt_Printer_PORT;
        //    printDocument.Print();

        //}

        //void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        //{

        //    Graphics graphic = e.Graphics;
        //    Font font = new Font("Courier New", 8);

        //    string AP_InvoiceName = ""; // AP = Account Payment
        //    string AP_Address = "";
        //    string AP_Telephone = "";

        //    string CustomerName = "";
        //    string StoreName = "";
        //    string StorePhn = "";
        //    string StoreAddress = "";
        //    string StoreReciptComment = "";


        //    SqlConnection connection = new SqlConnection();
        //    connection.ConnectionString = scdr.connection_string();
        //    connection.Open();
        //    try
        //    {

        //        SqlCommand command = new SqlCommand("select * from cptpos_store", connection);
        //        SqlDataReader dr = command.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            StoreName = dr["store_name"].ToString();
        //            StorePhn = dr["store_phone_no"].ToString();
        //            StoreAddress = dr["store_address"].ToString();
        //            StoreReciptComment = dr["store_recipt_msg"].ToString();
        //        }
        //        dr.Close();

        //        if (loyalty_code != 0)
        //        {
        //            SqlCommand command2 = new SqlCommand("select * from cptpos_loyaltycard where loyalty_code=" + loyalty_code, connection);
        //            SqlDataReader dr2 = command2.ExecuteReader();

        //            while (dr2.Read())
        //            {
        //                CustomerName = dr2["customer_name"].ToString();
        //            }
        //            dr2.Close();
        //        }

        //    }
        //    catch (SqlException exception)
        //    {
        //        MessageBox.Show(exception.ToString());
        //    }
        //    int startx = 10;
        //    int starty = 10;
        //    int offset = 25;
        //    StringFormat sf = new StringFormat();
        //    sf.Alignment = StringAlignment.Center;
        //    //  sf.LineAlignment = StringAlignment.Center;
        //    //Harrington

        //    graphic.DrawString("C C", new Font("Harrington", 18, FontStyle.Bold), new SolidBrush(Color.Black), new PageSettings().Margins.Left + (int)FontHeight + 20, starty - 10, sf);
        //    offset = offset + (int)FontHeight;
        //    graphic.DrawString(StoreName, new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), new PageSettings().Margins.Left + (int)FontHeight + 20, starty + 20, sf);
        //    offset = offset + (int)FontHeight;
        //    graphic.DrawString(StoreAddress, new Font("Courier New", 8, FontStyle.Regular), new SolidBrush(Color.Black), new PageSettings().Margins.Left + (int)FontHeight + 30, starty + 40, sf);
        //    //offset = offset + (int)FontHeight;
        //    // graphic.DrawString(StorePhn, new Font("Courier New", 8, FontStyle.Regular), new SolidBrush(Color.Black), new PageSettings().Margins.Left + (int)FontHeight + 30, starty + 40, sf);
        //    //Pen blackPen = new Pen(Color.Black, 3);

        //    // Account Payment Printing Receipt
        //    if (Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["customeraccount_id"]) != 0)
        //    {
        //        int c_id = 0;
        //        c_id = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["customeraccount_id"]);

        //        try
        //        {
        //            SqlCommand command = new SqlCommand("select * from " + accountTable + " where id=" + c_id, connection);
        //            SqlDataReader dr = command.ExecuteReader();

        //            while (dr.Read())
        //            {
        //                // for Printing
        //                AP_InvoiceName = dr["invoice_name"].ToString();
        //                AP_Address = dr["postal_address"].ToString();
        //                AP_Telephone = dr["telephone"].ToString();
        //            }
        //            dr.Close();

        //            Font stringFont = new Font("Arial", 16);
        //            SizeF rectSize = graphic.MeasureString(Text, stringFont);
        //            graphic.FillRectangle(Brushes.Blue, 10, 70, rectSize.Width + 180, rectSize.Height);
        //            graphic.DrawString("Account Payment", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.White), new PageSettings().Margins.Left + (int)FontHeight - 25, starty + offset + 14);
        //            offset = offset + (int)FontHeight + 5;

        //            graphic.DrawString("Cashier: " + textBox_Cashier.Text, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 20);
        //            offset = offset + (int)FontHeight + 2;

        //            graphic.DrawString("Date: " + System.DateTime.Now.ToString(), new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 22);
        //            offset = offset + (int)FontHeight + 2;

        //            graphic.DrawString("Tran#: " + transaction_id, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 24);
        //            offset = offset + (int)FontHeight + 10;

        //            graphic.DrawString("_________________________________________", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight - 15;

        //            graphic.DrawString("Account Name: " + AP_InvoiceName, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 20);
        //            offset = offset + (int)FontHeight + 2;

        //            graphic.DrawString("Address: " + AP_Address, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 22);
        //            offset = offset + (int)FontHeight + 2;

        //            graphic.DrawString("Telephone: " + AP_Telephone, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 24);
        //            offset = offset + (int)FontHeight + 2;

        //            graphic.FillRectangle(Brushes.Blue, 10, 200, rectSize.Width + 180, rectSize.Height);
        //            graphic.DrawString("Ammount Paid:".PadRight(20) + String.Format("{0:N}", Convert.ToDouble(textBox_Amount_Rcvd.Text)), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.White), (new PageSettings().Margins.Left * 0) + 15, starty + offset + 30);
        //            offset = offset + (int)FontHeight + 65;

        //            graphic.DrawString("Signed By: " + "_________________________________________", new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 24);
        //            offset = offset + (int)FontHeight + 5;

        //            graphic.DrawString("(" + AP_InvoiceName + ")", new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 24);
        //            offset = offset + (int)FontHeight + 12;

        //            graphic.DrawString("_________________________________________", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight + 10;

        //            graphic.DrawString(StoreReciptComment, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);

        //        }
        //        catch (SqlException exception)
        //        {
        //            MessageBox.Show(exception.ToString());
        //        }
        //    }
        //    else
        //    {
        //        //draw rectangle + write text in it
        //        Font stringFont = new Font("Arial", 16);
        //        SizeF rectSize = graphic.MeasureString(Text, stringFont);
        //        graphic.FillRectangle(Brushes.Blue, 10, 70, rectSize.Width + 180, rectSize.Height);



        //        //graphic.DrawString("________________________________", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);

        //        graphic.DrawString("Cashier: " + textBox_Cashier.Text, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 20);
        //        offset = offset + (int)FontHeight + 2;

        //        graphic.DrawString("Date: " + System.DateTime.Now.ToString(), new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 22);
        //        offset = offset + (int)FontHeight + 2;

        //        graphic.DrawString("Tran#: " + transaction_id, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 24);
        //        offset = offset + (int)FontHeight + 2;

        //        graphic.DrawString("Customer: " + CustomerName + "  |  Points: " + point_total, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 24);
        //        offset = offset + (int)FontHeight + 2;




        //        //graphic.DrawImage()
        //        if (onHoldSale == 1)
        //        {
        //            offset = offset + (int)FontHeight + 40;

        //            graphic.DrawString("_________________________________________", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight + 5;
        //            graphic.DrawString("This is sale is currently on hold", new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //            graphic.DrawString("_________________________________________", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight + 5;


        //            //graphic.DrawImage(barcodePanel1.BackgroundImage, new PageSettings().Margins.Left + (int)FontHeight - 30, starty + offset-50);

        //            graphic.DrawString("Hold Sale Code :  " + barcodePanel1.Text, new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset - 50);


        //            graphic.DrawString("Return item within 15 days Excluding Electronics items.", new Font("Courier New", 7, FontStyle.Regular), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight + 4;
        //            graphic.DrawString("Software Developed By : www.itntworld.com ", new Font("Courier New", 7, FontStyle.Regular), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight + 4;
        //            graphic.DrawString("Ph# : +92-304-8563-470", new Font("Courier New", 7, FontStyle.Regular), new SolidBrush(Color.Black), startx, starty + offset);
        //            return;

        //        }
        //        //draw rectangle + write text in it
        //        graphic.FillRectangle(Brushes.Blue, 10, 165, rectSize.Width + 180, rectSize.Height);
        //        graphic.DrawString("Item Description", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.White), new PageSettings().Margins.Left + (int)FontHeight - 35, starty + offset + 30);
        //        offset = offset + (int)FontHeight + 45;

        //        //graphic.DrawString("________________________________", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 10);
        //        //offset = offset + (int)FontHeight + 15;


        //        for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
        //        {
        //            //SqlCommand commandInsert2 = new SqlCommand("INSERT INTO [cptpos_sold_items] ([sales_id],[item_id],[item_name],[selling_price],[quantity],[total_price]) VALUES ( " + sales_id + "," + dataGridView1.Rows[i].Cells[0].Value + ", '" + dataGridView1.Rows[i].Cells[1].Value + "', " + dataGridView1.Rows[i].Cells[2].Value + ", " + dataGridView1.Rows[i].Cells[3].Value + ", " + dataGridView1.Rows[i].Cells[4].Value + "  )", connection);
        //            string ProductName = dataGridView1.Rows[i].Cells[1].Value.ToString();
        //            string ProductQty = dataGridView1.Rows[i].Cells[3].Value.ToString();
        //            string ProductUnitPrice = dataGridView1.Rows[i].Cells[2].Value.ToString();
        //            string ProdcutTotalPrice = dataGridView1.Rows[i].Cells[4].Value.ToString();

        //            string ProductPriceLine = "Rs." + ProductUnitPrice + "  Qty:" + ProductQty;
        //            offset = offset + (int)FontHeight + 5;
        //            graphic.DrawString(ProductName, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight + 2;
        //            graphic.DrawString(ProductPriceLine.PadRight(30) + String.Format("Rs. " + Convert.ToDouble(ProdcutTotalPrice)), new Font("Courier New", 7, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);

        //            graphic.DrawString("_________________________________________", new Font("Courier New", 10, FontStyle.Regular), new SolidBrush(Color.Gray), startx, starty + offset);
        //        }
        //        graphic.DrawString("_________________________________________", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //        offset = offset + 20;
        //        if (textBox_Discount.Text != "")
        //        {
        //            string discount_note = " "; //"(Loaylty Card Holder)";
        //            graphic.DrawString("Total Amount".PadRight(30) + String.Format("Rs. " + Convert.ToDouble(textBox_TotalAmount.Text)), new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight + 5;
        //            graphic.DrawString("Discount" + discount_note.PadRight(5) + "Rs. " + textBox_Discount.Text, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight + 5;
        //            graphic.DrawString("Net Total Amount".PadRight(30) + String.Format("Rs. " + textBox_TotalAmount.Text), new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight + 5;
        //            graphic.DrawString("Payment Type".PadRight(30) + String.Format("Rs. " + label_paymenttype.Text), new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight + 5;
        //            graphic.DrawString("Change".PadRight(30) + String.Format("Rs. " + Convert.ToDouble(textBox_Balance.Text)), new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight + 5;
        //        }
        //        else
        //        {
        //            graphic.DrawString("Total Amount".PadRight(30) + String.Format("Rs. " + Convert.ToDouble(textBox_TotalAmount.Text)), new Font("Courier New", 7, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //            offset = offset + (int)FontHeight + 5;


        //            if (textBox_Amount_Rcvd.Text != "")
        //            {
        //                graphic.DrawString("Payment Type".PadRight(30) + String.Format("Rs. " + label_paymenttype.Text), new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //                offset = offset + (int)FontHeight + 5;
        //                graphic.DrawString("Cash".PadRight(30) + String.Format("Rs. " + Convert.ToDouble(textBox_Amount_Rcvd.Text)), new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //                offset = offset + (int)FontHeight + 5;
        //                graphic.DrawString("Change".PadRight(30) + String.Format("Rs. " + Convert.ToDouble(textBox_Balance.Text)), new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //                offset = offset + (int)FontHeight + 5;
        //            }
        //        }

        //        graphic.DrawString("_________________________________________", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //        offset = offset + (int)FontHeight + 5;
        //        graphic.DrawString(StoreReciptComment, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //        //+ (int)FontHeight + 15
        //        graphic.DrawString("_________________________________________", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
        //        offset = offset + (int)FontHeight + 5;
        //        //offset = offset + (int)FontHeight + 5;
        //        graphic.DrawString("Return item within 15 days Excluding Electronics items.", new Font("Courier New", 7, FontStyle.Regular), new SolidBrush(Color.Black), startx, starty + offset);
        //        offset = offset + (int)FontHeight + 2;
        //        graphic.DrawString("Software Developed By : www.itntworld.com ", new Font("Courier New", 7, FontStyle.Regular), new SolidBrush(Color.Black), startx, starty + offset);
        //        offset = offset + (int)FontHeight + 2;
        //        graphic.DrawString("Ph# : +92-304-8563-470", new Font("Courier New", 7, FontStyle.Regular), new SolidBrush(Color.Black), startx, starty + offset);

        //    }
        //    connection.Close();

        //}
    }
}
