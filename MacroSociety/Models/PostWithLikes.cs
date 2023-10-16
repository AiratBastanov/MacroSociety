using Newtonsoft.Json;
using System.Collections.Generic;

namespace MacroSociety.Models
{
    public class PostWithLikes
    {
        /*   public int Id { get; set; }
           public string NamePost { get; set; }
           public string PhotoUrl { get; set; }
           public string NameUser { get; set; }
           public int IdUser { get; set; }
           public List<string> Likes { get; set; }
           public int TotalLikes { get; set; }*/
        //второй варинт
        /*   public PostWithLikes(Post post, int likesCount, List<Like> likes)
           {
               Post = post;
               TotalLikes = likesCount;
               Likes = likes;
           }

           Post Post;
           //public virtual ICollection<CommentForPost> CommentForPosts { get; set; }
           public int TotalLikes { get; set; }
           public List<Like> Likes { get; set; }
       }
       public class PostsAndLikes
       {
           public List<PostWithLikes> Posts { get; set; }
       }*/
        //третий варинат
        public int Id { get; set; }
        public string NamePost { get; set; }
        public string PhotoUrl { get; set; }
        public string NameUser { get; set; }
        public int LikeCount { get; set; }
        public List<LikeDTO> Likes { get; set; }
    }
    public class LikeDTO
    {
        public int Id { get; set; }
        public string NameUserLike { get; set; }
        public string WhosePost { get; set; }
        public int IdFriendPost { get; set; }
    }
}
