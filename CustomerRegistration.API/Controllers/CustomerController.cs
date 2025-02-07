using Microsoft.AspNetCore.Mvc;

using CustomerRegistration.Application.DTOs;
using CustomerRegistration.Application.Interfaces.Service;

namespace CustomerRegistration.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #region Register

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer(CustomerRegistrationDTO customerDTO)
        {
            CustomerRegistrationDTO customer = await _customerService.RegisterCustomerAsync(customerDTO);

            return Ok(customer);
        }

        #endregion Register

        #region VerifyMobile

        [HttpPost("verify-mobile")]
        public async Task<IActionResult> VerifyMobile([FromBody] VerificationDTO dto)
        {
            await _customerService.VerifyMobileCodeAsync(dto);

            return Ok(new { Message = "Enter the 4-digit code sent to your email." });
        }

        #endregion VerifyMobile

        #region VerifyEmail

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerificationDTO dto)
        {
            await _customerService.VerifyEmailCodeAsync(dto);

            return Ok();
        }

        #endregion VerifyEmail

        #region AcceptTerms

        [HttpPost("accept-terms")]
        public async Task<IActionResult> AcceptTermsAndConditions([FromBody] VerificationDTO dto)
        {
            await _customerService.AcceptTermsAndConditions(dto);

            return Ok();
        }

        #endregion AcceptTerms

        #region CreatePin

        [HttpPost("create-pin")]
        public async Task<IActionResult> CreatePin([FromBody] VerificationDTO dto)
        {
            await _customerService.CreatePinAsync(dto);

            return Ok();
        }

        #endregion CreatePin

        #region ConfirmPin

        [HttpPost("confirm-pin")]
        public async Task<IActionResult> ConfirmPin([FromBody] VerificationDTO dto)
        {
            await _customerService.ConfirmPinAsync(dto);

            return Ok();
        }

        #endregion ConfirmPin

        #region EnableBiometricLogin

        [HttpPost("enable-biometric-login")]
        public async Task<IActionResult> EnableBiometricLogin([FromBody] VerificationDTO dto)
        {
            await _customerService.EnableBiometricLoginAsync(dto);

            return Ok();
        }

        #endregion EnableBiometricLogin

        #region Login

        [HttpPost("login")]
        public async Task<IActionResult> LoginCustomer(string icNumber)
        {
            CustomerRegistrationDTO customer = await _customerService.LoginCustomerAsync(icNumber);

            return Ok(customer);
        }

        #endregion Login

        #region GetCustomer

        [HttpGet("{icNumber}")]
        public async Task<IActionResult> GetCustomer(string icNumber)
        {
            CustomerRegistrationDTO customer = await _customerService.GetCustomerAsync(icNumber);

            return Ok(customer);
        }

        #endregion GetCustomer
    }
}

