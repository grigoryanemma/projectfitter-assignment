using CustomerRegistration.Application.Common.Utilities;
using CustomerRegistration.Application.DTOs;
using CustomerRegistration.Application.Interfaces.Repository;
using CustomerRegistration.Application.Interfaces.Service;
using CustomerRegistration.Application.Security;

using CustomerRegistration.Domain.Entities;

namespace CustomerRegistration.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IHashingService _hashingService;

        public CustomerService(ICustomerRepository customerRepository, IHashingService hashingService)
        {
            _customerRepository = customerRepository;
            _hashingService = hashingService;
        }

        private async Task<Customer> GetCustomerByIcNumberAsync(string? icNumber)
        {
            if (string.IsNullOrEmpty(icNumber))
            {
                throw new KeyNotFoundException("IC number cannot be empty");
            }

            return await _customerRepository.GetByIcNumber(icNumber)
                   ?? throw new KeyNotFoundException($"Customer with IC Number {icNumber} is not found.");
        }

        #region Register
        public async Task<CustomerRegistrationDTO> RegisterCustomerAsync(CustomerRegistrationDTO customerDto)
        {
            if (string.IsNullOrEmpty(customerDto.IcNumber))
            {
                throw new KeyNotFoundException("IC number cannot be empty");
            }

            Customer? customer = await _customerRepository.GetByIcNumber(customerDto.IcNumber);

            if (customer != null)
            {
                throw new Exception("Account Already exists. There is account registered with the IC number. Please login to continue.");
            }

            string mobileCode = Utility.GenerateRandomCode();

            customer = new Customer
            {
                Name = customerDto.Name,
                IcNumber = customerDto.IcNumber,
                MobileNumber = customerDto.MobileNumber,
                EmailAddress = customerDto.EmailAddress,
                MobileNumberCode = mobileCode,
                MobileNumberCodeExp = DateTime.UtcNow.AddMinutes(20),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();

            return new CustomerRegistrationDTO
            {
                Name = customer.Name,
                MobileNumber = customer.MobileNumber,
                EmailAddress = customer.EmailAddress,
                IcNumber = customer.IcNumber,
            };
        }

        #endregion Register

        #region VerifyMobileCode
        public async Task VerifyMobileCodeAsync(VerificationDTO dto)
        {
            if (string.IsNullOrEmpty(dto.VerificationCode))
            {
                throw new KeyNotFoundException("Mobile number code can not be empty");
            }

            Customer? customer = await GetCustomerByIcNumberAsync(dto.IcNumber);

            if (customer.MobileNumberCode != dto.VerificationCode)
            {
                throw new InvalidOperationException("Invalid code.");
            }

            if (customer.MobileNumberCodeExp < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Code expired.");
            }

            customer.IsPhoneVerified = true;
            customer.MobileNumberCode = null;
            customer.MobileNumberCodeExp = null;

            string emailCode = Utility.GenerateRandomCode();
            customer.EmailCode = emailCode;

            await _customerRepository.SaveChangesAsync();
        }

        #endregion VerifyMobileCode

        #region VerifyEmailCode
        public async Task VerifyEmailCodeAsync(VerificationDTO dto)
        {
            if (string.IsNullOrEmpty(dto.VerificationCode))
            {
                throw new KeyNotFoundException("Email code can not be empty");
            }

            Customer customer = await GetCustomerByIcNumberAsync(dto.IcNumber);

            if (customer.EmailCode != dto.VerificationCode)
            {
                throw new InvalidOperationException("Invalid code.");
            }

            if (customer.EmailCodeExp < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Code expired.");
            }

            customer.IsEmailVerified = true;
            customer.EmailCode = null;
            customer.EmailCodeExp = null;

            await _customerRepository.SaveChangesAsync();
        }

        #endregion VerifyEmailCode

        #region AcceptTermsAndConditions
        public async Task AcceptTermsAndConditions(VerificationDTO dto)
        {
            Customer customer = await GetCustomerByIcNumberAsync(dto.IcNumber);

            if (!customer.IsPhoneVerified)
            {
                throw new InvalidOperationException("Mobile number verification is required before accepting the terms and conditions.");
            }

            if (!customer.IsEmailVerified)
            {
                throw new InvalidOperationException("Email verification is required before accepting the terms and conditions.");
            }

            customer.IsTermsAndConditionsAccepted = dto.IsTermsAndConditionsAccepted;

            await _customerRepository.SaveChangesAsync();
        }

        #endregion AcceptTermsAndConditions

        #region CreatePin
        public async Task CreatePinAsync(VerificationDTO dto)
        {
            if (string.IsNullOrEmpty(dto.PinCode))
            {
                throw new KeyNotFoundException("PIN can not be empty");
            }

            Customer customer = await GetCustomerByIcNumberAsync(dto.IcNumber);

            if (!customer.IsTermsAndConditionsAccepted)
            {
                throw new InvalidOperationException("You must agree to Terms & Conditions and the Privacy Policy before creating PIN");
            }

            string hashedPin = _hashingService.HashPin(dto.PinCode);
            customer.PinCode = hashedPin;

            await _customerRepository.SaveChangesAsync();
        }

        #endregion CreatePin

        #region ConfirmPin

        public async Task ConfirmPinAsync(VerificationDTO dto)
        {
            if (string.IsNullOrEmpty(dto.PinCode))
            {
                throw new KeyNotFoundException("PIN can not be empty");
            }

            Customer customer = await GetCustomerByIcNumberAsync(dto.IcNumber);

            if (string.IsNullOrEmpty(customer.PinCode))
            {
                throw new InvalidOperationException("Unmatched PIN. Please enter your PIN again.");
            }

            bool isValidPin = _hashingService.VerifyPin(dto.PinCode, customer.PinCode);

            if (!isValidPin)
            {
                throw new InvalidOperationException("Unmatched PIN. Please enter your PIN again.");
            }
        }

        #endregion ConfirmPin

        #region EnableBiometricLogin

        public async Task EnableBiometricLoginAsync(VerificationDTO dto)
        {
            Customer customer = await GetCustomerByIcNumberAsync(dto.IcNumber);
            customer.IsBiometricEnabled = true;

            await _customerRepository.SaveChangesAsync();
        }

        #endregion EnableBiometricLogin

        #region Login
        public async Task<CustomerRegistrationDTO> LoginCustomerAsync(string icNumber) 
        {
            Customer customer = await GetCustomerByIcNumberAsync(icNumber);

            if (!customer.IsPhoneVerified || !customer.IsEmailVerified || !customer.IsTermsAndConditionsAccepted || string.IsNullOrEmpty(customer.PinCode))
            {
                throw new Exception("One of the following is missing: Phone verification, Email verification, Terms verification, PIN code assignment.");
            }

            return new CustomerRegistrationDTO
            {
                Name = customer.Name,
                MobileNumber = customer.MobileNumber,
                EmailAddress = customer.EmailAddress,
                IcNumber = customer.IcNumber,
                IsPhoneVerified = customer.IsPhoneVerified,
                IsEmailVerified = customer.IsEmailVerified,
                IsTermsAndConditionsAccepted = customer.IsTermsAndConditionsAccepted,
                IsPinCodeAvailable = !string.IsNullOrEmpty(customer.PinCode),
                IsBiometricEnabled = customer.IsBiometricEnabled,
            };
        }

        #endregion Login

        #region GetCustomerAsync

        public async Task<CustomerRegistrationDTO> GetCustomerAsync(string icNumber)
        {
            Customer customer = await GetCustomerByIcNumberAsync(icNumber);

            return new CustomerRegistrationDTO
            {
                Name = customer.Name,
                MobileNumber = customer.MobileNumber,
                EmailAddress = customer.EmailAddress,
                IcNumber = customer.IcNumber,
                IsPhoneVerified = customer.IsPhoneVerified,
                IsEmailVerified = customer.IsEmailVerified,
                IsTermsAndConditionsAccepted = customer.IsTermsAndConditionsAccepted,
                IsPinCodeAvailable = !string.IsNullOrEmpty(customer.PinCode),
                IsBiometricEnabled = customer.IsBiometricEnabled,
            };
        }

       #endregion GetCustomerAsync 
    }
}
