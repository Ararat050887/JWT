namespace JWT_API_SECOND_EXPIRIENCE.Models.DTO
{
    public class RefreshTokenRequest
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
