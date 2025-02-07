namespace CustomerRegistration.Application.Common.Utilities
{
    public static class Utility
    {
        private static readonly Random _random = new();

        public static string GenerateRandomCode(int length = 4)
        {
            string code = _random.Next(0, (int)Math.Pow(10, length)).ToString("D" + length);

            return code;
        }
    }
}
