using Newtonsoft.Json;
using System.Collections.Generic;

namespace MacroSociety.Models
{
    public class PostWithLikes
    {
        /* public IEnumerable<Post> Posts { get; set; }
         public IEnumerable<Like> Likes { get; set; }*/
        /*  public Post Post { get; set; }
          [JsonIgnore] // Исключите это свойство из сериализаци*/
        /*   public int PostId { get; set; }
           public string NamePost { get; set; }
           public string PhotoUrl { get; set; }
           public string NameUser { get; set; }
           public int UserId { get; set; }*/
        public int Id { get; set; }
        public string NamePost { get; set; }
        public string PhotoUrl { get; set; }
        public string NameUser { get; set; }
        public int IdUser { get; set; }
        public List<string> Likes { get; set; }
        public int TotalLikes { get; set; }
    }
}
