using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slippi_Stats_Database_App
{
    public class Match
    {
        public string matchID { set; get; }
        public string stageName { set; get; }
        public int stageID { set; get; }
        public DateTime matchDate { set; get; }
        public string gameType { set; get; }
        public int numOfFrames { set; get; }
        public string fileName { set; get; }
    }
}
