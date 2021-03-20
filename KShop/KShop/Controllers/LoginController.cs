
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace KShop.Controllers {
    [Route("api/login")]
    [ApiController]
    public class LoginController: ControllerBase {

        SqlConnection connection = null;
        private readonly IConfiguration configuration;

        public LoginController(IConfiguration configuration) {
            this.configuration = configuration;
            connection = new SqlConnection(configuration.GetConnectionString("DefaultConnectionStrings"));
        }

        // POST api/<LoginController>
        [HttpPost]
        public string Post([FromBody] string[] value) {
            bool checkLogin = false;
            string username = value[0];
            string password = value[1];

            connection.Open();

            #region working with procedure
            SqlCommand command = new SqlCommand("sp_login_Account", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@username", SqlDbType.NVarChar);
            command.Parameters.Add("@password", SqlDbType.NVarChar);
            command.Parameters["@username"].Value = username;
            command.Parameters["@password"].Value = password;
            #endregion

            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read()) {
                checkLogin = true;
            }
            connection.Close();
            return JsonConvert.SerializeObject(checkLogin.ToString());
        }

        
    }
}
    