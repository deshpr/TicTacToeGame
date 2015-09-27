using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TheTicTacToeGame.Screens
{
    public class LoadingScreen : GameScreen
    {
        private GameScreen[] screensToLoad;
        public bool shouldTakeTimeToLoad;
        private bool removedAllScreens;
        private double elapsedTime;
        private Texture2D loadingTexture;
        private Rectangle loadingRectangle;
        private Texture2D loadingBoundaryTexture;
        private Rectangle loadedRectangle;

        public LoadingScreen(GameScreen[] screensToLoad, bool shouldTakeTimeToLoad, ScreenManager screenManager)
        {
           
            this.shouldTakeTimeToLoad = shouldTakeTimeToLoad;
            this.screensToLoad = screensToLoad;
            this.ScreenManager = screenManager;
        // tell all screens to move out
            foreach (GameScreen screen in ScreenManager.Screens)
                screen.IsExiting = true;    // takes care of the rest.
            // Add this  to the list of screens
            ScreenManager.Screens.Add(this);
            this.LoadContent();
        }
        public override void LoadContent()
        {
            loadingTexture = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            loadingTexture.SetData<Color>(new Color[] { Color.Green });
            loadingBoundaryTexture = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            loadingBoundaryTexture.SetData<Color>(new Color[] { Color.Yellow });
            
            loadingRectangle = new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 500/2, ScreenManager.GraphicsDevice.Viewport.Height / 2, 20,20);
            base.LoadContent();
        }
        public override void Update(bool coveredByOtherScreen, bool otherScreenHasFocus, Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(coveredByOtherScreen, otherScreenHasFocus, gameTime);   // call this only to upadte the TransitionAlpha position.
            if(removedAllScreens)
            {
                // now remove the loading screen
                ScreenManager.RemoveScreen(this);
                // add the new screens
                for(int i = 0; i < screensToLoad.Count(); i++)
                {
                    ScreenManager.AddScreen(screensToLoad[i]);
                    ScreenManager.Screens[i].LoadContent();
                }
            }
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
           // Remove all the screens
         
            if(shouldTakeTimeToLoad)
            {
                elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if(elapsedTime> gameTime.ElapsedGameTime.TotalMilliseconds * 200)
                {
                    if (ScreenManager.Screens.Count == 1)
                    {
                        removedAllScreens = true;
                    }
                }
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
         
                // get the string to display
                string message = "Loading.........";
                // first draw the loaded rectangle
                loadedRectangle = new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 500/2, ScreenManager.GraphicsDevice.Viewport.Height / 2, 100 * 5,20);
                spriteBatch.Draw(loadingBoundaryTexture, loadedRectangle, Color.Yellow);
                // obtain width of rectangle
                double percent = elapsedTime / (gameTime.ElapsedGameTime.TotalMilliseconds * 200);
                percent *= 100;
                loadingRectangle.Width = (int)(percent * 5);
                System.Diagnostics.Debug.WriteLine("PERCENT = {0}, width = {1}", percent, loadingRectangle.Width);
             
                spriteBatch.Draw(loadingTexture, loadingRectangle, Color.Green);
                Vector2 position = Vector2.Zero;
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - ScreenManager.SpriteFont.MeasureString(message).X / 2;
                position.Y = ScreenManager.GraphicsDevice.Viewport.Height / 2 - 200f;
                float scale = 2.2f;
                if (percent * 5 >= 500)
                    message = "Done !!!";
                spriteBatch.DrawString(ScreenManager.SpriteFont, message, position, Color.Green, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                Debug.WriteLine("done");

            }
            base.Draw(gameTime);
        }

    }


}
