using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Model
{
    public class Statistik
    {

        public string Name { get; private set; }
        public int Elo { get; private set; }
        public int Wins { get; private set; }
        public int Losses { get; private set; }
        public string UserId { get; private set; }
        public string Id { get; private set; }
        public double WinLoseRatio { get; private set; }
        public int Draw { get; private set; }

        public Statistik(string name, int elo, int wins, int losses, string userId, string id, double winLoseRatio, int draw)
        {
            Name = name;
            Elo = elo;
            Wins = wins;
            Losses = losses;
            UserId = userId;
            Id = id;
            WinLoseRatio = winLoseRatio;
            Draw = draw;
        }

        public Statistik()
        {
        }

        public void UpdateWins(int newWin)
        {
            Wins = newWin;

            if (Losses == 0)
                WinLoseRatio = 0;
            else
                WinLoseRatio = Wins * 1.0 / Losses;

            Elo += 3;
        }

        public void UpdateLosses(int newLoss)
        {
            Losses = newLoss;

            if (Losses == 0)
                WinLoseRatio = 0;
            else
                WinLoseRatio = Wins * 1.0 / Losses;

            Elo -= 5;
        }

        public void UpdateDrawByOne()
        {
            Draw += 1;
        }
    }


}
