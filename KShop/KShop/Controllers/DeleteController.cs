using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController: ControllerBase {
        SqlConnection connection = null;
        private readonly IConfiguration configuration;
        public DeleteController(IConfiguration config) {
            this.configuration = config;
            connection = new SqlConnection(config.GetConnectionString("DefaultConnectionStrings"));
        }
        [HttpPost("delete-product")]
        public string DeleteProduct(int productId) {
            bool result = false;
            string txtConnection = configuration.GetConnectionString("DefaultConnectionStrings");
            SqlConnection connection = new SqlConnection(txtConnection);
            try {
                connection.Open();
                string sql = "UPDATE dbo.Product SET Status = 0 WHERE ProductId = " + productId;
                SqlCommand command = new SqlCommand(sql, connection);
                result = command.ExecuteNonQuery() > 0;
                connection.Close();
            } catch {
            }
            return JsonConvert.SerializeObject(result.ToString());
        }
    }
}
