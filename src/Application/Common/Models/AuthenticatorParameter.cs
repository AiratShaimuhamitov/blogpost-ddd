namespace Blogpost.Application.Common.Models;

public abstract class AuthenticatorParameter
{
    public string IpAddress { get; set; }
}

public class EmailAuthenticatorParameter : AuthenticatorParameter
{
    public string Email { get; set; }

    public string Password { get; set; }
}

public class GoogleAuthenticatorParameter : AuthenticatorParameter
{
    public string Token { get; set; }
}

public class FacebookAuthenticatorParameter : AuthenticatorParameter
{
    public string Token { get; set; }
}