
using KShop.Models;
using Microsoft.AspNetCore.Http;
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
        public string GetAccountForLogin([FromBody] string[] value) {
            bool resultLogin = false ;
            //string username = value[0];
            //string password = value[1];
            string username = "admin";
            string password = "123456";
            connection.Open();

            #region working with procedure
            SqlCommand command = new SqlCommand("sp_check_Account", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@username", SqlDbType.NVarChar);
            command.Parameters.Add("@password", SqlDbType.NVarChar);
            command.Parameters["@username"].Value = username;
            command.Parameters["@password"].Value = password;
            #endregion

            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read()) {
                resultLogin = true;
            }
            
            connection.Close();
            if(resultLogin) {
                
                Account user = GetAccount(username);
                string currentUser = JsonConvert.SerializeObject(user);
                HttpContext.Session.SetString("user", currentUser);
                return currentUser;
            }
            return "";
        }

        Account GetAccount(string username) {
            Account result = null;
            connection.Open();
            #region working with procedure
            SqlCommand command = new SqlCommand("sp_get_Account", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@username", SqlDbType.NVarChar);
            command.Parameters["@username"].Value = username;
            #endregion

            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read()) {
                string fullname = reader.GetString("FullName");
                string address = reader.GetString("Address");
                int age = reader.GetInt32("Age");
                string gender = reader.GetString("gender");
                string role = reader.GetString("RoleName");
                result = new Account(fullname, address, age, gender, role);
            }
            connection.Close();
            return result;
        }
        
    }
}
    