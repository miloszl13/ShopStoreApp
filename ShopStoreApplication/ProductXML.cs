using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ShopStoreApplication
{
    //class that allows working with XMl
    class ProductXML
    {
        //method that import Xml from path that is set as a parameter
        public static void importXML(string path)
        {
            //Creating instances that represents the xml document 
            XmlDocument xmlDoc = new XmlDocument();
            //Load document from the specified path
            xmlDoc.Load(path);
            //Create collection of nodes based on xml document.Collection elements are stored in tag Product
            XmlNodeList productsNodes = xmlDoc.GetElementsByTagName("Product");
            //Iterate through every element in collection
            foreach(XmlNode productNode in productsNodes)
            {
                //Create instance of Product class
                Product p = new Product();
                //Assign values to product fields by reading values of internal nodes in xml
                p.ProductName = productNode.ChildNodes[0].InnerText;
                p.ProductManufacturer = productNode.ChildNodes[1].InnerText;
                p.ProductCost = Convert.ToDecimal(productNode.ChildNodes[2].InnerText);
                p.ProductQuantity = Convert.ToInt32(productNode.ChildNodes[3].InnerText);
                //Add product to database
                p.AddProduct();
            }
        }
        //Method that saves data as XML document on the given path,method returns true if file is successfully saved
        public static Boolean exportXML(string path,List<Product> products)
        {
            //Creating instances that represents the xml document 
            XmlDocument xmlDoc = new XmlDocument();
            //Create instance of class that allows writting in xml document,text will be encoded according to the UTF8 standard
            XmlTextWriter xmlWriter = new XmlTextWriter(path, System.Text.Encoding.UTF8);
            //Enter xml header file
            xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
            //Enter start element
            xmlWriter.WriteStartElement("Products");
            //close writer.Document now represent regular xml document with header and start element so it can be loaded using xml load()
            xmlWriter.Close();
            xmlDoc.Load(path);
            //Iterate through products in list
            foreach(Product p in products)
            {
                //Create xml node "Product"
                XmlElement productNode = xmlDoc.CreateElement("Product");

                //Create xml node "ProductName"
                XmlElement productName = xmlDoc.CreateElement("ProductName");
                //Set its inner text to Product name from product 
                productName.InnerText = p.ProductName;
                //Add node in productNode
                productNode.AppendChild(productName);

                //create xml node "ProductManufacturer"
                XmlElement productManufacturer = xmlDoc.CreateElement("ProductManufacturer");
                //Set its inner text to Product manufacturer from product
                productManufacturer.InnerText = p.ProductManufacturer;
                //Add node in productNode
                productNode.AppendChild(productManufacturer);

                //create xml node "ProductCost"
                XmlElement productCost = xmlDoc.CreateElement("ProductCost");
                //Set its inner text to Product Cost from Product
                productCost.InnerText = p.ProductCost.ToString();
                //Add node in productNode
                productNode.AppendChild(productCost);
                 
                //create xml node "ProductQuantity"
                XmlElement productQuantity = xmlDoc.CreateElement("ProductQuantity");
                //Set its inner text to Product Quantity from product
                productQuantity.InnerText = p.ProductQuantity.ToString();
                //Add node in productNode
                productNode.AppendChild(productQuantity);

                //Add node to xml document
                xmlDoc.DocumentElement.InsertAfter(productNode, xmlDoc.DocumentElement.LastChild);
            }
            //Save document on the path
            xmlDoc.Save(path);
            return true;
        }
    }
}
