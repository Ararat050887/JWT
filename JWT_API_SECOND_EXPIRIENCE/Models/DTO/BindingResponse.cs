namespace JWT_API_SECOND_EXPIRIENCE.Models.DTO
{
    public class BindingResponse
    {
        public int Id { get; set; }
        public string BindingId { get; set; }
        public byte IsActive { get; set; }
        public byte IsConfirmed { get; set; }
        public int MerchantId { get; set; }
        public string MerchantUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Cardholder { get; set; }
    }
}
