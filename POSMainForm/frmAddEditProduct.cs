using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;

using MySql.Data.MySqlClient;

namespace POSMainForm
{
    public partial class frmAddEditProduct : Form
    {
        int productID;
        int categoryID;
        public frmAddEditProduct(int prodID)
        {
            InitializeComponent();
            productID = prodID;
        }

        private void GetProductNo()
        {
            try
            {
                SQLConn.sqL = "SELECT ProductNo FROM Product ORDER BY ProductNo DESC";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                if (SQLConn.dr.Read() == true)
                {
                    lblProductNo.Text = (Convert.ToInt32(SQLConn.dr["ProductNo"]) + 1).ToString();
                }
                else
                {
                    lblProductNo.Text = "1";
                }
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

        private void LoadUpdateCategory()
        {
            try
            {
                //PopulateProductUnitComboBox();

                SQLConn.sqL = "SELECT pu.UnitName as UnitName, pu.Id as ProductUnitId, ProductNo, ProductCode, P.Description, Barcode, P.CategoryNo, CategoryName,costPrice, UnitPrice, StocksOnHand, ReorderLevel FROM Product as P LEFT JOIN Category as C ON P.CategoryNo = C.CategoryNo LEFT JOIN productunit pu ON pu.Id = P.ProductUnitId WHERE ProductNo = '" + productID + "'";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.dr = SQLConn.cmd.ExecuteReader();

                if (SQLConn.dr.Read() == true)
                {

                    //BindProductUnitComboBox(SQLConn.dr["UnitName"].ToString());


                    lblProductNo.Text = SQLConn.dr["ProductNo"].ToString();
                    txtProductCode.Text = SQLConn.dr["ProductCode"].ToString();
                    txtDescription.Text = SQLConn.dr["Description"].ToString();
                    txtBarcode.Text = SQLConn.dr["Barcode"].ToString();
                    txtCategory.Text = SQLConn.dr["CategoryName"].ToString();
                    txtCategory.Tag = SQLConn.dr["CategoryNo"];
                    txtSalePrice.Text = Strings.Format(SQLConn.dr["UnitPrice"], "#,##0.00");
                    txtCostPrice.Text = Strings.Format(SQLConn.dr["costPrice"], "#,##0.00");
                    txtStocksOnHand.Text = SQLConn.dr["StocksOnHand"].ToString();
                    txtReorderLevel.Text = SQLConn.dr["ReorderLevel"].ToString();
                    //cbProductUnit.SelectedValue = SQLConn.dr["ProductUnitId"].ToString();
                    //cbProductUnit.SelectedText = SQLConn.dr["UnitName"].ToString();
                }
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

        private void AddProducts()
        {

            Console.WriteLine("Combo Box Value");

            Console.WriteLine(cbProductUnit.SelectedItem.ToString());
            Console.WriteLine(cbProductUnit.SelectedIndex);

            try
            {
                SQLConn.sqL = "INSERT INTO Product(ProductCode, Description, Barcode, UnitPrice,costPrice, StocksOnHand, ReorderLevel, CategoryNo, ProductUnitId) VALUES('" + txtProductCode.Text + "', '" + txtDescription.Text + "', '" + txtBarcode.Text.Trim() + "', '" + txtSalePrice.Text.Replace(",", "") + "','" + txtCostPrice.Text.Replace(",", "") + "', '" + txtStocksOnHand.Text.Replace(",", "") + "', '" + txtReorderLevel.Text + "', '" + categoryID + "','" + cbProductUnit.SelectedIndex + "')";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();
                Interaction.MsgBox("Product successfully added.", MsgBoxStyle.Information, "Add Product");
                AddStockIn();
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

        private void AddStockIn()
        {
            try
            {
                SQLConn.sqL = "INSERT INTO StockIn(ProductNo, Quantity, DateIn) Values('" + lblProductNo.Text + "', '" + txtStocksOnHand.Text + "', '" + System.DateTime.Now.ToString("MM/dd/yyyy") + "')";
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

        private void UpdateProduct()
        {
            try
            {
                SQLConn.sqL = "UPDATE Product SET ProductCode = '" + txtProductCode.Text + "', Description = '" + txtDescription.Text + "', Barcode = '" + txtBarcode.Text.Trim() + "', costPrice = '" + txtCostPrice.Text.Replace(",", "") + "',UnitPrice = '" + txtSalePrice.Text.Replace(",", "") + "', StocksOnHand = '" + txtStocksOnHand.Text.Replace(",", "") + "', ReorderLevel = '" + txtReorderLevel.Text + "', CategoryNo ='" + categoryID + "' WHERE ProductNo = '" + productID + "'";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                SQLConn.cmd.ExecuteNonQuery();

                Interaction.MsgBox("Product successfully Updated.", MsgBoxStyle.Information, "Update Product");
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

        private void ClearFields()
        {
            lblProductNo.Text = "";
            txtProductCode.Text = "";
            txtDescription.Text = "";
            txtBarcode.Text = "";
            txtCostPrice.Text = "";
            txtCategory.Text = "";
            txtSalePrice.Text = "";
            txtStocksOnHand.Text = "";
            txtReorderLevel.Text = "";
        }

        private void frmAddEditProduct_Load(object sender, EventArgs e)
        {
            if (SQLConn.adding == true)
            {
                lblTitle.Text = "Adding New Product";
                ClearFields();
                GetProductNo();
                PopulateProductUnitComboBox();
            }
            else
            {
                lblTitle.Text = "Updating Product";
                LoadUpdateCategory();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtCategory.Text == "")
            {
                Interaction.MsgBox("Please select category.", MsgBoxStyle.Information, "Category");
                return;
            }

            if (SQLConn.adding == true)
            {
                frmMain mainForm = new frmMain(null, 0);
                if (!mainForm.IsProductExist(txtProductCode.Text, txtBarcode.Text.Trim()))
                {
                    AddProducts();
                }
                else
                {
                    Interaction.MsgBox("Product already exist.", MsgBoxStyle.Information, "Add Product");
                }
            }
            else
            {
                UpdateProduct();

            }

            if (System.Windows.Forms.Application.OpenForms["frmListProduct"] != null)
            {
                (System.Windows.Forms.Application.OpenForms["frmListProduct"] as frmListProduct).LoadProducts("");
            }

            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            frmSelectCategory flc = new frmSelectCategory(this);
            flc.ShowDialog();
        }

        public string Category
        {
            get { return txtCategory.Text; }
            set { txtCategory.Text = value; }
        }

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtBarcode.Text = Class1.IDGenerate(5);
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public void PopulateProductUnitComboBox()
        {
            try
            {
                DataRow dr;
                SQLConn.sqL = "SELECT u.Id as Id, u.UnitName as UnitName FROM `productunit` AS u;";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                MySqlDataAdapter sda = new MySqlDataAdapter(SQLConn.cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dr = dt.NewRow();
                dr.ItemArray = new object[] { 0, "Select Unit" };
                dt.Rows.InsertAt(dr, 0);

                cbProductUnit.ValueMember = "Id";
                cbProductUnit.DisplayMember = "UnitName";
                cbProductUnit.DataSource = dt;

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



            //using (SqlDataAdapter da = new SqlDataAdapter("SELECT name FROM sys.databases ORDER BY name", connection))
            //{
            //    DataTable dt = new DataTable();
            //    da.Fill(dt);
            //    cbDBName.DisplayMember = "name";
            //    cbDBName.DataSource = dt;
            //    connection.Close();
            //}
        }

        public void BindProductUnitComboBox(string SelectedProductUnit)
        {
            try
            {
                DataRow dr;
                SQLConn.sqL = "SELECT u.Id as Id, u.UnitName as UnitName FROM `productunit` AS u;";
                SQLConn.ConnDB();
                SQLConn.cmd = new MySqlCommand(SQLConn.sqL, SQLConn.conn);
                MySqlDataAdapter sda = new MySqlDataAdapter(SQLConn.cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dr = dt.NewRow();
                dr.ItemArray = new object[] { 0, "Select Unit" };
                dt.Rows.InsertAt(dr, 0);

                cbProductUnit.ValueMember = "Id";
                cbProductUnit.DisplayMember = "UnitName";
                cbProductUnit.DataSource = dt;

                Console.WriteLine("Checking");
                Console.WriteLine(dt);

                

            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString());
            }
            finally
            {
                //SQLConn.cmd.Dispose();
                //SQLConn.conn.Close();
            }



            //using (SqlDataAdapter da = new SqlDataAdapter("SELECT name FROM sys.databases ORDER BY name", connection))
            //{
            //    DataTable dt = new DataTable();
            //    da.Fill(dt);
            //    cbDBName.DisplayMember = "name";
            //    cbDBName.DataSource = dt;
            //    connection.Close();
            //}
        }
    }
}
