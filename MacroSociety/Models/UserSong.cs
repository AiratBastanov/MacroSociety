using System;
using System.Collections.Generic;

#nullable disable

namespace MacroSociety.Models
{
    public partial class UserSong
    {
        public int UserId { get; set; }
        public int SongId { get; set; }

        public virtual Song Song { get; set; }
        public virtual User User { get; set; }
    }
}
