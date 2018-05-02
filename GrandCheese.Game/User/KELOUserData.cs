using GrandCheese.Util;
using GrandCheese.Util.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game.User
{
    public class KELOUserData : ISerializable
    {
        const int MT_PLACEMENTTEST = 0; // Placement test (배치고사)
        const int MT_NORMAL = 1; // General (일반)
        const int MT_MAX = 2; // Undocumented
        
        public int InitELOWin { get; set; } = 0;
        public int ELOWin { get; set; } = 0;
        public int InitELOLose { get; set; } = 0;
        public int ELOLose { get; set; } = 0;
        public int RatingPoint { get; set; } = 0;
        public int InitRatingPoint { get; set; } = 0;
        public int ELOType { get; set; } = MT_NORMAL;
        public int InitMatchTotalCount { get; set; } = 0;
        public int MatchTotalCount { get; set; } = 0;
        public int LastWinLose { get; set; } = 0; // 0 = Lose, 1 = Win
        public int ConstantK { get; set; } = 0; // Constant K for placement tests
        public byte Grade { get; set; } = 0; // Grade
        public int PlacementTestPlayCount { get; set; } = 0; // Placement test number.

        public void Serialize(Packet packet, int i, params object[] optional)
        {
            packet.Put(
                InitELOWin,
                ELOWin,
                InitELOLose,
                ELOLose,
                1660, // m_nRatingPoint
                1660, // m_nInitRatingPoint
                ELOType,
                InitMatchTotalCount,
                MatchTotalCount,
                LastWinLose,
                ConstantK,
                Grade,
                7
            );
        }
    }
}
