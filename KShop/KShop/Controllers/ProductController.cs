using KShop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace KShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        SqlConnection connection = null;
        private readonly IConfiguration configuration;
        public ProductController(IConfiguration config)
        {
            this.configuration = config;
            connection = new SqlConnection(config.GetConnectionString("DefaultConnectionStrings"));
        }

        [HttpGet("getAllProduct")]
        public ActionResult<List<Product>> GetAllUser() {
            List<Product> listProduct = null;
            try
            {
                connection.Open();
                SqlCommand sql = new SqlCommand("select * from Product where Status=1", connection);
                SqlDataReader dr = sql.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int productId = int.Parse(dr["ProductId"].ToString() + "");
                        int categoryId = int.Parse(dr["CategoryId"].ToString() + "");
                        int quantity = int.Parse(dr["Quantity"].ToString() + "");
                        float price = float.Parse(dr["Price"].ToString() + "");
                        DateTime createdDate = DateTime.Parse(dr["CreatedDate"].ToString() + "");
                        string productName = dr["ProductName"].ToString() + "";
                        string image = dr["Image"].ToString() + "";
                        if (listProduct == null)
                        {
                            listProduct = new List<Product>();
                        }
                        listProduct.Add(new Product(productId,productName,image,quantity,price,categoryId,createdDate,true));
                    }
                }
                connection.Close();
            }
            catch { 
                
            }
           
            return listProduct;
        }

        [HttpGet("manageproducts")]
        
        public ActionResult<List<Product>> GetAllAdmin()
        {
            List<Product> listProduct = null;
            try
            {
                connection.Open();
                SqlCommand sql = new SqlCommand("select * from Product ", connection);
                SqlDataReader dr = sql.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int productId = int.Parse(dr["ProductId"].ToString() + "");
                        int categoryId = int.Parse(dr["CategoryId"].ToString() + "");
                        int quantity = int.Parse(dr["Quantity"].ToString() + "");
                        float price = float.Parse(dr["Price"].ToString() + "");
                        DateTime createdDate = DateTime.Parse(dr["CreatedDate"].ToString() + "");
                        string productName = dr["ProductName"].ToString() + "";
                        string image = dr["Image"].ToString() + "";
                        bool status = Boolean.Parse(dr["Status"].ToString() + "");
                        if (listProduct == null)
                        {
                            listProduct = new List<Product>();
                        }
                        listProduct.Add(new Product(productId, productName, image, quantity, price, categoryId, createdDate, status));
                    }
                }
                connection.Close();
            }
            catch
            {

            }

            return listProduct;
        }

        [HttpPost("addProduct")]
        public string AddProduct([FromBody] Product product) {
            bool addSuccess = false;
            try {
                connection.Open();

                #region
                SqlCommand command = new SqlCommand("sp_add_Product", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@productName", SqlDbType.NVarChar);
                command.Parameters.Add("@quantity", SqlDbType.Int);
                command.Parameters.Add("@price", SqlDbType.Float);
                command.Parameters.Add("@categoryId", SqlDbType.NVarChar);
                command.Parameters.Add("@image", SqlDbType.NVarChar);

                command.Parameters["@productName"].Value = product.ProductName;
                command.Parameters["@quantity"].Value = product.Quantity;
                command.Parameters["@price"].Value = product.Price;
                command.Parameters["@categoryId"].Value = product.CategoryId;
                command.Parameters["@image"].Value = product.Image;

                int row = command.ExecuteNonQuery();
                if(row > 0) {
                    addSuccess = true;
                }
                #endregion

                connection.Close();
            } catch(Exception) {
                throw;
            }
            return JsonConvert.SerializeObject(addSuccess.ToString());
        }

        [HttpPost("updateProduct")]
        public string UpdateProduct([FromBody] Product product) {
            bool addSuccess = false;
            try {
                connection.Open();

                #region
                SqlCommand command = new SqlCommand("sp_add_Product", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@productId", SqlDbType.Int);
                command.Parameters.Add("@productName", SqlDbType.NVarChar);
                command.Parameters.Add("@quantity", SqlDbType.Int);
                command.Parameters.Add("@price", SqlDbType.Float);
                command.Parameters.Add("@categoryId", SqlDbType.NVarChar);
                command.Parameters.Add("@image", SqlDbType.NVarChar);

                command.Parameters["@productId"].Value = product.ProductId;
                command.Parameters["@productName"].Value = product.ProductName;
                command.Parameters["@quantity"].Value = product.Quantity;
                command.Parameters["@price"].Value = product.Price;
                command.Parameters["@categoryId"].Value = product.CategoryId;
                command.Parameters["@image"].Value = product.Image;

                int row = command.ExecuteNonQuery();
                if(row > 0) {
                    addSuccess = true;
                }
                #endregion

                connection.Close();
            } catch(Exception) {
                throw;
            }
            return JsonConvert.SerializeObject(addSuccess.ToString());
        }


    }
}
