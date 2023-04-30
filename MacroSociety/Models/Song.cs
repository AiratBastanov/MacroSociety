using System;
using System.Collections.Generic;

#nullable disable

namespace MacroSociety.Models
{
    public partial class Song
    {
        public Song()
        {
            UserSongs = new HashSet<UserSong>();
        }

        public int Id { get; set; }
        public string SongTitle { get; set; }
        public string SongArtist { get; set; }
        public string SongAlbum { get; set; }
        public string SongGenre { get; set; }
        public int? Duration { get; set; }

        public virtual ICollection<UserSong> UserSongs { get; set; }
    }
}
