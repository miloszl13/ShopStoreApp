using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopStoreApplication
{
    public partial class ProductsReport : Form
    {
        public ProductsReport()
        {
            InitializeComponent();
        }

        private void ProductsReport_Load(object sender, EventArgs e)
        {
            //Data that will be displayed is list of producst that are loaded from database,using LoadProducts() method from Product class
            ProductBindingSource.DataSource = new Product().LoadProducts();
            this.reportViewer1.RefreshReport();
        }
    }
}
