using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roullete
{
   sealed class Player
    {
       public Chip currentChip { get; set; }
       public List<Bet> currentBet { get; set; }
       public int totalBet { get; set; }
       public int balance { get; set; }
       public List<Bet> savedBetToRebet { get; set; }
       public Player(Image image)
       {
           balance = 1000;
           totalBet = 0;
           currentChip = new Chip(5, new System.Windows.Forms.PictureBox(), image);
           currentBet = new List<Bet>();
           savedBetToRebet = new List<Bet>();
       }
  }
}
