using System;
using System.Runtime.Serialization;

namespace Blogpost.WebApi.Models
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class TokenResponse
    {
        /// <summary>
        /// Gets or Sets Value
        /// </summary>
        [DataMember(Name = "accessToken")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or Sets ExpiresAt
        /// </summary>
        [DataMember(Name = "expiresAt")]
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>
        [DataMember(Name = "refreshToken")]
        public string RefreshToken { get; set; }
    }
}
