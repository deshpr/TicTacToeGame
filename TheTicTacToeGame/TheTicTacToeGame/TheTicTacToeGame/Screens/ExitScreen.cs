using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheTicTacToeGame.Screens
{
    public class ExitScreen : MenuScreen
    {
        private MenuEntry continueGame;
        private MenuEntry exitGame;
        private MenuEntry changeSound;

        public ExitScreen() : base("Quit Game")
        {
            continueGame = new MenuEntry("Continue");
            exitGame = new MenuEntry("Quit Game");
            continueGame.entryClicked += OnContinueGame;
            exitGame.entryClicked += OnExitGame;
            menuEntries.Add(continueGame);
            menuEntries.Add(exitGame);
        }
        public override void Update(bool coveredByOtherScreen, bool otherScreenHasFocus, Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(coveredByOtherScreen, otherScreenHasFocus, gameTime);
            
        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // draw a faded rectangle

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            float scale = 1.1f * TransitionAlpha;
            Rectangle fullScreen = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width,
                                                        ScreenManager.GraphicsDevice.Viewport.Height);
            Color colorForScreen = Color.Black * (TransitionAlpha/1.18f);
            Texture2D t = new Texture2D(ScreenManager.GraphicsDevice,1,1);
            t.SetData<Color>(new Color[]{colorForScreen});
            spriteBatch.Draw(t, fullScreen, null, colorForScreen, 0, Vector2.Zero, SpriteEffects.None, 0);
           

            // Display Message
            // array useful to add more instructions later on
            String[] communicate = new String[] { "Are you sure you want to quit the game? " };
            Vector2 comVector = Vector2.Zero;
            comVector.Y = ScreenManager.GraphicsDevice.Viewport.Height/2 - 200f;
            // establish destination
            Color color = Color.White;
            color *= TransitionAlpha;
            foreach(String s in communicate)
            {
                comVector.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - ScreenManager.SpriteFont.MeasureString(s).X / 2 - 100f;
                 if(screenstate == ScreenState.TransitionOn)
                 {
                     // enter the screen
                     // reduce the amount by which distance from the destination exists
                     comVector.Y -= (1 - TransitionAlpha) * 100;
                 }
                 spriteBatch.DrawString(ScreenManager.SpriteFont, s, comVector, color,0,Vector2.Zero,1.5f,SpriteEffects.None,0);
            }

            base.Draw(gameTime);
        }
        private void OnExitGame(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
            IsExiting = true;
            ScreenManager.Screens.Remove(this);
            GoodByeScreen gbyscreen = new GoodByeScreen();
            gbyscreen.ScreenManager = ScreenManager;
            gbyscreen.LoadContent();
            ScreenManager.AddScreen(gbyscreen);

        }
        private void OnContinueGame(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
            IsExiting = true;
           // do not add another screen on top again. 
        }


    }
}
