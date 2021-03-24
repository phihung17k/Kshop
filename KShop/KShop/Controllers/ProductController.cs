using KShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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


        [HttpGet("addProduct")]
        
        public ActionResult AddProduct() {
            bool addSuccess = false;
            try {
                string serialize = HttpContext.Session.GetString("tempForm");
                TempForm tempForm = System.Text.Json.JsonSerializer.Deserialize<TempForm>(serialize); 

                connection.Open();

                #region
                SqlCommand command = new SqlCommand("sp_add_Product", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@productName", SqlDbType.NVarChar);
                command.Parameters.Add("@quantity", SqlDbType.Int);
                command.Parameters.Add("@price", SqlDbType.Float);
                command.Parameters.Add("@categoryId", SqlDbType.NVarChar);
                command.Parameters.Add("@image", SqlDbType.NVarChar);

                command.Parameters["@productName"].Value = tempForm.ProductName;
                command.Parameters["@quantity"].Value = tempForm.Quantity;
                command.Parameters["@price"].Value = tempForm.Price;
                command.Parameters["@categoryId"].Value = tempForm.CategoryId;
                command.Parameters["@image"].Value = tempForm.ImageLocation;
                
                int row = command.ExecuteNonQuery();
                if(row > 0) {
                    addSuccess = true;
                }
                #endregion

                connection.Close();
            } catch(Exception ex) {
                return BadRequest(error: ex);
            }
            return Ok(JsonConvert.SerializeObject(addSuccess.ToString()));
        }

        [HttpPost("updateProduct")]
        public string UpdateProduct() {
            bool addSuccess = false;
            try {
                string serialize = HttpContext.Session.GetString("tempForm");
                TempForm tempForm = System.Text.Json.JsonSerializer.Deserialize<TempForm>(serialize);

                connection.Open();

                #region
                SqlCommand command = new SqlCommand("sp_add_Product", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@productId", SqlDbType.Int);
                command.Parameters.Add("@productName", SqlDbType.NVarChar);
                command.Parameters.Add("@quantity", SqlDbType.Int);
                command.Parameters.Add("@price", SqlDbType.Float);
                command.Parameters.Add("@categoryId", SqlDbType.NVarChar);
                if(tempForm.ImageLocation != null) {
                    command.Parameters.Add("@image", SqlDbType.NVarChar);
                    command.Parameters["@image"].Value = tempForm.ImageLocation;
                }

                command.Parameters["@productId"].Value = tempForm.ProductId;
                command.Parameters["@productName"].Value = tempForm.ProductName;
                command.Parameters["@quantity"].Value = tempForm.Quantity;
                command.Parameters["@price"].Value = tempForm.Price;
                command.Parameters["@categoryId"].Value = tempForm.CategoryId;
                

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

        int GetLastProductId() {
            int result = -1;
            try {
                connection.Open();

                #region
                SqlCommand command = new SqlCommand("sp_getLastID_Product", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read()) {
                    result = reader.GetInt32("ProductId");
                }
                #endregion

                connection.Close();
            } catch(Exception) {
                throw;
            }
            return result;
        }

    }
}
