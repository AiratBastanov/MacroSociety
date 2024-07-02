using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication1.Models
{
    public partial class Group
    {
        public Group()
        {
            GroupInvitations = new HashSet<GroupInvitation>();
            GroupMembers = new HashSet<GroupMember>();
            GroupPosts = new HashSet<GroupPost>();
        }

        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual ICollection<GroupInvitation> GroupInvitations { get; set; }
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
        public virtual ICollection<GroupPost> GroupPosts { get; set; }
    }
}
