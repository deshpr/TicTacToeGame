using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheTicTacToeGame.Screens
{
    public class BackgroundScreen : MenuScreen
    {
        Texture2D backgroundTexture;
    

        public BackgroundScreen() : base("")
        {

        }
        public override void LoadContent()
        {
            backgroundTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Images\wallpaper");
            base.LoadContent();
        }
        public override void Update(bool coveredByOtherScreen, bool otherScreenHasFocus, GameTime gameTime)
        {
            base.Update(coveredByOtherScreen, true, gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Rectangle fullScreen = new Rectangle(0,0, screenManager.GraphicsDevice.Viewport.Width, screenManager.GraphicsDevice.Viewport.Height);
           spriteBatch.Draw(backgroundTexture, fullScreen, Color.Green);
           base.Draw(gameTime);
        }

    }
}
