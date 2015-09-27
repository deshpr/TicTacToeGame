using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TheTicTacToeGame.Screens;
// SpriteBatch
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using TheTicTacToeGame.Screens.Screen_Event_Handlers;

namespace TheTicTacToeGame
{
    public class MenuEntry
    {
        static int i = 0;
        public String title;
        public Vector2 position = Vector2.Zero;
        float selectionFade;
        public event EventHandler<ScreenEventArgs> entryClicked;

        public virtual void OnEntryClicked(Keys k)
        {
            if (entryClicked != null)
                entryClicked(this, new ScreenEventArgs(title, k));
        }

        public MenuEntry(String title ="")
        {
            this.title = title;
        }

        public float GetWidth(MenuScreen menuScreen)
        {
            return menuScreen.ScreenManager.SpriteFont.MeasureString(title).X;
            
        }
        public float GetHeight(MenuScreen menuScreen)
        {
            return menuScreen.ScreenManager.SpriteFont.LineSpacing;
        }

        public void Update(GameTime gameTime, bool isSelected)
        {
            if (isSelected)
                selectionFade = (float)Math.Min(selectionFade + gameTime.ElapsedGameTime.TotalMilliseconds * 4, 1);
            else
                selectionFade = (float)Math.Max(selectionFade - gameTime.ElapsedGameTime.TotalMilliseconds * 4, 0);
        }
        public void Draw(MenuScreen menuScreen,GameTime gameTime, bool isSelected)
        {
            ++i;
            SpriteBatch spriteBatch = menuScreen.ScreenManager.SpriteBatch;
            
            // determine the scale using a sine function
            float pulsate =  6 +  (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6);
            Color color = isSelected? Color.Yellow  : Color.White;
            color = color * menuScreen.TransitionAlpha;
            float scale = 2 + 0.05f * pulsate * selectionFade;
            Debug.WriteLine("Position of {0} is X: {1}, Y: {2}", title,position.X, position.Y);
            spriteBatch.DrawString(menuScreen.ScreenManager.SpriteFont, title, position, color,0,Vector2.Zero,scale,SpriteEffects.None,0);   
        }

    }
}
