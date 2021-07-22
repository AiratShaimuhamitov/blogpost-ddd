/*
 * Posts service
 *
 * Posts service
 *
 * OpenAPI spec version: 1.0.0
 *
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Blogpost.WebApi.Models
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public partial class GetPostsResponse
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
        /// Общее количество доступных постов
        /// </summary>
        /// <value>Общее количество доступных постов</value>
        [DataMember(Name = "totalCount")]
        public int? TotalCount { get; set; }

        /// <summary>
        /// Gets or Sets Items
        /// </summary>
        [DataMember(Name = "items")]
        public List<Post> Items { get; set; }
    }
}