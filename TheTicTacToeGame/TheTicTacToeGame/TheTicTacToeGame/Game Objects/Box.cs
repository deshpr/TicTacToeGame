using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheTicTacToeGame.Game_Objects
{
    public class Box : IComparable<Box>
    {
        public Vector2 start;
        public Vector2 destination;
        public Vector2 speed;
        public Vector2 currentPosition;
        public Texture2D pixelToUse;
        public Vector2 boxDimension;
        public bool GrowInZ;
        public Color[] color;
        public Color fillColor;
        public Texture2D TextureForMarking { get; set; }

        public Box(Vector2 start,Vector2 destination, Vector2 speed, Vector2 boxDimension,Texture2D pixelToUse, Color fillColor,bool GrowInZ = false)
        {
            this.start = start;
            this.destination = destination;
            this.currentPosition = start;
            this.speed = speed;
            this.boxDimension = boxDimension;
            this.pixelToUse = pixelToUse;
            this.GrowInZ = GrowInZ;
            color = new Color[9];
            pixelToUse.GetData<Color>(0, new Rectangle(0,0,3,3), color, 0, 9);
            this.fillColor = fillColor;
        }

        

        public bool HasGrownToSize(Vector2 size)
        {
            return size == boxDimension;
        }

        public bool HasReachedDestination
        {
            get
            {
                return currentPosition == destination;
            }
            
        }

        public char Character { get; set; }
        public bool Marked { get; set; }

        
        // Use this function to highlight current selection
        public void Highlight(GameTime gameTime, SpriteBatch spriteBatch, Texture2D highlightingTexture)
        {
            Texture2D tempPixel = pixelToUse;
            this.pixelToUse = highlightingTexture;
            Vector2 tempPosition = currentPosition;
            currentPosition.X++;
            currentPosition.Y++;
            boxDimension.X -= 5; 
            boxDimension.Y -=5;
            this.pixelToUse.GetData<Color>(0, new Rectangle(0, 0, 3, 3), this.color, 0,9);
            this.Draw(gameTime, spriteBatch);
            currentPosition = tempPosition;
            boxDimension.X += 5;
            boxDimension.Y+=5;
            pixelToUse = tempPixel;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, int level=0)
        {
// Draw Rectangle
            for(int j=0; j< boxDimension.Y; j++)   // or boxDimension.Width
            {
// -------------
                spriteBatch.Draw(pixelToUse, new Vector2(currentPosition.X + j, currentPosition.Y),color[0]);
// -------------
                spriteBatch.Draw(pixelToUse, new Vector2(currentPosition.X + j, currentPosition.Y + boxDimension.Y), color[0]);
// |||||||||||||
                spriteBatch.Draw(pixelToUse, new Vector2(currentPosition.X, currentPosition.Y + j), color[0]);
// |||||||||||||
                spriteBatch.Draw(pixelToUse, new Vector2(currentPosition.X + boxDimension.X, currentPosition.Y + j), null,color[0]);
            }
// Check if the box needs to be Marked
            if(Marked)
            {
                Texture2D marker;
                if (TextureForMarking == null)
                    marker = pixelToUse;
                else
                    marker = TextureForMarking;

                if(Character.Equals('X'))
                {
// Draw 'X'
                    float topYCoordinate = currentPosition.Y + 5;
                    float bottomYCoordinate = currentPosition.Y + boxDimension.Y - 5;
                    for (float i = currentPosition.X + 5; i < currentPosition.X + boxDimension.X - 5; i++)
                    {
                        spriteBatch.Draw(marker, new Vector2(i, topYCoordinate++), color[0]);
                        spriteBatch.Draw(marker, new Vector2(i, bottomYCoordinate--), color[0]);
                    }
                }
                else if(Character.Equals('O'))
                {
// Draw 'O'               
// Use the circle equation to obtain y-points of the circle

                    for(float i = -boxDimension.X; i!=boxDimension.X-5; i+=0.5f)
                    {
                        float yCoordinate = (float)Math.Pow(Math.Pow(boxDimension.X/3,2) - Math.Pow(i, 2), 0.5);
                        spriteBatch.Draw(marker, new Vector2(currentPosition.X + boxDimension.X/2 + i,currentPosition.Y + boxDimension.Y/2 - yCoordinate), color[0]);
                        spriteBatch.Draw(marker, new Vector2(currentPosition.X + boxDimension.X / 2 + i, currentPosition.Y + boxDimension.Y / 2 + yCoordinate), color[0]);
                    }
                }
            }
        }
            
// Implement methods of the IComparable interface
        public int CompareTo(Box b)
        {
            if (this.Character.Equals(b.Character))
                return 0;
            else
                return 1;   // dummy
        }


    }
}
