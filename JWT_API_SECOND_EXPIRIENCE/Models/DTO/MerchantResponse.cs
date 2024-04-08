namespace JWT_API_SECOND_EXPIRIENCE.Models.DTO
{
    public class MerchantResponse
    {       
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public int BindedTo { get; set; }
            public int? MerchantId { get; set; }
            public int? TerminalId { get; set; }
        
    }
}
