using Azure;
using JWT_API_SECOND_EXPIRIENCE.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;


namespace JWT_API_SECOND_EXPIRIENCE.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly string _connectionString;
        private readonly RoleManager<IdentityRole> _roleManager;

        public OrdersController(IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _connectionString = configuration.GetConnectionString("mariconn");
            this._roleManager = roleManager;
        }
        [HttpGet("GetOrders")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetOrders()
        {
            try
            {
                List<OrdersResponse> responses = new List<OrdersResponse>();
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    DateTime timestamp = DateTime.MinValue;

                    string Query = "SELECT * FROM orders";
                    MySqlCommand command = new MySqlCommand(Query, connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {

                        OrdersResponse response = new OrdersResponse()
                        {
                            Id = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : reader.GetInt32("id"),
                            OrderId = reader.IsDBNull(reader.GetOrdinal("order_id")) ? string.Empty : reader.GetString("order_id"),
                            Amount = reader.IsDBNull(reader.GetOrdinal("amount")) ? 0 : reader.GetDouble("amount"),
                            Currency = reader.IsDBNull(reader.GetOrdinal("currency")) ? string.Empty : reader.GetString("currency"),
                            IsPaidWithBinding = reader.IsDBNull(reader.GetOrdinal("is_paid_with_binding")) ? false : reader.GetBoolean("is_paid_with_binding"),
                            Ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? string.Empty : reader.GetString("ip"),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString("description"),
                            AuthorizationCode = reader.IsDBNull(reader.GetOrdinal("authorization_code")) ? string.Empty : reader.GetString("authorization_code"),
                            Is3Ds = reader.IsDBNull(reader.GetOrdinal("is_3ds")) ? false : reader.GetBoolean("is_3ds"),
                            LastOperationResult = reader.IsDBNull(reader.GetOrdinal("last_operation_result")) ? string.Empty : reader.GetString("last_operation_result"),
                            CardholderName = reader.IsDBNull(reader.GetOrdinal("cardholder_name")) ? string.Empty : reader.GetString("cardholder_name"),
                            PanFirstDigits = reader.IsDBNull(reader.GetOrdinal("pan_first_digits")) ? string.Empty : reader.GetString("pan_first_digits"),
                            PanLastDigits = reader.IsDBNull(reader.GetOrdinal("pan_last_digits")) ? string.Empty : reader.GetString("pan_last_digits"),
                            MerchantUserId = reader.IsDBNull(reader.GetOrdinal("merchant_user_id")) ? string.Empty : reader.GetString("merchant_user_id"),
                            ExternalId = reader.IsDBNull(reader.GetOrdinal("external_id")) ? string.Empty : reader.GetString("external_id"),
                            ExpirationDate = reader.IsDBNull(reader.GetOrdinal("expiration_date")) ? DateTime.MinValue : reader.GetDateTime("expiration_date"),
                            MerchantId = reader.IsDBNull(reader.GetOrdinal("merchant_id")) ? 0 : reader.GetInt32("merchant_id"),
                            Status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString("status"),
                            Links = reader.IsDBNull(reader.GetOrdinal("links")) ? 0 : reader.GetInt32("links"),
                            JsonParams = reader.IsDBNull(reader.GetOrdinal("json_params")) ? string.Empty : reader.GetString("json_params"),
                            OrderData = reader.IsDBNull(reader.GetOrdinal("order_data")) ? string.Empty : reader.GetString("order_data"),
                            BindingId = reader.IsDBNull(reader.GetOrdinal("binding_id")) ? string.Empty : reader.GetString("binding_id"),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? DateTime.MinValue : reader.GetDateTime("created_at"),
                            AuthorizedAt = reader.IsDBNull(reader.GetOrdinal("authorized_at")) ? DateTime.MinValue : reader.GetDateTime("authorized_at"),
                            AmountDeposited = reader.IsDBNull(reader.GetOrdinal("amount_deposited")) ? 0 : reader.GetDouble("amount_deposited"),
                            AmountRefunded = reader.IsDBNull(reader.GetOrdinal("amount_refunded")) ? 0 : reader.GetDouble("amount_refunded")
                        };
                        responses.Add(response);
                    }
                    reader.Close();
                }
                return Ok(responses);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while fetching orders: " + ex.Message);
            }
        }

        

        [HttpGet("GetOrders/{TID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrdersByTID(int TID)
        {
            try
            {
                List<OrdersResponse> responses = new List<OrdersResponse>();
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                  
                    string Query = "SELECT * FROM orders WHERE merchant_id IN (SELECT id FROM merchants WHERE client_id LIKE '%"+TID+"%')";//'%@tid%'
                    MySqlCommand command = new MySqlCommand(Query, connection);
                    //command.Parameters.Add(new MySqlParameter("tid", TID));           
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {

                        OrdersResponse response = new OrdersResponse()
                        {
                            Id = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : reader.GetInt32("id"),
                            OrderId = reader.IsDBNull(reader.GetOrdinal("order_id")) ? string.Empty : reader.GetString("order_id"),
                            Amount = reader.IsDBNull(reader.GetOrdinal("amount")) ? 0 : reader.GetDouble("amount"),
                            Currency = reader.IsDBNull(reader.GetOrdinal("currency")) ? string.Empty : reader.GetString("currency"),
                            IsPaidWithBinding = reader.IsDBNull(reader.GetOrdinal("is_paid_with_binding")) ? false : reader.GetBoolean("is_paid_with_binding"),
                            Ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? string.Empty : reader.GetString("ip"),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString("description"),
                            AuthorizationCode = reader.IsDBNull(reader.GetOrdinal("authorization_code")) ? string.Empty : reader.GetString("authorization_code"),
                            Is3Ds = reader.IsDBNull(reader.GetOrdinal("is_3ds")) ? false : reader.GetBoolean("is_3ds"),
                            LastOperationResult = reader.IsDBNull(reader.GetOrdinal("last_operation_result")) ? string.Empty : reader.GetString("last_operation_result"),
                            CardholderName = reader.IsDBNull(reader.GetOrdinal("cardholder_name")) ? string.Empty : reader.GetString("cardholder_name"),
                            PanFirstDigits = reader.IsDBNull(reader.GetOrdinal("pan_first_digits")) ? string.Empty : reader.GetString("pan_first_digits"),
                            PanLastDigits = reader.IsDBNull(reader.GetOrdinal("pan_last_digits")) ? string.Empty : reader.GetString("pan_last_digits"),
                            MerchantUserId = reader.IsDBNull(reader.GetOrdinal("merchant_user_id")) ? string.Empty : reader.GetString("merchant_user_id"),
                            ExternalId = reader.IsDBNull(reader.GetOrdinal("external_id")) ? string.Empty : reader.GetString("external_id"),
                            ExpirationDate = reader.IsDBNull(reader.GetOrdinal("expiration_date")) ? DateTime.MinValue : reader.GetDateTime("expiration_date"),
                            MerchantId = reader.IsDBNull(reader.GetOrdinal("merchant_id")) ? 0 : reader.GetInt32("merchant_id"),
                            Status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString("status"),
                            Links = reader.IsDBNull(reader.GetOrdinal("links")) ? 0 : reader.GetInt32("links"),
                            JsonParams = reader.IsDBNull(reader.GetOrdinal("json_params")) ? string.Empty : reader.GetString("json_params"),
                            OrderData = reader.IsDBNull(reader.GetOrdinal("order_data")) ? string.Empty : reader.GetString("order_data"),
                            BindingId = reader.IsDBNull(reader.GetOrdinal("binding_id")) ? string.Empty : reader.GetString("binding_id"),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? DateTime.MinValue : reader.GetDateTime("created_at"),
                            AuthorizedAt = reader.IsDBNull(reader.GetOrdinal("authorized_at")) ? DateTime.MinValue : reader.GetDateTime("authorized_at"),
                            AmountDeposited = reader.IsDBNull(reader.GetOrdinal("amount_deposited")) ? 0 : reader.GetDouble("amount_deposited"),
                            AmountRefunded = reader.IsDBNull(reader.GetOrdinal("amount_refunded")) ? 0 : reader.GetDouble("amount_refunded")
                        };
                        responses.Add(response);
                    }
                    reader.Close();
                }
                return Ok(responses);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while fetching orders: " + ex.Message);
            }
        }
    }
}
        //public async Task<IActionResult> GetOrdersByTID()
        //{
        //    try
        //    {
        //        List<OrdersResponse> responses = new List<OrdersResponse>();
        //        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        //        {
        //            string merchantQuery = "SELECT id, client_id FROM merchants";
        //            MySqlCommand commandTID = new MySqlCommand(merchantQuery, connection);
        //            connection.Open();
        //            MySqlDataReader merchantReader = commandTID.ExecuteReader();


//            while (merchantReader.Read())
//            {
//                string client_id = merchantReader.GetString("client_id");

//                string[] parts = client_id.Split('.');
//                if (parts.Length > 0)
//                {
//                    int lastNumber = int.Parse(parts[^1]);

//                    if (lastNumber == TID)
//                    {
//                        int merchantId = merchantReader.GetInt32("id");
//                        merchantReader.Close();

//                        string orderQuery = "SELECT * FROM orders WHERE merchant_id = @MerchantId";
//                        MySqlCommand commandOrder = new MySqlCommand(orderQuery, connection);
//                        commandOrder.Parameters.AddWithValue("@MerchantId", merchantId);

//                        MySqlDataReader orderReader = commandOrder.ExecuteReader();
//                        while (orderReader.Read())
//                        {
//                            OrdersResponse response = new OrdersResponse()
//                            {
//                                Id = merchantReader.IsDBNull(merchantReader.GetOrdinal("id")) ? 0 : merchantReader.GetInt32("id"),
//                                OrderId = merchantReader.IsDBNull(merchantReader.GetOrdinal("order_id")) ? string.Empty : merchantReader.GetString("order_id"),
//                                Amount = merchantReader.IsDBNull(merchantReader.GetOrdinal("amount")) ? 0 : merchantReader.GetDouble("amount"),
//                                Currency = merchantReader.IsDBNull(merchantReader.GetOrdinal("currency")) ? string.Empty : merchantReader.GetString("currency"),
//                                IsPaidWithBinding = merchantReader.IsDBNull(merchantReader.GetOrdinal("is_paid_with_binding")) ? false : merchantReader.GetBoolean("is_paid_with_binding"),
//                                Ip = merchantReader.IsDBNull(merchantReader.GetOrdinal("ip")) ? string.Empty : merchantReader.GetString("ip"),
//                                Description = merchantReader.IsDBNull(merchantReader.GetOrdinal("description")) ? string.Empty : merchantReader.GetString("description"),
//                                AuthorizationCode = merchantReader.IsDBNull(merchantReader.GetOrdinal("authorization_code")) ? string.Empty : merchantReader.GetString("authorization_code"),
//                                Is3Ds = merchantReader.IsDBNull(merchantReader.GetOrdinal("is_3ds")) ? false : merchantReader.GetBoolean("is_3ds"),
//                                LastOperationResult = merchantReader.IsDBNull(merchantReader.GetOrdinal("last_operation_result")) ? string.Empty : merchantReader.GetString("last_operation_result"),
//                                CardholderName = merchantReader.IsDBNull(merchantReader.GetOrdinal("cardholder_name")) ? string.Empty : merchantReader.GetString("cardholder_name"),
//                                PanFirstDigits = merchantReader.IsDBNull(merchantReader.GetOrdinal("pan_first_digits")) ? string.Empty : merchantReader.GetString("pan_first_digits"),
//                                PanLastDigits = merchantReader.IsDBNull(merchantReader.GetOrdinal("pan_last_digits")) ? string.Empty : merchantReader.GetString("pan_last_digits"),
//                                MerchantUserId = merchantReader.IsDBNull(merchantReader.GetOrdinal("merchant_user_id")) ? string.Empty : merchantReader.GetString("merchant_user_id"),
//                                ExternalId = merchantReader.IsDBNull(merchantReader.GetOrdinal("external_id")) ? string.Empty : merchantReader.GetString("external_id"),
//                                ExpirationDate = merchantReader.IsDBNull(merchantReader.GetOrdinal("expiration_date")) ? DateTime.MinValue : merchantReader.GetDateTime("expiration_date"),
//                                MerchantId = merchantReader.IsDBNull(merchantReader.GetOrdinal("merchant_id")) ? 0 : merchantReader.GetInt32("merchant_id"),
//                                Status = merchantReader.IsDBNull(merchantReader.GetOrdinal("status")) ? string.Empty : merchantReader.GetString("status"),
//                                Links = merchantReader.IsDBNull(merchantReader.GetOrdinal("links")) ? 0 : merchantReader.GetInt32("links"),
//                                JsonParams = merchantReader.IsDBNull(merchantReader.GetOrdinal("json_params")) ? string.Empty : merchantReader.GetString("json_params"),
//                                OrderData = merchantReader.IsDBNull(merchantReader.GetOrdinal("order_data")) ? string.Empty : merchantReader.GetString("order_data"),
//                                BindingId = merchantReader.IsDBNull(merchantReader.GetOrdinal("binding_id")) ? string.Empty : merchantReader.GetString("binding_id"),
//                                CreatedAt = merchantReader.IsDBNull(merchantReader.GetOrdinal("created_at")) ? DateTime.MinValue : merchantReader.GetDateTime("created_at"),
//                                AuthorizedAt = merchantReader.IsDBNull(merchantReader.GetOrdinal("authorized_at")) ? DateTime.MinValue : merchantReader.GetDateTime("authorized_at"),
//                                AmountDeposited = merchantReader.IsDBNull(merchantReader.GetOrdinal("amount_deposited")) ? 0 : merchantReader.GetDouble("amount_deposited"),
//                                AmountRefunded = merchantReader.IsDBNull(merchantReader.GetOrdinal("amount_refunded")) ? 0 : merchantReader.GetDouble("amount_refunded")
//                            };
//                            responses.Add(response);
//                        }
//                        orderReader.Close();
//                    }
//                }
//            }

//        }
//        return Ok(responses);
//    }
//    catch (Exception ex)
//    {
//        return BadRequest("An error occurred: " + ex.Message);
//    }
//}

//        [HttpGet("GetOrders/{TID}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> GetOrdersByTID()
//        {
//            try
//            {
//                List<OrdersResponse> responses = await GetOrdersByTIDAsync();
//                return Ok(responses);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest("An error occurred: " + ex.Message);
//            }
//        }

//        private async Task<List<OrdersResponse>> GetOrdersByTIDAsync()
//        {
//            List<OrdersResponse> responses = new List<OrdersResponse>();

//            using (MySqlConnection connection = new MySqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();

//                string merchantQuery = "SELECT id, client_id FROM merchants";
//                MySqlCommand commandTID = new MySqlCommand(merchantQuery, connection);
//                using (MySqlDataReader merchantReader = await commandTID.ExecuteReaderAsync())
//                {
//                    while (await merchantReader.ReadAsync())
//                    {
//                        string client_id = merchantReader.GetString("client_id");
//                        string[] parts = client_id.Split('.');
//                        if (parts.Length > 0)
//                        {
//                            int lastNumber = int.Parse(parts[^1]);
//                            if (lastNumber == TID)
//                            {
//                                int merchantId = merchantReader.GetInt32("id");

//                                // Execute the query for orders
//                                string orderQuery = "SELECT * FROM orders WHERE merchant_id = @MerchantId";
//                                MySqlCommand commandOrder = new MySqlCommand(orderQuery, connection);
//                                commandOrder.Parameters.AddWithValue("@MerchantId", merchantId);

//                                using (MySqlDataReader orderReader = await commandOrder.ExecuteReaderAsync())
//                                {
//                                    while (await orderReader.ReadAsync())
//                                    {
//                                        OrdersResponse response = CreateOrdersResponse(orderReader);
//                                        responses.Add(response);
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }

//            return responses;
//        }

//        private OrdersResponse CreateOrdersResponse(MySqlDataReader reader)
//        {
//            OrdersResponse response = new OrdersResponse()
//            {
//                Id = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : reader.GetInt32("id"),
//                OrderId = reader.IsDBNull(reader.GetOrdinal("order_id")) ? string.Empty : reader.GetString("order_id"),
//                Amount = reader.IsDBNull(reader.GetOrdinal("amount")) ? 0 : reader.GetDouble("amount"),
//                Currency = reader.IsDBNull(reader.GetOrdinal("currency")) ? string.Empty : reader.GetString("currency"),
//                IsPaidWithBinding = reader.IsDBNull(reader.GetOrdinal("is_paid_with_binding")) ? false : reader.GetBoolean("is_paid_with_binding"),
//                Ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? string.Empty : reader.GetString("ip"),
//                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString("description"),
//                AuthorizationCode = reader.IsDBNull(reader.GetOrdinal("authorization_code")) ? string.Empty : reader.GetString("authorization_code"),
//                Is3Ds = reader.IsDBNull(reader.GetOrdinal("is_3ds")) ? false : reader.GetBoolean("is_3ds"),
//                LastOperationResult = reader.IsDBNull(reader.GetOrdinal("last_operation_result")) ? string.Empty : reader.GetString("last_operation_result"),
//                CardholderName = reader.IsDBNull(reader.GetOrdinal("cardholder_name")) ? string.Empty : reader.GetString("cardholder_name"),
//                PanFirstDigits = reader.IsDBNull(reader.GetOrdinal("pan_first_digits")) ? string.Empty : reader.GetString("pan_first_digits"),
//                PanLastDigits = reader.IsDBNull(reader.GetOrdinal("pan_last_digits")) ? string.Empty : reader.GetString("pan_last_digits"),
//                MerchantUserId = reader.IsDBNull(reader.GetOrdinal("merchant_user_id")) ? string.Empty : reader.GetString("merchant_user_id"),
//                ExternalId = reader.IsDBNull(reader.GetOrdinal("external_id")) ? string.Empty : reader.GetString("external_id"),
//                ExpirationDate = reader.IsDBNull(reader.GetOrdinal("expiration_date")) ? DateTime.MinValue : reader.GetDateTime("expiration_date"),
//                MerchantId = reader.IsDBNull(reader.GetOrdinal("merchant_id")) ? 0 : reader.GetInt32("merchant_id"),
//                Status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString("status"),
//                Links = reader.IsDBNull(reader.GetOrdinal("links")) ? 0 : reader.GetInt32("links"),
//                JsonParams = reader.IsDBNull(reader.GetOrdinal("json_params")) ? string.Empty : reader.GetString("json_params"),
//                OrderData = reader.IsDBNull(reader.GetOrdinal("order_data")) ? string.Empty : reader.GetString("order_data"),
//                BindingId = reader.IsDBNull(reader.GetOrdinal("binding_id")) ? string.Empty : reader.GetString("binding_id"),
//                CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? DateTime.MinValue : reader.GetDateTime("created_at"),
//                AuthorizedAt = reader.IsDBNull(reader.GetOrdinal("authorized_at")) ? DateTime.MinValue : reader.GetDateTime("authorized_at"),
//                AmountDeposited = reader.IsDBNull(reader.GetOrdinal("amount_deposited")) ? 0 : reader.GetDouble("amount_deposited"),
//                AmountRefunded = reader.IsDBNull(reader.GetOrdinal("amount_refunded")) ? 0 : reader.GetDouble("amount_refunded")
//            };
//            return response;
//        }

//    }
//}
//ExpirationDate = reader.IsDBNull(reader.GetOrdinal("expiration_date")) ? string.Empty : reader.GetString("expiration_date"),
//ExpirationDate = reader.IsDBNull(reader.GetOrdinal("expiration_date")) ? (DateTime?)null : DateTime.Parse(reader.GetString("expiration_date")),
//ExpirationDate = reader.IsDBNull(reader.GetOrdinal("expiration_date")) ? null : Timestamp.FromDateTime(reader.GetDateTime("expiration_date")),//(reader.GetDateTime("expiration_date") - timestamp),
//CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? (DateTime?)null : DateTime.Parse(reader.GetString("created_at")),
//CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? null : Timestamp.FromDateTime(reader.GetDateTime("created_at")),//reader.GetTim("created_at"),
//AuthorizedAt = reader.IsDBNull(reader.GetOrdinal("authorized_at")) ? (DateTime?)null : DateTime.Parse(reader.GetString("authorized_at")),
////AuthorizedAt = reader.IsDBNull(reader.GetOrdinal("authorized_at")) ? null : Timestamp.FromDateTime(reader.GetDateTime("authorized_at")),
///


//OrdersResponse response = new OrdersResponse()
//{
//    Id = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : reader.GetInt32("id"),
//    OrderId = reader.IsDBNull(reader.GetOrdinal("order_id")) ? string.Empty : reader.GetString("order_id"),
//    Amount = reader.IsDBNull(reader.GetOrdinal("amount")) ? 0 : reader.GetDouble("amount"),
//    Currency = reader.IsDBNull(reader.GetOrdinal("currency")) ? string.Empty : reader.GetString("currency"),
//    IsPaidWithBinding = reader.IsDBNull(reader.GetOrdinal("is_paid_with_binding")) ? false : reader.GetBoolean("is_paid_with_binding"),
//    Ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? string.Empty : reader.GetString("ip"),
//    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString("description"),
//    AuthorizationCode = reader.IsDBNull(reader.GetOrdinal("authorization_code")) ? string.Empty : reader.GetString("authorization_code"),
//    Is3Ds = reader.IsDBNull(reader.GetOrdinal("is_3ds")) ? false : reader.GetBoolean("is_3ds"),
//    LastOperationResult = reader.IsDBNull(reader.GetOrdinal("last_operation_result")) ? string.Empty : reader.GetString("last_operation_result"),
//    CardholderName = reader.IsDBNull(reader.GetOrdinal("cardholder_name")) ? string.Empty : reader.GetString("cardholder_name"),
//    PanFirstDigits = reader.IsDBNull(reader.GetOrdinal("pan_first_digits")) ? string.Empty : reader.GetString("pan_first_digits"),
//    PanLastDigits = reader.IsDBNull(reader.GetOrdinal("pan_last_digits")) ? string.Empty : reader.GetString("pan_last_digits"),
//    MerchantUserId = reader.IsDBNull(reader.GetOrdinal("merchant_user_id")) ? string.Empty : reader.GetString("merchant_user_id"),
//    ExternalId = reader.IsDBNull(reader.GetOrdinal("external_id")) ? string.Empty : reader.GetString("external_id"),
//    ExpirationDate = reader.IsDBNull(reader.GetOrdinal("expiration_date")) ? DateTime.MinValue : reader.GetDateTime("expiration_date"),
//    MerchantId = reader.IsDBNull(reader.GetOrdinal("merchant_id")) ? 0 : reader.GetInt32("merchant_id"),
//    Status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString("status"),
//    Links = reader.IsDBNull(reader.GetOrdinal("links")) ? 0 : reader.GetInt32("links"),
//    JsonParams = reader.IsDBNull(reader.GetOrdinal("json_params")) ? string.Empty : reader.GetString("json_params"),
//    OrderData = reader.IsDBNull(reader.GetOrdinal("order_data")) ? string.Empty : reader.GetString("order_data"),
//    BindingId = reader.IsDBNull(reader.GetOrdinal("binding_id")) ? string.Empty : reader.GetString("binding_id"),
//    CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? DateTime.MinValue : reader.GetDateTime("created_at"),
//    AuthorizedAt = reader.IsDBNull(reader.GetOrdinal("authorized_at")) ? DateTime.MinValue : reader.GetDateTime("authorized_at"),
//    AmountDeposited = reader.IsDBNull(reader.GetOrdinal("amount_deposited")) ? 0 : reader.GetDouble("amount_deposited"),
//    AmountRefunded = reader.IsDBNull(reader.GetOrdinal("amount_refunded")) ? 0 : reader.GetDouble("amount_refunded")
//};
//responses.Add(response);

//Id = merchantReader.IsDBNull(merchantReader.GetOrdinal("id")) ? 0 : merchantReader.GetInt32("id"),
//                OrderId = merchantReader.IsDBNull(merchantReader.GetOrdinal("order_id")) ? string.Empty : merchantReader.GetString("order_id"),
//                Amount = merchantReader.IsDBNull(merchantReader.GetOrdinal("amount")) ? 0 : merchantReader.GetDouble("amount"),
//                Currency = merchantReader.IsDBNull(merchantReader.GetOrdinal("currency")) ? string.Empty : merchantReader.GetString("currency"),
//                IsPaidWithBinding = merchantReader.IsDBNull(merchantReader.GetOrdinal("is_paid_with_binding")) ? false : merchantReader.GetBoolean("is_paid_with_binding"),
//                Ip = merchantReader.IsDBNull(merchantReader.GetOrdinal("ip")) ? string.Empty : merchantReader.GetString("ip"),
//                Description = merchantReader.IsDBNull(merchantReader.GetOrdinal("description")) ? string.Empty : merchantReader.GetString("description"),
//                AuthorizationCode = merchantReader.IsDBNull(merchantReader.GetOrdinal("authorization_code")) ? string.Empty : merchantReader.GetString("authorization_code"),
//                Is3Ds = merchantReader.IsDBNull(merchantReader.GetOrdinal("is_3ds")) ? false : merchantReader.GetBoolean("is_3ds"),
//                LastOperationResult = merchantReader.IsDBNull(merchantReader.GetOrdinal("last_operation_result")) ? string.Empty : merchantReader.GetString("last_operation_result"),
//                CardholderName = merchantReader.IsDBNull(merchantReader.GetOrdinal("cardholder_name")) ? string.Empty : merchantReader.GetString("cardholder_name"),
//                PanFirstDigits = merchantReader.IsDBNull(merchantReader.GetOrdinal("pan_first_digits")) ? string.Empty : merchantReader.GetString("pan_first_digits"),
//                PanLastDigits = merchantReader.IsDBNull(merchantReader.GetOrdinal("pan_last_digits")) ? string.Empty : merchantReader.GetString("pan_last_digits"),
//                MerchantUserId = merchantReader.IsDBNull(merchantReader.GetOrdinal("merchant_user_id")) ? string.Empty : merchantReader.GetString("merchant_user_id"),
//                ExternalId = merchantReader.IsDBNull(merchantReader.GetOrdinal("external_id")) ? string.Empty : merchantReader.GetString("external_id"),
//                ExpirationDate = merchantReader.IsDBNull(merchantReader.GetOrdinal("expiration_date")) ? DateTime.MinValue : merchantReader.GetDateTime("expiration_date"),
//                MerchantId = merchantReader.IsDBNull(merchantReader.GetOrdinal("merchant_id")) ? 0 : merchantReader.GetInt32("merchant_id"),
//                Status = merchantReader.IsDBNull(merchantReader.GetOrdinal("status")) ? string.Empty : merchantReader.GetString("status"),
//                Links = merchantReader.IsDBNull(merchantReader.GetOrdinal("links")) ? 0 : merchantReader.GetInt32("links"),
//                JsonParams = merchantReader.IsDBNull(merchantReader.GetOrdinal("json_params")) ? string.Empty : merchantReader.GetString("json_params"),
//                OrderData = merchantReader.IsDBNull(merchantReader.GetOrdinal("order_data")) ? string.Empty : merchantReader.GetString("order_data"),
//                BindingId = merchantReader.IsDBNull(merchantReader.GetOrdinal("binding_id")) ? string.Empty : merchantReader.GetString("binding_id"),
//                CreatedAt = merchantReader.IsDBNull(merchantReader.GetOrdinal("created_at")) ? DateTime.MinValue : merchantReader.GetDateTime("created_at"),
//                AuthorizedAt = merchantReader.IsDBNull(merchantReader.GetOrdinal("authorized_at")) ? DateTime.MinValue : merchantReader.GetDateTime("authorized_at"),
//                AmountDeposited = merchantReader.IsDBNull(merchantReader.GetOrdinal("amount_deposited")) ? 0 : merchantReader.GetDouble("amount_deposited"),
//                AmountRefunded = merchantReader.IsDBNull(merchantReader.GetOrdinal("amount_refunded")) ? 0 : merchantReader.GetDouble("amount_refunded")