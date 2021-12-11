using System;
using System.Runtime.Serialization;

namespace Blogpost.WebApi.Models;

[DataContract]
public partial class CreatedBy
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    /// <value>Идентификатор пользователя</value>
    [DataMember(Name = "id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    /// <value>Имя пользователя</value>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Ссылка на фотографию пользователя
    /// </summary>
    /// <value>Ссылка на фотографию пользователя</value>
    [DataMember(Name = "photoUrl")]
    public string PhotoUrl { get; set; }
}