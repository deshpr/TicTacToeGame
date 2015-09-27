using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TheTicTacToeGame.Game_Objects
{
    public struct Move
    {
        public int Cost;
        public int Index;
    }
    public class MyMoveComparer : IComparer<Move>
    {
        public bool Maximize { get; set; }
        public MyMoveComparer(bool Maximize)
        {
            this.Maximize = Maximize;
        }
        public int Compare(Move a, Move b)
        {
            if (a.Cost > b.Cost)
            {
                if (Maximize)
                    return -1;
                else
                    return 1;
            }
            else if (a.Cost < b.Cost)
            {
                if (Maximize)
                    return 1;
                else
                    return -1;
            }
            return 0;
        }
    }
    public class HumanMachine
    {
        public static int level = 1;
        public Node root;
        public static int gamePlayLevel = 1;
        static int Count;
        public bool foundElement = false;
        public int nodeCount = 0;
        public bool playerChance;
        public Node currentPosition = null;
        public char Character { get; set; }
        public Node GamePossibilities;

        public HumanMachine(char Character)
        {
            this.Character = Character;
                 char[,] gameStatus = new char[3, 3];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        gameStatus[i, j] = '\0';
                    }
                }
                root = new Node();
                root.gameStatus = gameStatus;
                int k = 0;
                root.Children = new Node[9];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        root.Children[k++] = createTree(MakeCopy(gameStatus), true, new Index() { x = i, y = j });
                    }
                }
                //          tree.root = tree.createNode2(gameStatus, true, new Index() { x = 0, y = 0 });
                Debug.WriteLine("done");
                GamePossibilities = root;
                currentPosition = root;
        }
        public Node createTree(char[,] gameStatus, bool markPlayer, Index idx)
        {
            // markPlayer at index idx
            Node n = new Node();
            n.playerChance = markPlayer;
            bool done;
            MarkTicTacToe(gameStatus, idx, markPlayer, out done);
            bool result = CalculateWin(gameStatus);
            if (result)
            {
                if (!markPlayer)
                {
                    //              Console.WriteLine("brake point");
                }
                // won the game. Check who won
                n.gameStatus = gameStatus;
                n.Children = null;
                return n;
            }
            List<Index> indicesToMark = GetIndicesToMark(gameStatus);
            //       Display(gameStatus);
            if (indicesToMark.Count > 0)
            {
                //   Display(gameStatus);  
                n.gameStatus = gameStatus;
                n.Children = new Node[indicesToMark.Count];
                for (int i = 0; i < indicesToMark.Count; i++)
                {
                    n.Children[i] = createTree(MakeCopy(gameStatus), !markPlayer, indicesToMark[i]);
                }
                return n;
            }
            else
            {
                n.Children = null;
                n.gameStatus = gameStatus;
                return n;
            }
        }


        public char[,] MarkTicTacToe(bool player, char[,] gameStatus, out bool successful)
        {
            successful = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameStatus[i, j] == '\0')
                    {
                        if (player)
                            gameStatus[i, j] = 'O';
                        else
                            gameStatus[i, j] = 'X';
                        successful = true;
                        return gameStatus;
                    }
                }
            }
            return gameStatus;
        }


        public int numberOfChildren(char[,] gameStatus)
        {
            int cnt = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameStatus[i, j] == '\0')
                    {
                        ++cnt;
                    }
                }
            }
            return cnt;
        }

        public static char[,] MakeCopy(char[,] gameStatus)
        {
            char[,] updatedGameStatus = new char[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    updatedGameStatus[i, j] = gameStatus[i, j];
                }
            }
            return updatedGameStatus;
        }

        public bool CalculateWin(char[,] gameStatus)
        {
            if (HasWon(gameStatus) != null)
                return true;
            else
                return false;

        }
        public static int GetBestMove(Node p)
        {
            List<Move> moves = new List<Move>();
            MyMoveComparer comparer = new MyMoveComparer(false);
            if(p.Children == null)
            {
                // Game Over!
                return -1;
            }
            for (int i = 0; i < p.Children.Length; i++)
            {
                if (gamePlayLevel == 1)
                {
                    moves.Add(new Move() { Cost = GetScore(p.Children[i].gameStatus, false), Index = i });
                }
                else
                {
                    moves.Add(getLeastCostAndMove(p.Children[i], comparer, i));
                }
            }
            comparer.Maximize = true;
            moves.Sort(comparer);
            return moves[0].Index;
        }


        public static Move getLeastCostAndMove(Node node, MyMoveComparer comparer, int parentIndex)
        {
            List<Move> moves = new List<Move>();
            if (node.Children != null)
            {
                for (int i = 0; i < node.Children.Length; i++)
                {

                    if (node.Children[i] != null)
                        moves.Add(new Move() { Cost = GetScore(node.Children[i].gameStatus, true), Index = parentIndex });
                    else
                        return new Move() { Cost = 1000, Index = i };
                }
            }
            else
            {
                return new Move() { Cost = 1000, Index = parentIndex };
            }
            // Get the best cost
            comparer.Maximize = false;
            moves.Sort(comparer);
            return moves[0];
        }

        public void PreOrder(Node p)
        {
            if (p != null)
            {
                Console.WriteLine(p);
                foreach (Node n in p.Children)
                {
                    PreOrder(n);
                }
            }
        }

        public struct Index
        {
            public int x;
            public int y;
            public override string ToString()
            {
                return String.Format("x = {0}, y = {1}", x, y);
            }
        }

        public static int CalculateRawScore(List<char> gameStatusRow, char playerSymbol)
        {

            int res = 0;
            int emptyCount = 0;
            char empty = '\0';
            for (int i = 0; i < gameStatusRow.Count; i++)
            {
                if (gameStatusRow[i] == playerSymbol)
                {
                    ++res;
                }
                else if (gameStatusRow[i] == empty)
                {
                    ++emptyCount;
                }
            }
            // VIMP: 
            if (res == 3)
                return 1000;
            if (res == 2 && emptyCount == 1)
                return 100;
            if (res == 2 && emptyCount != 1)
                return 1;   // unfavorable
            if (res == 1 && emptyCount == 2)
                return 10;
            if (res == 1 && emptyCount == 0)
                return 9;
            if (emptyCount == 3)
                return 11;
            return 1;

        }
        public List<Index> GetIndicesToMark(char[,] gameStatus)
        {
            List<Index> indices = new List<Index>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameStatus[i, j] == '\0')
                    {
                        // include remaining indices into matrix
                        indices.Add(new Index() { x = i, y = j });
                    }
                }
            }
            return indices;
        }

        public void MarkTicTacToe(char[,] gameStatus, Index idx, bool playerChance, out bool successful)
        {
            successful = false;

            if (playerChance)
            {
                gameStatus[idx.x, idx.y] = 'O';
                successful = true;
            }
            else
            {
                gameStatus[idx.x, idx.y] = 'X';
                successful = true;
            }

        }

    // The Heuristic Function.
        public static int GetScore(char[,] gameStatus, bool player)
        {
            char ch;
            if (!player)
                ch = 'X';
            else
                ch = 'O';
            //Rules : 
            // 1. If there areX's and O's in a row, score is 0 for that row
            // 2. If there is nothing in a row, score for that row depends upon:
            //a. More player symbols in a row, score is 1 * number of symbols
            // 3. If there's 2   player symbols in a row, with a gap, score is 100
            // 4. One player symbol with 2 empty spaces, score is 10
            // 5. Three player symbols in a row = 1000
            // 6. Apply the same rules for the columns

            List<char> list = new List<char>();
            List<char> list2 = new List<char>();
            List<char> diagonal = new List<char>();
            List<char> diagonal2 = new List<char>();
            int res = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    list.Add(gameStatus[i, j]);
                    if (i == 0)
                    {

                    }
                }
                //             for (int j = 0; j < 3; j++ )
                //             {
                //                 list2.Add(gameStatus[j, i]);
                //             }
                //            res += CalculateRawScore(list, ch) + CalculateRawScore(list2, ch);

                for (int k = 0; k < 3; k++)
                {
                    list2.Add(gameStatus[k, i]);
                }
                diagonal.Add(gameStatus[0, 0]);
                diagonal.Add(gameStatus[1, 1]);
                diagonal.Add(gameStatus[2, 2]);
                diagonal2.Add(gameStatus[0, 2]);
                diagonal2.Add(gameStatus[1, 1]);
                diagonal2.Add(gameStatus[2, 0]);
                res += CalculateRawScore(list, ch) + CalculateRawScore(list2, ch) + CalculateRawScore(diagonal, ch) + CalculateRawScore(diagonal2, ch);

                list = new List<char>();
                list2 = new List<char>();
                diagonal2 = new List<char>();
                diagonal = new List<char>();
            }


            if (ch == 'X')
                return res;
            else
            {
                return -res;
            }
        }
        
        public int GetStrategicMove(int PlayerMove)
        {
            return getNextMove(PlayerMove);
        }

        public int getNextMove(int PathWay)
        {
  //        Node p = null;
            currentPosition = currentPosition.Children[PathWay];
            
        //    Display(currentPosition.gameStatus);
            if (HasWon(currentPosition.gameStatus) != null)
            {
                Console.WriteLine("you won the game!!!");
                currentPosition = null;
  //              return gameStatus;
            }
            //             int nextMove = getBestMovesRecursively(1,node,out p);
            if(currentPosition == null)
            {
// With this move, the player has won the game, so end the game.
                return -1;
            }
            int nextMove = GetBestMove(currentPosition);
            if (nextMove != -1)
                currentPosition = currentPosition.Children[nextMove];

            return nextMove;
         //   return node.gameStatus;
        }
        
        public static void Display(char[,] gameStatus)
        {
            ++Count;
            Console.WriteLine("Matrix is:");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameStatus[i, j] == '\0')
                        Console.Write("NA");
                    Console.Write("  {0}  | ", gameStatus[i, j]);
                }
                Console.WriteLine("");
                Console.WriteLine("---------------------------");
            }
        }
        public static int? HasWon(char[,] gameStatus)
        {
            int? horWon = HorizontalWon(gameStatus);
            int? verWon = VerticalWon(gameStatus);
            int? diagWon = DiagonalWon(gameStatus);
            if (horWon != null && verWon == null && diagWon == null)
            {
                //           Debug.WriteLine("row won the game!!!, horWon = {0}", horWon);
                return horWon;
            }
            else if (horWon == null && verWon != null && diagWon == null)
            {
                return verWon;
            }
            else if (horWon == null && verWon == null && diagWon != null)
            {
                //         Debug.WriteLine("diagonal won the game !!!!");
                //         Debug.WriteLine("diagonal won is {0}", diagWon);
                return diagWon;
            }
            else
                return null;
        }

        public static int? DiagonalWon(char[,] gameStatus)
        {
            int countX = 0;
            int countO = 0;
            int countCrossY = 0;
            int countCrossX = 0;
            // Check diagonally
            for (int i = 0; i < 3; i++)
            {
                if (gameStatus[i, i] == 'X')
                {
                    ++countX;
                }
                else if (gameStatus[i, i] == 'O')
                {
                    ++countO;
                }
                if (countX == 3 || countO == 3)
                {
                    return -10;
                }
            }
            //          Debug.WriteLine("displaying where the crosses are!!!");
            //          Debug.Write("[0,2] = {" + gameStatus[0, 2] + "}, [1,1] = {" + gameStatus[1, 1] + "}, [2,1] ={" + gameStatus[2, 1] + "}");
            if (gameStatus[0, 2] == 'X' && gameStatus[1, 1] == 'X' && gameStatus[2, 0] == 'X')
            {
                //             Debug.WriteLine("worked: condition true, all X's");
                return -15; // diagonal in opposite direction
            }
            else if (gameStatus[0, 2] == 'O' && gameStatus[1, 1] == 'O' && gameStatus[2, 0] == 'O')
            {
                //             Debug.WriteLine("worked: condition true, all X's");
                return -15; // diagonal in opposite direction
            }

            //  Console.WriteLine("no need to make a diagonal cut");
            return null;
        }


        public static int? VerticalWon(char[,] gameStatus)
        {
            //      int countVerti = 0;
            int countVertX = 0;
            int countVertO = 0;
            int j = 0;
            for (int i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    if (gameStatus[j, i] == 'X')
                    {
                        ++countVertX;
                    }
                    else if (gameStatus[j, i] == 'O')
                    {
                        ++countVertO;
                    }
                }
                if (countVertX == 3)
                {
                    //        Debug.WriteLine("there is a winner at column {0}", i);
                    // One row has all consecutive X's or O's
                    return (i + 1);
                }
                else if (countVertO == 3)
                {
                    return -(i + 1);
                }
                else
                {
                    countVertO = 0;
                    countVertX = 0;
                    continue;
                }
            }
            return null;
        }

        public static int? HorizontalWon(char[,] gameStatus)
        {
            int countHoriX = 0;
            int countHoriO = 0;
            int j = 0;
            for (int i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    if (gameStatus[i, j] == 'X')
                    {
                        ++countHoriX;
                    }
                    else if (gameStatus[i, j] == 'O')
                    {
                        ++countHoriO;
                    }
                }
                if (countHoriX == 3)
                {
                    //      Debug.WriteLine("there is a winner at row {0}", i);
                    // One row has all consecutive X's or O's
                    return i + 1;
                }
                else if (countHoriO == 3)
                {
                    return -i - 1;
                }
                else
                {
                    countHoriO = 0;
                    countHoriX = 0;
                    continue;
                }
            }
            return null;
        }

    }
}
