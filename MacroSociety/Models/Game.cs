using System;
using System.Collections.Generic;

#nullable disable

namespace MacroSociety.Models
{
    public partial class Game
    {
        public int Id { get; set; }
        public string NameUser { get; set; }
        public int LevelCompleted { get; set; }
        public string UserReceivedGamePrize { get; set; }
        public int IdUser { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
