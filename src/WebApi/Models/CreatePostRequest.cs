using System.Runtime.Serialization;

namespace Blogpost.WebApi.Models
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class CreatePostRequest
    {
        /// <summary>
        /// Могут ли другие пользователи видеть этот пост
        /// </summary>
        /// <value>Содержание поста</value>
        [DataMember(Name = "content")]
        public string Content { get; set; }

        /// <summary>
        /// Могут ли другие пользователи видеть этот пост
        /// </summary>
        /// <value>Могут ли другие пользователи видеть этот пост</value>
        [DataMember(Name = "isVisible")]
        public bool? IsVisible { get; set; }
    }
}
