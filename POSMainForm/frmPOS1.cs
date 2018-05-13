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
    public partial class frmPOS1 : Form
    {

        public decimal price { get; set; }
        public decimal total { get; set; }
        public decimal quantity { get; set; }
        public double globalStockOnHand = 0;

        public static string userID { get; set; }


        public static string rtrnInv = "";

        public frmPOS1()
        {
            InitializeComponent();
            dtpDate.CustomFormat = "dd-MM-yyyy";
        }
        public frmPOS1(int id)
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
                SQLConn.sqL = "SELECT `StaffID`, `Firstname`, `Username`, `Role` FROM `staff` where StaffID="+_id+" ";
                dt = d.GetData(SQLConn.sqL);
                txtPosition.Text = dt.Rows[0][3].ToString();
                txtName.Text = dt.Rows[0][2].ToString();
            }
            catch (Exception)
            {
                
            }
            
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


        string currentItemName;
        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dt = new DataTable();
            DB d = new DB();
            if (e.KeyCode == Keys.Enter)
            {
                SQLConn.sqL = "SELECT `ProductNo`, `ProductCode`,`UnitPrice`, `StocksOnHand`, `ReorderLevel`, pu.UnitName as UnitName, pu.Id as ProductUnitId FROM `Product` LEFT JOIN productunit pu ON pu.Id = Product.ProductUnitId WHERE ProductCode = '" + txtProductName.Text + "' or barcode = '" + txtProductName.Text + "' ";
                dt = d.GetData(SQLConn.sqL);

                if(dt.Rows.Count > 0)
                {
                    itemId = Int32.Parse(dt.Rows[0][0].ToString());
                    currentItemName = dt.Rows[0][1].ToString();
                    price = decimal.Parse(dt.Rows[0][2].ToString());
                    itemUnit = dt.Rows[0][5].ToString();
                    itemUnitId = Int32.Parse(dt.Rows[0][6].ToString());


                    if (int.Parse(dt.Rows[0][3].ToString()) != 0)
                    {
                        globalStockOnHand = int.Parse(dt.Rows[0][3].ToString());
                    }
                }
                txtProductUnit.Text = price.ToString();
                cbProductUnit.Text = itemUnit;


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
            reg = new System.Text.RegularExpressions.Regex ("[0-9]");

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
            quantity  = decimal.Parse(txtQty.Text);
            total = price * quantity;

            if (globalStockOnHand != 0)
            {
                dataGridView1.Rows.Add(itemId, currentItemName, itemUnit, price, quantity, total);
            }
            else {
                MessageBox.Show("This Product stock is 0 in quantity !!! ");
                txtProductName.Text = "";
                txtProductUnit.Text = "";
                txtQty.Text = "";
                return;
            }
            


            //txtItemQty.Text = dataGridView1.Rows.Count.ToString();

            //grand_total = total + grand_total;
            //txtTotal.Text = grand_total.ToString();
            //txtReceive.Text = grand_total.ToString();

            total = 0;
            //grand_total = 0;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 4)
            {

            }
            else if (e.ColumnIndex == 5)
            {
                decimal subtract = Decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                strTotalAmt = Decimal.Parse(txtTotal.Text) - subtract;
                grand_total -= subtract;
                
                txtTotal.Text = strTotalAmt.ToString();
                txtReceive.Text = strTotalAmt.ToString();
                
                dataGridView1.Rows.RemoveAt(e.RowIndex);

                strTotalAmt = 0;
            }
        }


        private void Clear()
        {
            dataGridView1.Rows.Clear();
            txtProductName.Clear();
            txtQty.Clear();
            txtProductUnit.Clear();
            txtQty.Clear();

            txtTotal.Text = "0";
            txtDisc.Text = "0";
            txtReturn.Text = "0";
            txtReceive.Text = "0";
            paidamount.Text = "0";


        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveSale();

                Clear(); 

            }
            else
            {
                MessageBox.Show("Please enter items...");
            }
            
            
            
        }



        private void SaveSale()
        {
            try
            {
                SQLConn.sqL = @"INSERT INTO transactions(TDate,Customer, StaffID,TotalAmount,discount,paidAmount) values ('" + dtpDate.Text + "','Cash'," + _id + " ," + txtTotal.Text + ","+txtDisc.Text+","+txtReceive.Text+")";
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

        private void UpdateProductQuantity(string query)
        {
            try
            {
                SQLConn.sqL = query;
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();
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
                    //Ahmer - Add productunit field here

                    SQLConn.sqL = @"insert into transactiondetails(InvoiceNo,ProductNo,ItemName,ItemPrice,Quantity) 
                        values ('" + invId + "', " + dataGridView1.Rows[i].Cells[0].Value.ToString() + ",'" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "',  '" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "')";
                    SQLConn.ConnDB();
                    SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                    SQLConn.cmd.ExecuteNonQuery();
                    UpdateProductQuantity("UPDATE Product SET StocksOnhand = StocksOnHand - '" + Conversion.Val(dataGridView1.Rows[i].Cells[3].Value.ToString()) + "' WHERE ProductNo = '" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "'");
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
                ReciptPrint();
            }
        }


        private void ReciptPrint()
        {
            PrintDialog printDialog = new PrintDialog();

            PrintDocument printDocument = new PrintDocument();
        
            printDialog.Document = printDocument;

            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            //old code
            //PrinterSettings.InstalledPrinters

            // StoreDetails SD = new StoreDetails();
            // SD.device_Settings();
            //  printDocument.PrinterSettings.PrinterName = SD.Receipt_Printer_PORT;

            //new code
            DialogResult dialogResult = printDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                printDocument.Print();
            }
            else
            {
                return;
            }

        }
        void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {

            Graphics graphic = e.Graphics;
            Font font = new Font("Courier New", 8);

            string StoreName = "Sindh Sweets";
            string StorePhn = "92-2134151389";
            string StoreAddress = "Shop# 9,Rim Jhim Towers,Safoora Chowrangi";
            string StoreReciptComment = "Thank You! Hope You Come Again.";


            int startx = 10;
            int starty = 50;
            int offset = 25;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            //  sf.LineAlignment = StringAlignment.Center;
            //Harrington

            //graphic.DrawString(StoreName, new Font("Canterbury", 30, FontStyle.Bold), new SolidBrush(Color.Black), new PageSettings().Margins.Left + (int)FontHeight + 20, starty, sf);
            //offset = offset + (int)FontHeight;

            //
            Image img = System.Drawing.Image.FromFile("E:\\GitHub-Clone\\radium_1\\POSMainForm\\Resources\\logo_2_1.jpg");
            Point loc = new Point(110,5);
            e.Graphics.DrawImage(img, loc);

            graphic.DrawString(StoreAddress, new Font("Century Gothic", 8, FontStyle.Regular), new SolidBrush(Color.Black), new PageSettings().Margins.Left + (int)FontHeight + 35, starty + 10, sf);
            offset = offset + (int)FontHeight;
            graphic.DrawString("Scheme 33 Karachi Sindh.", new Font("Century Gothic", 8, FontStyle.Regular), new SolidBrush(Color.Black), new PageSettings().Margins.Left + (int)FontHeight + 50, starty + 30, sf);
            offset = offset + (int)FontHeight;
            graphic.DrawString(StorePhn, new Font("Century Gothic", 8, FontStyle.Regular), new SolidBrush(Color.Black), new PageSettings().Margins.Left + (int)FontHeight + 50, starty + 45, sf);
            offset = offset + (int)FontHeight;

            //Pen blackPen = new Pen(Color.Black, 3);

            //draw rectangle + write text in it
            Font stringFont = new Font("Canterbury", 16);
            SizeF rectSize = graphic.MeasureString(Text, stringFont);
            
            //graphic.FillRectangle(Brushes.Blue, 10, 80, rectSize.Width + 180, rectSize.Height);

            //graphic.DrawString("Sales Receipt", new Font("Century Gothic", 10, FontStyle.Bold), new SolidBrush(Color.White), new PageSettings().Margins.Left + (int)FontHeight - 35, starty + offset + 25);
            //offset = offset + (int)FontHeight + 10;


            graphic.DrawString("Cashier: CASH", new Font("Century Gothic", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 10);
            offset = offset + (int)FontHeight + 2;

            graphic.DrawString("Date: " + System.DateTime.Now.ToString(), new Font("Century Gothic", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 12);
            offset = offset + (int)FontHeight + 2;

            graphic.DrawString("Tran#: "+ invId, new Font("Century Gothic", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 17);
            offset = offset + (int)FontHeight + 2;

            //graphic.DrawString("Customer: " + CustomerName + "   ", new Font("Century Gothic", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 37);
            //offset = offset + (int)FontHeight + 2;



            //draw rectangle + write text in it
           // graphic.FillRectangle(Brushes.Blue, 10, 165, rectSize.Width + 180, rectSize.Height);
            graphic.DrawString("Item Description", new Font("Century Gothic", 10, FontStyle.Bold), new SolidBrush(Color.Black), new PageSettings().Margins.Left + (int)FontHeight - 25, starty + offset + 20);
            offset = offset + (int)FontHeight + 10;

            //graphic.DrawString("________________________________", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset + 10);
            //offset = offset + (int)FontHeight + 15;

            graphic.DrawString("_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _", new Font("Century Gothic", 10, FontStyle.Regular), new SolidBrush(Color.Gray), startx, starty + offset);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {

                string ProductName = dataGridView1.Rows[i].Cells[1].Value.ToString();
                string ProductQty = dataGridView1.Rows[i].Cells[3].Value.ToString();

                string ProductUnitPrice = dataGridView1.Rows[i].Cells[2].Value.ToString();
                string ProdcutTotalPrice = dataGridView1.Rows[i].Cells[4].Value.ToString();

                string ProductPriceLine = "Rs. " + ProductUnitPrice + "     " + "  Qty: " + ProductQty;

                offset = offset + (int)FontHeight + 5;
                graphic.DrawString(ProductName, new Font("Century Gothic", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
                offset = offset + (int)FontHeight + 2;
                graphic.DrawString(ProductPriceLine.PadRight(30) + String.Format("Rs. " + Convert.ToDouble(ProdcutTotalPrice)), new Font("Century Gothic", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
            }

            graphic.DrawString("_________________________________________", new Font("Century Gothic", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
            offset = offset + 20;
            
            
                graphic.DrawString("Total Amount".PadRight(30) + String.Format("Rs. "+txtTotal.Text), new Font("Century Gothic", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
                offset = offset + (int)FontHeight + 5;
                graphic.DrawString("Discount".PadRight(30) + "Rs. " + txtDisc.Text.PadRight(30), new Font("Century Gothic", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
                offset = offset + (int)FontHeight + 5;
                graphic.DrawString("Net Total".PadRight(30) + String.Format("Rs."+txtReceive.Text), new Font("Century Gothic", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
                offset = offset + (int)FontHeight + 5;
                graphic.DrawString("Change".PadRight(30) + String.Format("Rs."+txtReturn.Text), new Font("Century Gothic", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
                offset = offset + (int)FontHeight + 5;
            

            graphic.DrawString("_________________________________________", new Font("Century Gothic", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
            offset = offset + (int)FontHeight + 5;
            graphic.DrawString(StoreReciptComment, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
            //+ (int)FontHeight + 15
            graphic.DrawString("_________________________________________", new Font("Century Gothic", 10, FontStyle.Bold), new SolidBrush(Color.Black), startx, starty + offset);
            offset = offset + (int)FontHeight + 5;
            //offset = offset + (int)FontHeight + 5;
            graphic.DrawString("Developed By : www.itntworld.com", new Font("Century Gothic", 7, FontStyle.Regular), new SolidBrush(Color.Black), startx, starty + offset);
            offset = offset + (int)FontHeight + 2;
            graphic.DrawString("Contact:92-2134151387", new Font("Century Gothic", 7, FontStyle.Regular), new SolidBrush(Color.Black), startx, starty + offset);
            //offset = offset + (int)FontHeight + 2;
            //graphic.DrawString("", new Font("Century Gothic", 7, FontStyle.Regular), new SolidBrush(Color.Black), startx, starty + offset);

            
        }


        public int itemId { get; set; }
        public string itemName { get; set; }
        public string itemUnit { get; set; }
        public int itemUnitId { get; set; }

        private void txtDisc_TextChanged(object sender, EventArgs e)
        {
            reg = new Regex("[0-9]");
            if (reg.IsMatch(txtDisc.Text))
            {
                strDisc = Decimal.Parse(txtDisc.Text);
            }
            else
            {
                MessageBox.Show("Discount should be in numeric format.");
                txtDisc.Focus();
                
            }
        }


        decimal strTotalAmt,strDisc,strPaid;


        private void txtDisc_KeyDown(object sender, KeyEventArgs e)
        {
            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtProductName.Clear();
            txtQty.Clear();
            txtProductUnit.Clear();
        }

        private void frmPOS1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtPaid_TextChanged(object sender, EventArgs e)
        {
            
        }

        decimal recAmt;
        private void button3_Click(object sender, EventArgs e)
        {
            if (paidamount.Text != "")
            {
                recAmt = decimal.Parse(paidamount.Text);
                txtReceive.Text = recAmt.ToString();

                    strTotalAmt = Decimal.Parse(txtTotal.Text);
                    strPaid = strTotalAmt - strDisc; // t-bill amount
                    txtReturn.Text = Math.Round((recAmt - strPaid), 2).ToString();
                
            }
            strTotalAmt = 0;
            strPaid = 0;

        }

        private void frmPOS1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to exit?", "Exit System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                if (txtPosition.Text == "Admin")
                {
                    this.Close();

                    frmMain frm = new frmMain(txtName.Text,_id);
                    frm.Show();
                }
                else
                {
                    Application.Exit();

                }

            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

            grand_total = 0;
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    grand_total += Decimal.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                }

                txtTotal.Text = grand_total.ToString();
                txtReceive.Text = grand_total.ToString();
            }
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void paidamount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (paidamount.Text != "")
                {
                    recAmt = decimal.Parse(paidamount.Text);
                    txtReceive.Text = recAmt.ToString();

                    strTotalAmt = Decimal.Parse(txtTotal.Text);
                    strPaid = strTotalAmt - strDisc; // t-bill amount
                    txtReturn.Text = Math.Round((recAmt - strPaid), 2).ToString();
                    //------------------------  //------------------------ //------------------------
                    //btnSale_Click(sender, e);
                }
                strTotalAmt = 0;
                strPaid = 0;

            }
        }

        private void btnSaleReturn_Click(object sender, EventArgs e)
        {
            frmSaleReturn frm = new frmSaleReturn();
            frm.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void paidamount_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtProductUnit_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void txtPosition_TextChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmPOS1_Activated(object sender, EventArgs e)
        {
            if (rtrnInv != "")
            {
                RetrunInv();
            }
        }



        private void RetrunInv()
        {
            dataGridView1.Rows.Clear();
            lblInvNo.Text = "Inv No: " + rtrnInv;
            SQLConn.sqL = "SELECT `ProductNo`,`ItemName`, `ItemPrice`, `Quantity`,`InvoiceNo` FROM `transactiondetails` WHERE InvoiceNo = "+rtrnInv+"  ";
            DB d = new DB();
            DataTable dt = new DataTable();
            dt = d.GetData(SQLConn.sqL);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                itemId =    Int32.Parse(dt.Rows[i]["ProductNo"].ToString());
                itemName = dt.Rows[i]["ItemName"].ToString();
                price = Int32.Parse(dt.Rows[i]["ItemPrice"].ToString());
                quantity = Int32.Parse(dt.Rows[i]["Quantity"].ToString());
                total = price * quantity;
                dataGridView1.Rows.Add(itemId, itemName, price, quantity, total);
            }
            
        }
    }
}
