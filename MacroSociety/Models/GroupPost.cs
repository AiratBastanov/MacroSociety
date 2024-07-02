using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication1.Models
{
    public partial class GroupPost
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string PostContent { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostDate { get; set; }

        public virtual Group Group { get; set; }
        public virtual User PostedByNavigation { get; set; }
    }
}
