/*
 * Profiles service
 *
 * Profiles service
 *
 * OpenAPI spec version: 1.0.0
 *
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Runtime.Serialization;

namespace Blogpost.WebApi.Models
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class SubscribeRequest
    {
        /// <summary>
        /// Идентификатор профиля
        /// </summary>
        /// <value>Идентификатор профиля</value>
        [DataMember(Name = "ToProfileId")]
        public Guid ToProfileId { get; set; }
    }
}