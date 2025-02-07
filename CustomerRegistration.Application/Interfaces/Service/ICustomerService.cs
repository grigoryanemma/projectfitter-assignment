using CustomerRegistration.Application.DTOs;

namespace CustomerRegistration.Application.Interfaces.Service
{
    public interface ICustomerService
    {
        Task<CustomerRegistrationDTO> RegisterCustomerAsync(CustomerRegistrationDTO customerDto);
        Task VerifyMobileCodeAsync(VerificationDTO dto);
        Task VerifyEmailCodeAsync(VerificationDTO dto);
        Task AcceptTermsAndConditions(VerificationDTO dto);
        Task CreatePinAsync(VerificationDTO dto);
        Task ConfirmPinAsync(VerificationDTO dto);
        Task EnableBiometricLoginAsync(VerificationDTO dto);
        Task<CustomerRegistrationDTO> LoginCustomerAsync(string icNumber);
        Task<CustomerRegistrationDTO> GetCustomerAsync(string icNumber);
    }
}
