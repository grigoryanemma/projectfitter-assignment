namespace CustomerRegistration.Application.Security
{
    public interface IHashingService
    {
        string HashPin(string pin);
        bool VerifyPin(string pin, string hashedPin);
    }
}
