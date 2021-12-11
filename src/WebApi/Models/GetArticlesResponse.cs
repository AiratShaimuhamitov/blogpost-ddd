using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Blogpost.WebApi.Models;

/// <summary>
///
/// </summary>
[DataContract]
public class GetArticlesResponse
{
    /// <summary>
    /// Номер страницы
    /// </summary>
    /// <value>Номер страницы</value>
    [DataMember(Name = "pageIndex")]
    public int? PageIndex { get; set; }

    /// <summary>
    /// Общее количество доступных страниц
    /// </summary>
    /// <value>Общее количество доступных страниц</value>
    [DataMember(Name = "totalPages")]
    public int? TotalPages { get; set; }

    /// <summary>
    /// Общее количество доступных статей
    /// </summary>
    /// <value>Общее количество доступных статей</value>
    [DataMember(Name = "totalCount")]
    public int? TotalCount { get; set; }

    /// <summary>
    /// Gets or Sets Items
    /// </summary>
    [DataMember(Name = "items")]
    public List<Article> Items { get; set; }
}