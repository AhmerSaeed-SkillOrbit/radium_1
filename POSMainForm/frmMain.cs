using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using ExcelDataReader;
using MySql.Data.MySqlClient;
namespace POSMainForm
{
    public partial class frmMain : Form
    {
        string Username;
        int StaffID;

        public frmMain(string username, int staffID)
        {
            InitializeComponent();

            Username = username;
            StaffID = staffID;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            SQLConn.getData();
            this.lbluser.Text = "Login user : " + Username.ToUpper();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if (Interaction.MsgBox("Are you sure you want to exit?", MsgBoxStyle.YesNo, "Exit System") == MsgBoxResult.Yes)
            {
                Application.Exit();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmListStaff f1 = new frmListStaff();
            f1.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = "Date-Time : " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss tt");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmDatabaseConfig db = new frmDatabaseConfig();
            db.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmListCategory lc = new frmListCategory();
            lc.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Interaction.MsgBox("Are you sure you want to exit?", MsgBoxStyle.YesNo, "Exit System") == MsgBoxResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmListProduct lp = new frmListProduct();
            lp.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            
            frmPOS1 lp = new frmPOS1(StaffID);
            lp.ShowDialog();



        }

        private void picMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmReportFilterDailySales FR = new frmReportFilterDailySales();
            FR.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmReportFilterStocks rf = new frmReportFilterStocks();
            rf.ShowDialog();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            frmSystemSetting ss = new frmSystemSetting();
            ss.ShowDialog();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            frmSale frm = new frmSale();
            frm.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ope = new OpenFileDialog();
            ope.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm;";

            if (ope.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                try
                {
                    FileStream stream = new FileStream(ope.FileName, FileMode.Open);
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    DataSet result = excelReader.AsDataSet();

                    Console.WriteLine("Now Print the Table Data");                    

                    foreach (DataTable table in result.Tables)
                    {
                        //now removing the header rows
                        table.Rows.Remove(table.Rows[0]);

                        Console.WriteLine("Now Print the Row");

                        foreach (DataRow dr in table.Rows)
                        {
                            //Console.WriteLine();
                            //Console.WriteLine("ProductCode: "+ dr[0]);
                            //Console.WriteLine("Description: " + dr[1]);
                            //Console.WriteLine("Barcode: "+ dr[2]);
                            //Console.WriteLine("UnitPrice: "+ dr[3]);
                            //Console.WriteLine("StocksOnHand: "+ dr[4]);
                            //Console.WriteLine("ReorderLevel: "+ dr[5]);
                            //Console.WriteLine("CategoryNo: "+ dr[6]);
                            //Console.WriteLine("costPrice: "+ dr[7]);
                            //Console.WriteLine();

                            //checking if product is exist or not 
                            //if exist do not insert it again 
                            //if not exist then insert it
                            if (!IsProductExist(dr[0].ToString(), dr[2].ToString()))
                            {
                                Console.WriteLine("#### This Product not Exist ####");

                                SQLConn.sqL = "INSERT INTO Product(ProductCode, Description, Barcode, UnitPrice, StocksOnHand, ReorderLevel, CategoryNo,costPrice)" +
                                                            " VALUES('" + dr[0].ToString() + "', '" + dr[1].ToString() + "', '" + dr[2].ToString() + "', '" + double.Parse(dr[3].ToString()) + "','" + double.Parse(dr[4].ToString()) + "', '" + dr[5] + "', '" + dr[6] + "', '" + double.Parse(dr[7].ToString()) + "')";
                                SQLConn.ConnDB();
                                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                                SQLConn.cmd.ExecuteNonQuery();
                            }
                            else {

                                Console.WriteLine("&&&& This Product is already Exist &&&&");


                                SQLConn.sqL = "UPDATE Product SET ProductCode = '" + dr[0].ToString() + "', Description = '" + dr[1].ToString() + "', Barcode = '" + dr[2].ToString() + "', costPrice = '" + double.Parse(dr[7].ToString()) + "',UnitPrice = '" + double.Parse(dr[3].ToString()) + "', StocksOnHand = '" + double.Parse(dr[4].ToString()) + "', ReorderLevel = '" + dr[5].ToString() + "', CategoryNo = '" + dr[6].ToString() + "' WHERE ProductCode = '" + dr[0].ToString() + "' OR BarCode = '" + dr[2].ToString() + "'";
                                SQLConn.ConnDB();
                                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                                SQLConn.cmd.ExecuteNonQuery();

                            }
                        }
                    }

                    SQLConn.cmd.Dispose();
                    SQLConn.conn.Close();
                    excelReader.Close();
                    stream.Close();
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox(ex.Message);
                }

                finally
                {
                    MessageBox.Show("Import Successfully Done");
                }
            }
        }

        public bool IsProductExist(string productCode, string barCode)
        {
            try
            {
                SQLConn.sqL = "SELECT ProductNo FROM Product AS P WHERE P.Barcode = '" + barCode + "' or P.ProductCode = '" + productCode + "'";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();
                
                Console.WriteLine("Data Row");

                if (SQLConn.dr.Read()==true) {

                    Console.WriteLine("True");
                    Console.WriteLine(SQLConn.dr["ProductNo"].ToString());

                    return true;                    
                }
                else {
                    Console.WriteLine("False");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
                return false;

            }
            finally
            {
                SQLConn.cmd.Dispose();
                SQLConn.conn.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            TestWinForm twf = new TestWinForm();
            twf.Show();
        }

        private void lbluser_Click(object sender, EventArgs e)
        {

        }
    }
}
