using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheTicTacToeGame.Game_Objects
{
    public class Node
    {
        public bool gameWon;
        public Node[] Children;
        public bool playerChance;
        public char[,] gameStatus;
        public int weight;
        public static Random rand = new Random();

    }
}
