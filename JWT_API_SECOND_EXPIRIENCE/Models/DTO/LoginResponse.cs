namespace JWT_API_SECOND_EXPIRIENCE.Models.DTO
{
    public class LoginResponse : Status
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? Expiration { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public List<string>? Roles { get; set; }
    }
}
