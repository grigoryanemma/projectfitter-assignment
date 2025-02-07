using System.ComponentModel.DataAnnotations;

using CustomerRegistration.Application.Common.Constants;

namespace CustomerRegistration.Application.DTOs
{
    public class CustomerRegistrationDTO : BaseRequestDTO
    {
        #region Properties

        [Required, MaxLength(100)]
        public required string Name { get; set; }
        
        [Required, MaxLength(15)]
        [Phone(ErrorMessage = ValidationMessages.InvalidMobileNumberFormat)]
        public required string MobileNumber { get; set; }

        [Required, MaxLength(255)]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        public required string EmailAddress { get; set; }
        public bool IsPhoneVerified { get; set; } = false;
        public bool IsEmailVerified { get; set; } = false;
        public bool IsTermsAndConditionsAccepted { get; set; } = false;
        public bool IsBiometricEnabled { get; set; } = false;
        public bool IsPinCodeAvailable { get; set; } = false;

        #endregion Properties
    }
}
