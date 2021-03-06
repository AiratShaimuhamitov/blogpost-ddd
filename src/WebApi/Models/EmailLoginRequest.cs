/*
 * Authentication service
 *
 * Authentication service
 *
 * OpenAPI spec version: 1.0.0
 *
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Blogpost.WebApi.Models;

/// <summary>
///
/// </summary>
[DataContract]
public partial class EmailLoginRequest : IEquatable<EmailLoginRequest>
{
    /// <summary>
    /// Gets or Sets Email
    /// </summary>
    [DataMember(Name = "email")]
    public string Email { get; set; }

    /// <summary>
    /// Gets or Sets Password
    /// </summary>
    [DataMember(Name = "password")]
    public string Password { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class EmailLoginRequest {\n");
        sb.Append("  Email: ").Append(Email).Append("\n");
        sb.Append("  Password: ").Append(Password).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="obj">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((EmailLoginRequest)obj);
    }

    /// <summary>
    /// Returns true if EmailLoginRequest instances are equal
    /// </summary>
    /// <param name="other">Instance of EmailLoginRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(EmailLoginRequest other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return
            (
                Email == other.Email ||
                Email != null &&
                Email.Equals(other.Email)
            ) &&
            (
                Password == other.Password ||
                Password != null &&
                Password.Equals(other.Password)
            );
    }

    /// <summary>
    /// Gets the hash code
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 41;
            // Suitable nullity checks etc, of course :)
            if (Email != null)
                hashCode = hashCode * 59 + Email.GetHashCode();
            if (Password != null)
                hashCode = hashCode * 59 + Password.GetHashCode();
            return hashCode;
        }
    }

    #region Operators
#pragma warning disable 1591

    public static bool operator ==(EmailLoginRequest left, EmailLoginRequest right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(EmailLoginRequest left, EmailLoginRequest right)
    {
        return !Equals(left, right);
    }

#pragma warning restore 1591
    #endregion Operators
}