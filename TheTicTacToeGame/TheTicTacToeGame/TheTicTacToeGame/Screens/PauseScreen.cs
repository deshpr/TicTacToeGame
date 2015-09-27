using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TheTicTacToeGame.Screens
{
    public class PauseScreen : MenuScreen
    {
        MenuEntry settings;
        MenuEntry continueGame;
        MenuEntry quitGame;
        MenuEntry restartGame;

        public PauseScreen() : base("Game Paused")
        {
            // hide the tic tac toe screen
           TicTacToeScreen.HideMe = true;
            settings = new MenuEntry("Settings");
            continueGame = new MenuEntry("Continue Game");
            quitGame  = new MenuEntry("Quit Game");
            restartGame = new MenuEntry("Restart Game");
            continueGame.entryClicked += OnContinueGame;
            settings.entryClicked += OnSettings;
            menuEntries.Add(settings);
            menuEntries.Add(continueGame);
            menuEntries.Add(quitGame);
            menuEntries.Add(restartGame);
        }

        public void OnContinueGame(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
            TicTacToeScreen.HideMe = false;
            IsExiting = true;
            ScreenManager.Screens[0].ScreenState = ScreenState.Active;
     
        }
        public void OnSettings(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
            SettingsMenuScreen settings = new SettingsMenuScreen(ScreenManager);
            TicTacToeScreen s = (TicTacToeScreen)sender;
            
            settings.LoadContent();
            ScreenManager.AddScreen(settings);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // draw a faded rectangle

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            float scale = 1.1f * TransitionAlpha;
            Rectangle fullScreen = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width,
                                                        ScreenManager.GraphicsDevice.Viewport.Height);
            Color colorForScreen = Color.Black * (TransitionAlpha / 1.18f);
            Texture2D t = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            t.SetData<Color>(new Color[] { colorForScreen });
            spriteBatch.Draw(t, fullScreen, null, colorForScreen, 0, Vector2.Zero, SpriteEffects.None, 0);

            // Display Message
            // array useful to add more instructions later on
            String[] communicate = new String[] { "Are you sure you want to quit the game? " };
            Vector2 comVector = Vector2.Zero;
            comVector.Y = ScreenManager.GraphicsDevice.Viewport.Height / 2 - 200f;
            // establish destination
            Color color = Color.White;
            color *= TransitionAlpha;
            foreach (String s in communicate)
            {
                comVector.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - ScreenManager.SpriteFont.MeasureString(s).X / 2 - 100f;
                if (screenstate == ScreenState.TransitionOn)
                {
                    // enter the screen
                    // reduce the amount by which distance from the destination exists
                    comVector.Y -= (1 - TransitionAlpha) * 100;
                }
                spriteBatch.DrawString(ScreenManager.SpriteFont, s, comVector, color, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            }

            base.Draw(gameTime);
        }

    
    
    }
}
