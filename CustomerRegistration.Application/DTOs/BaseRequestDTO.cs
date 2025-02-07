using System.ComponentModel.DataAnnotations;

namespace CustomerRegistration.Application.DTOs
{
    public class BaseRequestDTO
    {
        #region Properties

        [Required, MaxLength(20)]
        public required string IcNumber { get; set; }

        #endregion Properties
    }
}
