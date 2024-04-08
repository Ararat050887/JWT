using JWT_API_SECOND_EXPIRIENCE.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace JWT_API.Controllers
{
    [Route("api/merchant")]
    [ApiController]
    [Authorize]
    public class MerchantController : Controller
    {
        private readonly string _connectionString;

        public MerchantController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("mariconn");
        }

        [HttpGet]
        [Route("GetMerchants")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUsersAndMerchants()
        {
            try
            {

                int merchantCount = 0;

                List<MerchantResponse> responses = new List<MerchantResponse>();

                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {

                    string countQuery = "SELECT COUNT(*) FROM merchants";
                    string query = "SELECT u.email, u.firstname, u.lastname, m.client_id, m.client_secret, m.binded_to , b.merchant_id " +
                               "FROM users u " +
                               "LEFT JOIN merchants m ON u.merchant_id = m.id " +
                               "LEFT JOIN bindings b ON m.binded_to = b.id";

                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    merchantCount = Convert.ToInt32(countCommand.ExecuteScalar());
                    MySqlDataReader reader = command.ExecuteReader();



                    while (reader.Read())
                    {
                        string clientId = reader.IsDBNull(reader.GetOrdinal("client_id")) ? null : reader.GetString("client_id");
                        List<int?> clientIdParts = new List<int?>();
                        if (!string.IsNullOrEmpty(clientId))
                        {
                            string[] partStrings = clientId.Split('.');
                            foreach (string partString in partStrings)
                            {
                                if (int.TryParse(partString, out int partInt))
                                {
                                    clientIdParts.Add(partInt);
                                }
                                else
                                {
                                    clientIdParts.Add(null);
                                }
                            }
                        }

                        MerchantResponse response = new MerchantResponse
                        {
                            Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                            FirstName = reader.IsDBNull(reader.GetOrdinal("firstname")) ? null : reader.GetString("firstname"),
                            LastName = reader.IsDBNull(reader.GetOrdinal("lastname")) ? null : reader.GetString("lastname"),
                            ClientId = reader.IsDBNull(reader.GetOrdinal("client_id")) ? null : reader.GetString("client_id"),
                            ClientSecret = reader.IsDBNull(reader.GetOrdinal("client_secret")) ? null : reader.GetString("client_secret"),
                            BindedTo = reader.IsDBNull(reader.GetOrdinal("merchant_id")) ? 0 : reader.GetInt32("merchant_id"),
                            MerchantId = clientIdParts.Count > 1 ? clientIdParts[1] : null,
                            TerminalId = clientIdParts.Count > 2 ? clientIdParts[2] : null
                        };

                        responses.Add(response);
                    }

                    reader.Close();
                }
                return Ok(new { merchantCount = merchantCount, responses = responses });
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorcode = "7", message = ex.Message });
            }
        }
    }

}
