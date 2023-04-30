using System;
using System.Collections.Generic;

#nullable disable

namespace MacroSociety.Models
{
    public partial class FriendList
    {
        public int Id { get; set; }
        public string Friendname { get; set; }
        public string Username { get; set; }
        public int IdFriendname { get; set; }
        public int IdUsername { get; set; }

        public virtual User IdFriendnameNavigation { get; set; }
        public virtual User IdUsernameNavigation { get; set; }
    }
}
