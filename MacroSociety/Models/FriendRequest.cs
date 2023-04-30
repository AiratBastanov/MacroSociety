using System;
using System.Collections.Generic;

#nullable disable

namespace MacroSociety.Models
{
    public partial class FriendRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FutureFriend { get; set; }
        public int IdUser { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
