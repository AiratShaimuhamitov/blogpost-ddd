using System;
using System.Runtime.Serialization;

namespace Blogpost.WebApi.Models
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public partial class Article
    {
        /// <summary>
        /// Название статьи
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Содержание статьи (на данный момент ссылка на telegraph)
        /// </summary>
        [DataMember(Name = "content")]
        public string Content { get; set; }
    }
}
