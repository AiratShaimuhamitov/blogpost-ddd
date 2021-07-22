using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Blogpost.WebApi.Models
{
    [DataContract]
    public partial class Post
    {
        /// <summary>
        /// Идентификатор поста
        /// </summary>
        /// <value>Идентификатор поста</value>
        [DataMember(Name = "id")]
        public Guid? Id { get; set; }

        /// <summary>
        /// Содержание поста
        /// </summary>
        /// <value>Содержание поста</value>
        [DataMember(Name = "content")]
        public string Content { get; set; }

        /// <summary>
        /// Могут ли другие пользователи видеть этот пост
        /// </summary>
        /// <value>Могут ли другие пользователи видеть этот пост</value>
        [DataMember(Name = "isVisible")]
        public bool IsVisible { get; set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        /// <value>Количество лайков</value>
        [DataMember(Name = "likes")]
        public int? Likes { get; set; }

        /// <summary>
        /// Количество комментариев
        /// </summary>
        /// <value>Количество комментариев</value>
        [DataMember(Name = "comments")]
        public int? Comments { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name = "createdBy")]
        public CreatedBy CreatedBy { get; set; }

        /// <summary>
        /// Дата создания поста
        /// </summary>
        /// <value>Дата создания поста</value>
        [DataMember(Name = "createdAt")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Текущий пользователь добавил лайк посту
        /// </summary>
        /// <value>Текущий пользователь добавил лайк посту</value>
        [DataMember(Name = "hasLikeFromCurrentUser")]
        public bool HasLikeFromCurrentUser { get; set; }
    }
}
