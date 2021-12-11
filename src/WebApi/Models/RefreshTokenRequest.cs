using System.Runtime.Serialization;

namespace Blogpost.WebApi.Models;

[DataContract]
public class RefreshTokenRequest
{
    [DataMember(Name = "refreshToken")]
    public string RefreshToken { get; set; }
}