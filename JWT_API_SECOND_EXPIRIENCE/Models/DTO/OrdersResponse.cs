namespace JWT_API_SECOND_EXPIRIENCE.Models.DTO
{
    public class OrdersResponse
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public bool IsPaidWithBinding { get; set; }
        public string Ip { get; set; }
        public string Description { get; set; }
        public string AuthorizationCode { get; set; }
        public bool Is3Ds { get; set; }
        public string LastOperationResult { get; set; }
        public string CardholderName { get; set; }
        public string PanFirstDigits { get; set; }
        public string PanLastDigits { get; set; }
        public string MerchantUserId { get; set; }
        public string ExternalId { get; set; }
        public DateTime ExpirationDate { get; set; }//
        public int MerchantId { get; set; }
        public string Status { get; set; } 
        public int Links { get; set; }
        public string JsonParams { get; set; }
        public string OrderData { get; set; } 
        public string BindingId { get; set; }
        public DateTime CreatedAt { get; set; }//
        public DateTime AuthorizedAt { get; set; }//
        public double AmountDeposited { get; set; }
        public double AmountRefunded { get; set; }

    }
}
