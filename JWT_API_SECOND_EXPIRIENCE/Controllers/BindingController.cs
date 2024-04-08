using JWT_API_SECOND_EXPIRIENCE.Models.Domain;
using JWT_API_SECOND_EXPIRIENCE.Models;
using JWT_API_SECOND_EXPIRIENCE.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace JWT_API_SECOND_EXPIRIENCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class BindingController : Controller
    {
        private readonly string _connectionString;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BindingController(IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _connectionString = configuration.GetConnectionString("mariconn");
            this._roleManager = roleManager;
        }

        [HttpGet("GetBindings")]
        //[Route("api/getBinding")]
        [Authorize(Roles="Admin")]

        public async Task<IActionResult> GetBindings()
        {
            try
            {
                List<BindingResponse> responses = new List<BindingResponse>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "SELECT b.id, b.binding_id, b.is_active, b.is_confirmed," +
                " b.merchant_id, b.merchant_user_id, b.created_at, b.cardholder " +
                "FROM bindings b";

                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BindingResponse response = new BindingResponse()
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : reader.GetInt32("id"),
                        BindingId = reader.IsDBNull(reader.GetOrdinal("binding_id")) ? string.Empty : reader.GetString("binding_id"),
                        IsActive = reader.IsDBNull(reader.GetOrdinal("is_active")) ? (byte)0 : reader.GetByte("is_active"),
                        IsConfirmed = reader.IsDBNull(reader.GetOrdinal("is_confirmed")) ? (byte)0 : reader.GetByte("is_confirmed"),
                        MerchantId = reader.IsDBNull(reader.GetOrdinal("merchant_id")) ? 0 : reader.GetInt32("merchant_id"),
                        MerchantUserId = reader.IsDBNull(reader.GetOrdinal("merchant_user_id")) ? string.Empty : reader.GetString("merchant_user_id"),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? DateTime.MinValue : reader.GetDateTime("created_at"),
                        Cardholder = reader.IsDBNull(reader.GetOrdinal("cardholder")) ? string.Empty : reader.GetString("cardholder")

                    };
                    responses.Add(response);
                }
                reader.Close();
            }
            return Ok(responses);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while fetching bindings: " + ex.Message);
            }
        }
    }
}
