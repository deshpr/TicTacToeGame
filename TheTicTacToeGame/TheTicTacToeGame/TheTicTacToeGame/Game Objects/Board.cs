
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using TheTicTacToeGame.Screens;

namespace TheTicTacToeGame.Game_Objects
{
    public class Board
    {
        public static int BoxDimension = 120;
        Box[] boxes;
        Color color;
        static float call_number = 0;
        public double nextBlinkTime = 0.00f;
// Getter property. Do not allow
// modifying boxes of a board by external code.
        public Box[] Boxes
        {
            get
            {
                return boxes;
            }
        }
        public Board(Texture2D pixelForEachBox, float WindowWidth, float WindowHeight)
        {
            boxes = new Box[9];
            boxes[0] = new Box(new Vector2(128, 0), new Vector2(WindowWidth / 2 - BoxDimension / 2 - BoxDimension, WindowHeight / 2 - BoxDimension / 2 - BoxDimension), new Vector2(4f, 4f), new Vector2(BoxDimension, BoxDimension), pixelForEachBox, Color.Green);
            // reach coordinate of  452, 204
            boxes[1] = new Box(new Vector2(WindowWidth / 2 - BoxDimension / 2, 0), new Vector2(WindowWidth / 2 - BoxDimension / 2, WindowHeight / 2 - (3 * BoxDimension / 2)), new Vector2(0f, 4f), new Vector2(BoxDimension, BoxDimension),pixelForEachBox, Color.Green);
            // reach coordinate of 1024-332, 204
            boxes[2] = new Box(new Vector2(WindowWidth - BoxDimension - 128, 0), new Vector2(WindowWidth / 2 + BoxDimension / 2, WindowHeight / 2 - BoxDimension / 2 - BoxDimension), new Vector2(-4f, 4f), new Vector2(BoxDimension, BoxDimension), pixelForEachBox, Color.LightBlue);
            // reach coordinate of 332,204 + BoxDimension
            boxes[3] = new Box(new Vector2(128, WindowHeight / 2 - BoxDimension / 2), new Vector2(WindowWidth / 2 - BoxDimension / 2 - BoxDimension, WindowHeight / 2 - BoxDimension / 2), new Vector2(4f, 0f), new Vector2(BoxDimension, BoxDimension), pixelForEachBox, Color.Green);
            // Box to draw at the center
            boxes[4] = new Box(new Vector2(WindowWidth / 2, WindowHeight / 2), new Vector2(WindowWidth / 2 - BoxDimension / 2, WindowHeight / 2 - BoxDimension / 2), new Vector2(1f, 1f), new Vector2(0f, 0f), pixelForEachBox, Color.Green, true);
            // Box at the middle right
            boxes[5] = new Box(new Vector2(WindowWidth - BoxDimension - 128, WindowHeight / 2 - BoxDimension / 2), new Vector2(WindowWidth / 2 + BoxDimension / 2, WindowHeight / 2 - BoxDimension / 2), new Vector2(-4f, 0f), new Vector2(BoxDimension, BoxDimension), pixelForEachBox, Color.Green);
            // Box at bottom left
            boxes[6] = new Box(new Vector2(8, WindowHeight), new Vector2(WindowWidth / 2 - BoxDimension / 2 - BoxDimension, WindowHeight / 2 + BoxDimension / 2), new Vector2(6f, -6f), new Vector2(BoxDimension, BoxDimension), pixelForEachBox, Color.Green);
            // Box at the bottom middle
            boxes[7] = new Box(new Vector2(WindowWidth / 2 - BoxDimension / 2, WindowHeight), new Vector2(WindowWidth / 2 - BoxDimension / 2, WindowHeight / 2 + BoxDimension / 2), new Vector2(0f, -6f), new Vector2(BoxDimension, BoxDimension), pixelForEachBox, Color.Green);
            // Box at the bottom right
            boxes[8] = new Box(new Vector2(WindowWidth - 8 - BoxDimension, WindowHeight), new Vector2(WindowWidth / 2 + BoxDimension / 2, WindowHeight / 2 + BoxDimension / 2), new Vector2(-6f, -6f), new Vector2(BoxDimension, BoxDimension), pixelForEachBox, Color.Green);
        }

        public void MarkBox(int boxIndex, SpriteBatch spriteBatch, char characterToMark, Texture2D textureForMarking)
        {
            if (!boxes[boxIndex].Marked)
            {
// This box is unmarked. Therefore, mark it, and set the appropriate character to mark.
                System.Diagnostics.Debug.WriteLine("In Board, characterTo Mark ={0}", characterToMark);
                boxes[boxIndex].Marked = true;
                boxes[boxIndex].Character = characterToMark;
                boxes[boxIndex].TextureForMarking = textureForMarking;
            }
            else
            {
// This box is already marked. Therefore, do not update the character marker for this box.
                return;
            }
       }
        public void HighlightBox(int boxIndex, Texture2D highlightingTexture, GameTime gameTime, SpriteBatch spriteBatch)
        {
            boxes[boxIndex].Highlight(gameTime, spriteBatch, highlightingTexture);
        }

        public void Update(GameTime gameTime)
        {
            foreach(Box b in boxes)
            {
                if(!b.GrowInZ)
                {
                    if(!b.HasReachedDestination)
                    {
                        b.currentPosition += b.speed;
                    }
                }
                else
                {
                    if(!b.HasReachedDestination && !b.HasGrownToSize(new Vector2(BoxDimension, BoxDimension)))
                    {
                        b.currentPosition -= b.speed;
                        b.boxDimension += 2 * b.speed;
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, int level=0)
        {
            foreach(Box b in boxes)
            {
                b.Draw(gameTime, spriteBatch, level);
            }
        }
        
        public int CheckWin(out TicTacToeScreen.DrawStrike strikeDecision, out char winningCharacter)
        {

            // Returns Row Win
            for(int i = 0; i <=6; i+=3)
            {
                if(boxes[i].Character == boxes[i + 1].Character && boxes[i+1].Character == boxes[i + 2].Character && ((boxes[i].Character == 'X') || (boxes[i].Character =='O')))
                {
                    strikeDecision = TicTacToeScreen.DrawStrike.Row;
                    winningCharacter = boxes[i].Character;
                    return i;
                }
            }
            for (int i = 0; i < 3; i++ )
            {
                if((boxes[i].Character == boxes[i + 3].Character) && (boxes[i + 3].Character == boxes[i + 6].Character) && (boxes[i].Character.Equals('X') || (boxes[i].Character.Equals('O'))))
                {
                    strikeDecision = TicTacToeScreen.DrawStrike.Column;
                    winningCharacter = boxes[i].Character;
                    return i;
                }
     //           Debug.WriteLine("characters are: {0}, {1}, {2}", boxes[i].Character, boxes[i + 3].Character, boxes[i + 6].Character);
                Debug.WriteLine("");
            }
            if(boxes[0].Character == boxes[4].Character && boxes[4].Character == boxes[8].Character && ((boxes[0].Character == 'X') || (boxes[0].Character == 'O')))
            {
                strikeDecision = TicTacToeScreen.DrawStrike.Diagonal;
// could be 0, 4, 8
                Debug.WriteLine("Diagonal forward win!!!");
                winningCharacter = boxes[0].Character;
                return 1;
            }
            if(boxes[2].Character == boxes[4].Character  && boxes[4].Character== boxes[6].Character && ((boxes[2].Character == 'X' )||( boxes[2].Character == 'O')))
            {
                strikeDecision = TicTacToeScreen.DrawStrike.Diagonal;
// could be 2, 4, 6
                winningCharacter = boxes[2].Character;
                return 2;
            }
            strikeDecision = TicTacToeScreen.DrawStrike.NoDecision;
            winningCharacter = '\0';
                return -1;
        }

        public void DrawStrike(int number, TicTacToeScreen.DrawStrike strikeDecision,SpriteBatch spriteBatch, GameTime gameTime,Texture2D strikingPixel)
        {
// To draw the highlighting
            if (gameTime.TotalGameTime.TotalMilliseconds > nextBlinkTime)
            {
                if (color == Color.Yellow)
                {
                    color = Color.Red;
                }
                else
                {
                    color = Color.Yellow;
                }
                nextBlinkTime = gameTime.TotalGameTime.TotalMilliseconds + 1000f;
            }   
            Color[] drawingColor = new Color[9];
            System.Diagnostics.Debug.WriteLine("Row number is: {0}", number);
            strikingPixel.GetData<Color>(0,new Rectangle(0,0,1,1),drawingColor,0,1);
            float i = 0;
            Vector2 drawingVector = Vector2.Zero;
            if(strikeDecision == TicTacToeScreen.DrawStrike.Row)
            {
                i = boxes[number].currentPosition.X - 15;
                for(; i < boxes[number + 2].currentPosition.X + BoxDimension + 15; i++)
                {
                    spriteBatch.Draw(strikingPixel, new Vector2(i, boxes[number].currentPosition.Y + BoxDimension / 2), drawingColor[0]);
                }
               for(int j = number; j <= number + 2; j++)
               {
                   boxes[j].Highlight(gameTime, spriteBatch, TicTacToeScreen.GetTextureForHighlighting(color, 5));
               }
            }
            else if(strikeDecision == TicTacToeScreen.DrawStrike.Column)
            {
                System.Diagnostics.Debug.WriteLine("Column Win!!!!, at number {0}", number);
                i = boxes[number].currentPosition.Y - 15;
                for(; i < boxes[number + 6].currentPosition.Y + BoxDimension + 15; i++)
                {
                    spriteBatch.Draw(strikingPixel, new Vector2(boxes[number].currentPosition.X+BoxDimension/2, i),drawingColor[0]);
                }
                for (int j = number; j < 9; j += 3)
                {
                    boxes[j].Highlight(gameTime, spriteBatch, TicTacToeScreen.GetTextureForHighlighting(color, 5));
                }
            }
            else if (strikeDecision == TicTacToeScreen.DrawStrike.Diagonal)
            {
                if (number == 1)
                {
                    float y = boxes[0].currentPosition.Y;
                    for (i = boxes[0].currentPosition.X; i < boxes[8].currentPosition.X + BoxDimension; i++)
                    {
                        spriteBatch.Draw(strikingPixel, new Vector2(i, y), drawingColor[0]);
                        ++y;
                    }
                    for(int j = 0; j < 9; j+=4)
                    {
                        boxes[j].Highlight(gameTime, spriteBatch, TicTacToeScreen.GetTextureForHighlighting(color, 5));
                    }
                }
                else if(number == 2)
                {
                    float y = boxes[2].currentPosition.Y;
                    for (i = boxes[2].currentPosition.X + BoxDimension; i >= boxes[6].currentPosition.X; i--)
                    {
                        spriteBatch.Draw(strikingPixel, new Vector2(i, y), drawingColor[0]);
                        y++;
                    }
                    for(int j = 2; j < 9; j +=2)
                    {
                        boxes[j].Highlight(gameTime, spriteBatch, TicTacToeScreen.GetTextureForHighlighting(color, 5));
                    }
                }
            }
        }

    }
}

