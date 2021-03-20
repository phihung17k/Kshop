
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace KShop.Controllers {
    [Route("api/checkout")]
    [ApiController]
    public class CheckoutController: ControllerBase {

        SqlConnection connection = null;
        private readonly IConfiguration configuration;
        public CheckoutController(IConfiguration configuration) {
            this.configuration = configuration;
            connection = new SqlConnection(configuration.GetConnectionString("DefaultConnectionStrings"));
        }

        

    }
}
