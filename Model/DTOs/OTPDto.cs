using System.ComponentModel.DataAnnotations;


namespace Model.DTOs
{
    public class OTPDto
    {
        public string? Email { get; set; } = string.Empty;
        public string? OTP { get; set; } = string.Empty;
    }
}
