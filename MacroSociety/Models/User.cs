using System;
using System.Collections.Generic;

#nullable disable

namespace MacroSociety.Models
{
    public partial class User
    {
        public User()
        {
            FriendListIdFriendnameNavigations = new HashSet<FriendList>();
            FriendListIdUsernameNavigations = new HashSet<FriendList>();
            FriendRequests = new HashSet<FriendRequest>();
            Posts = new HashSet<Post>();
            UserSongs = new HashSet<UserSong>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string Money { get; set; }
        public string SubscriptionGame { get; set; }
        public string SubscriptionMusic { get; set; }
        public string IsOnline { get; set; }

        public virtual Game Game { get; set; }
        public virtual ICollection<FriendList> FriendListIdFriendnameNavigations { get; set; }
        public virtual ICollection<FriendList> FriendListIdUsernameNavigations { get; set; }
        public virtual ICollection<FriendRequest> FriendRequests { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UserSong> UserSongs { get; set; }
    }
}
