using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication1.Models
{
    public partial class GroupMember
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime JoinDate { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
