using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopStoreApplication
{

    public partial class Form1 : Form
    {
        //the list in which the products will be located
        List<Product> products = new List<Product>();
        //string that will store name of the action (add,change)
        string action = "";
        //variable that stores the index of the selected row
        int selector = -1;
        public Form1()
        { 
            InitializeComponent();
            //adding columns in datagridview (first column (id) wil not be visible)
            dgwProducts.Columns.Add("productid", "Id");
            dgwProducts.Columns["productid"].Visible = false;
            dgwProducts.Columns.Add("productname", "Name");
            dgwProducts.Columns.Add("productmanufacturer", "Manufacturer");
            dgwProducts.Columns.Add("productcost", "Cost");
            dgwProducts.Columns.Add("productquantity", "Quantity");
            //DataGridView is made only for reading,u cant manualy change data
            dgwProducts.ReadOnly = true;
            //methods that enable / disable clicking on certain buttons / text fields
            txtDisabled();
            EnableAddChangeDelete();
            DisableConfirmCancel();
            //Method that shows all products from database in DataGridView
            ShowAllProducts();
        }
        //method that enables writing in text fields
        private void txtEnabled()
        {
            txtPName.Enabled = true;
            txtPManufacturer.Enabled = true;
            txtPCost.Enabled = true;
            txtPQuantity.Enabled = true;
        }
        //method that disables writing in text fields
        private void txtDisabled()
        {
            txtPName.Enabled = false;
            txtPManufacturer.Enabled = false;
            txtPCost.Enabled = false;
            txtPQuantity.Enabled = false;
        }
        //method that enables clicking on the add, change, delete buttons
        private void EnableAddChangeDelete()
        {
            btnAdd.Enabled = true;
            btnChange.Enabled = true;
            btnDelete.Enabled = true;
        }
        //method that disbles clicking on the add, change, delete buttons
        private void DisableAddChangeDelete()
        {
            btnAdd.Enabled = false;
            btnChange.Enabled = false;
            btnDelete.Enabled = false;
        }
        //method that enables clicking on the confirm,cancel buttons
        private void EnableConfirmCancel()
        {
            btnConfirm.Enabled = true;
            btnCancel.Enabled = true;
        }
        //method that disables clicking on the confirm,cancel buttons
        private void DisableConfirmCancel()
        {
            btnConfirm.Enabled = false;
            btnCancel.Enabled = false;
        }
        //method that deletes everything from the textboxes
        private void ClearTxtBoxes()
        {
            txtPName.Text = "";
            txtPManufacturer.Text = "";
            txtPCost.Text = "";
            txtPQuantity.Text = "";
        }
        //a method that fills text boxes with data from the selected product
        private void ShowProductTxtBoxes()
        {
            // variable that takes a value from cell id from the selected row
            int idselected = (int)dgwProducts.SelectedRows[0].Cells["productid"].Value;
            //based on the id, the product from the list is loaded
            Product selectedProduct = products.Where(x => x.ProductId == idselected).FirstOrDefault();
            //if there is selected product fill text boxes with his data
            if (selectedProduct != null)
            {
                txtPName.Text = selectedProduct.ProductName;
                txtPManufacturer.Text = selectedProduct.ProductManufacturer;
                txtPCost.Text = selectedProduct.ProductCost.ToString();
                txtPQuantity.Text = selectedProduct.ProductQuantity.ToString();
            }
        }
        //Method that show all products from database in DataGridView
        private void ShowAllProducts()
        {
            //we load all products from the database through the LoadProducts() method
            products = new Product().LoadProducts();
            //deleting all data from datagridview
            dgwProducts.Rows.Clear();
            //Fill DataGridView with products from products list
            for (int i = 0; i < products.Count; i++)
            {
                dgwProducts.Rows.Add();
                dgwProducts.Rows[i].Cells["productid"].Value = products[i].ProductId;
                dgwProducts.Rows[i].Cells["productname"].Value = products[i].ProductName;
                dgwProducts.Rows[i].Cells["productmanufacturer"].Value = products[i].ProductManufacturer;
                dgwProducts.Rows[i].Cells["productcost"].Value = products[i].ProductCost;
                dgwProducts.Rows[i].Cells["productquantity"].Value = products[i].ProductQuantity;
            }
            //By default DataGridView selects the first row of the first column which is not desirable behavior for us
            dgwProducts.CurrentCell = null;
            //delete everything from the textboxes
            ClearTxtBoxes();
            //Buttons add,change and delete are enabled
            EnableAddChangeDelete();
            //buttons confirm and cancel are disabled
            DisableConfirmCancel();
            //If there are products in database,select row and show products data in text boxes
            if (products.Count > 0)
            {
                if (selector != -1)
                    dgwProducts.Rows[selector].Selected = true;
                else
                    dgwProducts.Rows[0].Selected = true;
                ShowProductTxtBoxes();
            }
        }
        //Event after you click the add button
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearTxtBoxes();
            txtEnabled();
            EnableConfirmCancel();
            action = "add";
        }
        //Event after you click the change button
        private void btnChange_Click(object sender, EventArgs e)
        {
            //if we have selected some row do the action
            if (dgwProducts.SelectedRows.Count > 0)
            {
                //typing in text boxes is enabled
                txtEnabled();
                //Buttons confirm and cancel and enabled
                EnableConfirmCancel();
                //Buttons add change and delete are disabled 
                DisableAddChangeDelete();
                action = "change";
            }
            //otherwise print the message
            else
            {
                MessageBox.Show("There are no products in storage or you didnt selected any of them!");
            }
        }
        //Event after you click the delete button
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //If we have selected some row 
            if (dgwProducts.SelectedRows.Count > 0)
            {
                //if the user presses the yes button after the mesage box shows him the message
                if (MessageBox.Show("Do you really want to delete this product?", "DELETING", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // variable that takes a value from cell id from the selected row
                    int idselected = (int)dgwProducts.SelectedRows[0].Cells["productid"].Value;
                    //based on the id, the product from the list is loaded
                    Product selectedProduct = products.Where(x => x.ProductId == idselected).FirstOrDefault();
                    //if there is selected product delete him
                    if (selectedProduct != null)
                    {
                        selectedProduct.DeleteProduct();
                    }
                    //after deleting ,the first row is selected
                    selector = -1;
                    //Show all products from database in DataGridView
                    ShowAllProducts();
                }
            }
            //Otherwise show the message
            else
            {
                MessageBox.Show("There are no products in storage or you didnt selected any of them!");
            }
        }
        //Event after you click the confirm button
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                //if the action was add ,it will add the new product in database
                if (action == "add")
                {
                    //a new instance of the product class is created
                    Product p = new Product();
                    //fill in its fields with data from text boxes
                    p.ProductName = txtPName.Text;
                    p.ProductManufacturer = txtPManufacturer.Text;
                    p.ProductCost = decimal.Parse(txtPCost.Text);
                    p.ProductQuantity = Int32.Parse(txtPQuantity.Text);
                    //we add object to the list 
                    p.AddProduct();
                    //The last row is selected
                    selector = dgwProducts.Rows.Count;
                }
                //if the action was change,it will change product data in databse
                else if (action == "change")
                {
                    // variable that takes a value from cell id from the selected row
                    int idselected = (int)dgwProducts.SelectedRows[0].Cells["productid"].Value;
                    //based on the id, the product from the list is loaded
                    Product selectedProduct = products.Where(x => x.ProductId == idselected).FirstOrDefault();
                    //Selected product receive new data
                    selectedProduct.ProductName = txtPName.Text;
                    selectedProduct.ProductManufacturer = txtPManufacturer.Text;
                    selectedProduct.ProductCost = decimal.Parse(txtPCost.Text);
                    selectedProduct.ProductQuantity = Int32.Parse(txtPQuantity.Text);
                    //Call method to change product in database
                    selectedProduct.ChangeProduct();
                    //row of the product that was changed is selected
                    selector = dgwProducts.SelectedRows[0].Index;
                }
                //typing in text boxes is disabled again
                txtDisabled();
                //Buttons add change and delete are enabled 
                EnableAddChangeDelete();
                //buttons cancel and confirm are disabled 
                DisableConfirmCancel();
                //action is set to "" again
                action = "";
                //show all products from database in DataGridView
                ShowAllProducts();
            }
            //if there is error ,show error message to user
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Event after you click the Cancel button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Buttons add change and delete are enabled 
            EnableAddChangeDelete();
            //buttons cancel and confirm are disabled 
            DisableConfirmCancel();
            //typing in text boxes is disabled 
            txtDisabled();
        }
        //Event after you click on cell in DataGridView
        private void dgwProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgwProducts.CurrentCell != null)
            {
                dgwProducts.Rows[dgwProducts.CurrentCell.RowIndex].Selected = true;
                ShowProductTxtBoxes();
            }
        }
        //Event after you click on Report->Products in menu
        private void productsReport_Click(object sender, EventArgs e)
        {
            //Creating instance of productsReport class
            ProductsReport report = new ProductsReport();
            //Open the form in which the reports will be displayed
            report.Show();
        }
        //Event after click on Xml->ImportXML in menu
        private void importXML_Click(object sender, EventArgs e)
        {
            //Open file dialog to choose file from computer
            OpenFileDialog opDlg = new OpenFileDialog();
            //Setting the starting path
            opDlg.InitialDirectory = "C:\\";
            //Which file types can be selected
            opDlg.Filter = "xml Files (*.xml)|*.xml";
            //if we selected file and pressed ok
            if (DialogResult.OK == opDlg.ShowDialog())
            {
                //call method importXML from productXML class, that writes an xml file and stores it in the database
                ProductXML.importXML(opDlg.FileName);
            }
            //Show all products from database in DataGridView
            ShowAllProducts();
        }
        //Event after click on Xml->ExportXML in menu
        private void exportXML_Click(object sender, EventArgs e)
        { 
            //Open save file dialog to allow user to save the file 
            SaveFileDialog svDlg = new SaveFileDialog();
            //Setting the starting path
            svDlg.InitialDirectory = "C:\\";
            //Which file types can be saved
            svDlg.Filter = "xml Files (*.xml)|*.xml";
            //If the file is set
            if (DialogResult.OK == svDlg.ShowDialog())
            {
                //Call methid exportXML from productXML class ,that creates XML file and store it in computer
                ProductXML.exportXML(svDlg.FileName, products);
            }
        }
        //Method that return list of products from database that contain text from txtSearch textbox
        private List<Product> SearchProduct()
        {
            //create List of types Product 
            List<Product> products = new List<Product>();
            //create string that is query for database
            string SqlSearch = "select * from Product " +
                    "where ProductName like '%" + txtSearch.Text + "%' or ProductManufacturer like '%" + txtSearch.Text + "%' or ProductCost like '%" + txtSearch.Text + "%' or ProductQuantity like '%" + txtSearch.Text + "%'";
            //create string that is our connection to sql server
            string connectionString = "Data Source=DESKTOP-369PH90;Initial Catalog=ShopStorage;Integrated Security=True";
            //using our connection 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //create cmmand on connection
                SqlCommand command = connection.CreateCommand();
                //give command text to command
                command.CommandText = SqlSearch;
                //open connection
                connection.Open();
                //create reader that will read data from database
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //while reader has data in database to read
                    while (reader.Read())
                    {
                        //create new Product instance
                        Product p = new Product();
                        //set values ​​to its fields by reading values ​​from the database 
                        p.ProductId = Int32.Parse(reader["ProductId"].ToString());
                        p.ProductName = reader["ProductName"].ToString();
                        p.ProductManufacturer = reader["ProductManufacturer"].ToString();
                        p.ProductCost = decimal.Parse(reader["ProductCost"].ToString());
                        p.ProductQuantity = Int32.Parse(reader["ProductQuantity"].ToString());
                        //add new product to List of products
                        products.Add(p);
                    }
                }
            }
            //return List of products from database
            return products;
        }
        //event after you click on Search button
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //If there is something in search textbox to search
            if (txtSearch.Text != "")
            {
                //Store list of products that match the search in variable products
                products = SearchProduct();
                //clear rows in datagridview
                dgwProducts.Rows.Clear();
                //Fill DataGridView with products from products list
                for (int i = 0; i < products.Count; i++)
                {
                    dgwProducts.Rows.Add();
                    dgwProducts.Rows[i].Cells["productid"].Value = products[i].ProductId;
                    dgwProducts.Rows[i].Cells["productname"].Value = products[i].ProductName;
                    dgwProducts.Rows[i].Cells["productmanufacturer"].Value = products[i].ProductManufacturer;
                    dgwProducts.Rows[i].Cells["productcost"].Value = products[i].ProductCost;
                    dgwProducts.Rows[i].Cells["productquantity"].Value = products[i].ProductQuantity;
                }
                //By default DataGridView selects the first row of the first column which is not desirable behavior for us
                dgwProducts.CurrentCell = null;
                //Clear all text boxes
                ClearTxtBoxes();
                //Buttons add change and delete are enabled
                EnableAddChangeDelete();
                //buttons confirm and cancel are disabled
                DisableConfirmCancel();
                //if threre are products in list products
                if (products.Count > 0)
                {
                    //select row and show product data in txt boxes
                    if (selector != -1)
                        dgwProducts.Rows[selector].Selected = true;
                    else
                        dgwProducts.Rows[0].Selected = true;
                    ShowProductTxtBoxes();
                }

            }
            //If there is nothing to search ,show all products that are in database
            else if(txtSearch.Text=="")
            {
                ShowAllProducts();
            }

        }
       
    }
}
