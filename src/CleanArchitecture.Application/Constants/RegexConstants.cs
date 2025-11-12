namespace CleanArchitecture.Application.Constants;

public class RegexConstants
{
    public const string PHONE_NUMBER_REGEX_PATTERN = @"^(?:\+84|0)(3|5|7|8|9)[0-9]{8}$";
    public const string USERNAME_REGEX_PATTERN = "^(?![_.@])(?!.*[_.@]{2})[a-zA-Z0-9_.@]{3,}(?<![_.@])$"; 

}