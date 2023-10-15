using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable

namespace MacroSociety.Models
{
    public partial class Like
    {
        public int Id { get; set; }
        public string NameUserLike { get; set; }
        public string WhosePost { get; set; }
        public int IdFriendPost { get; set; }      
        public virtual Post IdFriendPostNavigation { get; set; }
    }
}
