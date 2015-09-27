using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TheTicTacToeGame.Screens
{
    public class GoodByeScreen : MenuScreen
    {
        bool timeToExit = false;
         double elapsedTime = 0.0f;
        public GoodByeScreen(): base("Take Care!")
        {

        }

        public override void Update(bool coveredByOtherScreen, bool otherScreenHasFocus, GameTime gameTime)
        {
            if(timeToExit)
            {
                ScreenManager.Game.Exit();
            }
            base.Update(coveredByOtherScreen, otherScreenHasFocus, gameTime);
        }
         public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // draw a faded rectangle
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(elapsedTime > gameTime.ElapsedGameTime.TotalMilliseconds * 600)
            {
                timeToExit = true;
            }
            System.Diagnostics.Debug.WriteLine("Time elapsed is: {0}, and total time tracked: {1}", gameTime.ElapsedGameTime.TotalMilliseconds, elapsedTime);

               SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            float scale = 1.1f * TransitionAlpha;
            Rectangle fullScreen = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width,
                                                        ScreenManager.GraphicsDevice.Viewport.Height);
            Color colorForScreen = Color.Black;
            Texture2D t = new Texture2D(ScreenManager.GraphicsDevice,1,1);
            t.SetData<Color>(new Color[]{colorForScreen});
            spriteBatch.Draw(t, fullScreen, null, colorForScreen, 0, Vector2.Zero, SpriteEffects.None, 0);
           

            // Display Message
            // array useful to add more instructions later on
            String[] communicate = new String[] { "Thanks for playing! ", "If time avails, you can always shoot me", "your feedback at ", "rahul_appdeveloper@outlook.com   !!" };
            Vector2 comVector = Vector2.Zero;
            comVector.Y = ScreenManager.GraphicsDevice.Viewport.Height/2 - 200f;
            // establish destination
            Color color = Color.White;
            color *= TransitionAlpha;
            foreach(String s in communicate)
            {
                comVector.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - ScreenManager.SpriteFont.MeasureString(s).X / 2 - 100f;
                 spriteBatch.DrawString(ScreenManager.SpriteFont, s, comVector, color,0,Vector2.Zero,1.5f,SpriteEffects.None,0);
                 comVector.Y += ScreenManager.SpriteFont.MeasureString(s).Y + 10f;
            }
            comVector.Y += 20f;
            spriteBatch.DrawString(ScreenManager.SpriteFont, "Exiting...A moment please...", comVector, color, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
    

            base.Draw(gameTime);
             // keep screen static for 3 seconds
     //       System.Threading.Thread.Sleep(3000);
      
        }
         

    }
}
