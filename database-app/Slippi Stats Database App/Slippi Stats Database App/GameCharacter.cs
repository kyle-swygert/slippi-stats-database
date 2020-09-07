using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slippi_Stats_Database_App
{
    public class GameCharacter
    {
        public string charName { set; get; }
        public string charID { set; get; }
        public int colorInt { set; get; }
        public bool didWin { set; get; }
        public string teamColor { set; get; }
        public string tag { set; get; }
        public int portNumber { set; get; }
        public int cssID { set; get; }
    }
}
