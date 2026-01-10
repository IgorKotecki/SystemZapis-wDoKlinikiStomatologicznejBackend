namespace SystemZapisowDoKlinikiApi.Exceptions;

public class BusinessException(string errorCode, string message) : Exception(message)
{
    public string ErrorCode = errorCode;
}