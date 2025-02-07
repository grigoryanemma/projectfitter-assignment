using System.ComponentModel.DataAnnotations;

namespace CustomerRegistration.Application.DTOs
{
    public class VerificationDTO : BaseRequestDTO
    {
        #region Properties

        [MaxLength(4), MinLength(4)]
        public string? VerificationCode { get; set; }

        [MaxLength(6), MinLength(6)]
        public string? PinCode { get; set; }
        public bool IsTermsAndConditionsAccepted  { get; set; } = false;

        #endregion Properties
    }
}
