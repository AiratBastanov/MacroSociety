using System.Collections.Generic;

namespace MacroSociety.Models
{
    public class PostWithLikes
    {
        public List<Post> Posts { get; set; }
        public List<Like> Likes { get; set; }
    }

}
