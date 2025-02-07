using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace CustomerRegistration.Domain.Entities
{
    [Index(nameof(IcNumber), IsUnique = true)]
    public class Customer
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        
        [Required, MaxLength(100)]
        public required string Name { get; set; }
        [Required, MaxLength(20)]
        public required string IcNumber { get; set; }

        [Required, MaxLength(15)]
        public required string MobileNumber { get; set; }

        [MaxLength(4), MinLength(4)]
        public string? MobileNumberCode { get; set; }
        public DateTime? MobileNumberCodeExp { get; set; }

        [Required, MaxLength(255)]
        public required string EmailAddress { get; set; }

        [MaxLength(4), MinLength(4)]
        public string? EmailCode { get; set; }
        public DateTime? EmailCodeExp { get; set; }
        public bool IsPhoneVerified { get; set; } = false;
        public bool IsEmailVerified { get; set; } = false;
        public bool IsTermsAndConditionsAccepted { get; set; } = false;
        public bool IsBiometricEnabled { get; set; } = false;

        [MaxLength(64)] // TODO: PersonalData attribute can be added later.
        public string? PinCode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        #endregion Properties
    }
}
