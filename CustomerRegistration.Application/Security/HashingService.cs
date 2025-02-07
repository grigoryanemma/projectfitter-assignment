namespace CustomerRegistration.Application.Security
{
    public class HashingService : IHashingService
    {
        private const int WorkFactor = 12;

        public string HashPin(string pin)
        {
            return BCrypt.Net.BCrypt.HashPassword(pin, workFactor: WorkFactor);
        }

        public bool VerifyPin(string pin, string hashedPin)
        {
            return BCrypt.Net.BCrypt.Verify(pin, hashedPin);
        }
    }
}
