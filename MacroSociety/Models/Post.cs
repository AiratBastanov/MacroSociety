using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable

namespace MacroSociety.Models
{
    public partial class Post
    {
        public Post()
        {
            CommentForPosts = new HashSet<CommentForPost>();
            Likes = new HashSet<Like>();
        }

        public int Id { get; set; }
        public string NamePost { get; set; }
        public string PhotoUrl { get; set; }
        public string NameUser { get; set; }
        public int IdUser { get; set; }
        public virtual User IdUserNavigation { get; set; }      
        public virtual ICollection<CommentForPost> CommentForPosts { get; set; }     
        public virtual ICollection<Like> Likes { get; set; }
    }
}
