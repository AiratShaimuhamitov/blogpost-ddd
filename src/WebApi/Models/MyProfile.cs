using System;
using System.Runtime.Serialization;

namespace Blogpost.WebApi.Models
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public partial class MyProfile
    {
        /// <summary>
        /// Идентификатор профиля
        /// </summary>
        /// <value>Идентификатор профиля</value>
        [DataMember(Name = "id")]
        public Guid? Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        /// <value>Имя пользователя</value>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Email
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
