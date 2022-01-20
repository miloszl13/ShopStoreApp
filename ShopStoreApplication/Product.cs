using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopStoreApplication
{
    class Product
    {
        //attributes that each object will have
        private int id;
        private string productName;
        private string productManufacturer;
        private decimal productCost;
        private int productQuantity;

        //Properties which allow you to get and set values ​​to the id field
        public int ProductId
        {
            get { return id; }
            set { id = value; }
        }
        //Properties which allow you to get and set values ​​to the product name field
        public string ProductName
        {
            get { return productName; }
            set
            {
                //if user doesnt enter product name ,throw an error
                if(value == "")
                {
                    throw new Exception("You must enter product name !");
                }
                productName = value;
            }
        }
        //Properties which allow you to get and set values ​​to the product manufacturer field
        public string ProductManufacturer
        {
            get { return productManufacturer; }
            set
            {
                //if user doesnt enter product manufacturer ,throw an error
                if (value=="")
                {
                    throw new Exception("You must enter product manufacturer!");
                }
                productManufacturer = value;
            }
        }
        //Properties which allow you to get and set values ​​to the product cost field
        public decimal ProductCost
        {
            get { return productCost; }
            set
            {
                //if user tries to set 0 or number less than zero for product cost,it will throw an error
                if(value <= 0)
                {
                    throw new Exception("Product must cost more than 0e!");
                }
                productCost = value;
            }
        }
        //Properties which allow you to get and set values ​​to the product Quantity field
        public int ProductQuantity
        {
            get { return productQuantity; }
            set
            {
                //if user tries to set 0 or number less than zero for product quantity ,it will throw an error
                if(value <= 0)
                {
                    throw new Exception("You can't add 0 or less than 0 products!");
                }
                productQuantity = value;
            }
        }
        //create variable that will hold the connection string to database
        private string connectionString = "Data Source=DESKTOP-369PH90;Initial Catalog=ShopStorage;Integrated Security=True";
        //Method that adds the product to database
        public void AddProduct()
        {
            //creates variable that holds sql query to add products to the database
            string SqlAdd =
                "INSERT INTO Product " +
                "(ProductName,ProductManufacturer,ProductCost,ProductQuantity) VALUES " +
                "(@pname,@pmanufacturer,@pcost,@pquantity);";
            //set the connection 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //create command and add command text to the command
                SqlCommand command = connection.CreateCommand();
                command.CommandText = SqlAdd;
                //assign values ​​to parameters in the query
                command.Parameters.Add(new SqlParameter("@pname", ProductName));
                command.Parameters.Add(new SqlParameter("@pmanufacturer", ProductManufacturer));
                command.Parameters.Add(new SqlParameter("@pcost", ProductCost));
                command.Parameters.Add(new SqlParameter("@pquantity", ProductQuantity));
                //open connection 
                connection.Open();
                //execute query
                command.ExecuteNonQuery();
            }
        }
        //method that change product data in database
        public void ChangeProduct()
        {
            //creates variable that holds sql query for changing products to the database
            string SqlChange =
                "UPDATE Product " +
                "SET ProductName=@pname,ProductManufacturer=@pmanufacturer,ProductCost=@pcost,ProductQuantity=@pquantity " +
                "WHERE ProductId = @pid;";
            //set the connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //create command and add text to  the command
                SqlCommand command = connection.CreateCommand();
                command.CommandText = SqlChange;
                //assign values to parameters in the query
                command.Parameters.Add(new SqlParameter("@pid", ProductId));
                command.Parameters.Add(new SqlParameter("@pname", ProductName));
                command.Parameters.Add(new SqlParameter("@pmanufacturer", ProductManufacturer));
                command.Parameters.Add(new SqlParameter("@pcost", ProductCost));
                command.Parameters.Add(new SqlParameter("@pquantity", ProductQuantity));
                //open connection
                connection.Open();
                //execute query
                command.ExecuteNonQuery();
            }
        }
        //method that delete product from database
        public void DeleteProduct()
        {
            //create variable that holds sql query for deleting product from database
            string SqlDelete =
                "DELETE FROM Product WHERE ProductId=@pid;";
            //set the connection 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //create command and add text to the command
                SqlCommand command = connection.CreateCommand();
                command.CommandText = SqlDelete;
                //assign values to parameters in the query
                command.Parameters.Add(new SqlParameter("@pid", ProductId));
                //open connection
                connection.Open();
                //execute query
                command.ExecuteNonQuery();
            }
        }
        //method that returns List of all products from database
        public List<Product> LoadProducts()
        {
            //creates a product list type variable
            List<Product> products = new List<Product>();
            //create variable that hold sql query for selecting all products in database
            string SqlLoad = "SELECT * FROM Product;";
            //set the connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //create command and add text to the command
                SqlCommand command = connection.CreateCommand();
                command.CommandText = SqlLoad;
                //open connection
                connection.Open();
                //create sql data reader that reads data from database
                using(SqlDataReader reader=command.ExecuteReader())
                {
                    //while reader has what to read in database
                    while(reader.Read())
                    {
                        //create new instance of Product class
                        Product p = new Product();
                        //set values ​​to its fields by reading values ​​from the database 
                        p.ProductId = Int32.Parse(reader["ProductId"].ToString());
                        p.ProductName = reader["ProductName"].ToString();
                        p.ProductManufacturer = reader["ProductManufacturer"].ToString();
                        p.ProductCost =decimal.Parse(reader["ProductCost"].ToString());
                        p.ProductQuantity = Int32.Parse(reader["ProductQuantity"].ToString());
                        //add this object to products list
                        products.Add(p);
                    }    
                }
            }
            //return list of products
            return products;
        }

    }
}
