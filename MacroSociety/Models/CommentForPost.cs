using System;
using System.Collections.Generic;

#nullable disable

namespace MacroSociety.Models
{
    public partial class CommentForPost
    {
        public int Id { get; set; }
        public string NameUserComment { get; set; }
        public string TextComment { get; set; }
        public int IdFriendPost { get; set; }

        public virtual Post IdFriendPostNavigation { get; set; }
    }
}
