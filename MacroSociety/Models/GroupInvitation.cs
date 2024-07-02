using System;
using System.Collections.Generic;

#nullable disable

namespace MacroSociety.Models
{
    public partial class GroupInvitation
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int InvitedUserId { get; set; }
        public int InvitedBy { get; set; }
        public DateTime InvitationDate { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime? ResponseDate { get; set; }

        public virtual Group Group { get; set; }
        public virtual User InvitedByNavigation { get; set; }
        public virtual User InvitedUser { get; set; }
    }
}
