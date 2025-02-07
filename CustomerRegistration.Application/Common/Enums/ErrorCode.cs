namespace CustomerRegistration.Application.Common.Enums
{
    public enum ErrorCode
    {
        InternalError = 1,

        Unauthorized = 10,
        Forbidden = 11,

        BadRequest = 20,
        NotFound = 21,

        Common = 30,
        Unknown = 31,

        Required = 35,
        Duplicate = 36,

        InvalidOperation = 40,
    }
}
