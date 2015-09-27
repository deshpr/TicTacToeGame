using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
namespace TheTicTacToeGame.Screens
{
    public class MenuScreen : GameScreen
    {

        int selectedEntry = 0;
        KeyboardState state;
        private String title;
        public String Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        protected List<MenuEntry> menuEntries = new List<MenuEntry>();
        private String[] instructions;

        public MenuScreen(String title)
        {
            instructions = new String[] { "Use the Arrow Keys to navigate to options", " And the Enter Key to select an option" };
            this.title = title;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }



        public override void ObtainInput()
        {
            KeyboardState newKeyboardState = Keyboard.GetState();
            if(state!=null)
            {
                if(state.IsKeyDown(Keys.Down) && newKeyboardState.IsKeyUp(Keys.Down))
                {
                    ++selectedEntry;
                    if (selectedEntry == menuEntries.Count)
                        selectedEntry = 0;
                    PlayOptionClickSound("menuMouseClick");
                }
                if(state.IsKeyDown(Keys.Up) && newKeyboardState.IsKeyUp(Keys.Up))
                {
                    --selectedEntry;
                    if (selectedEntry  < 0)
                        selectedEntry = menuEntries.Count - 1;
                    PlayOptionClickSound("menuMouseClick");
                }
                DetectKeyPress(Keys.Enter, newKeyboardState);
                // these key presses are only for the settings menu screen.
                if(this is SettingsMenuScreen)
                {
                    DetectKeyPress(Keys.Left, newKeyboardState);
                    DetectKeyPress(Keys.Right, newKeyboardState);
                
                }
                
            }
            state = newKeyboardState;
        }

        public virtual void DetectKeyPress(Keys k, KeyboardState newKeyboardState)
        {
            if (state.IsKeyDown(k) && this.screenstate == ScreenState.Active && newKeyboardState.IsKeyUp(k))
            {
                menuEntries[selectedEntry].OnEntryClicked(k);
                //        ScreenManager.soundBank.PlayCue("cashAudio");
                PlayOptionClickSound("menuMouseClick");
            }
           
        }
      
        // Update the positions of the menu items
        public void UpdatePositions()
        {
            Vector2 position = new Vector2(0f, 250f);   
            for(int i = 0; i < menuEntries.Count; i++)
            {
                float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - ScreenManager.SpriteFont.MeasureString(menuEntries[i].title).X/ 2;
                if(screenstate == ScreenState.TransitionOn)
                {
                    Debug.WriteLine("transitionPosition is {0}", TransitionPosition);
         //           Debug.WriteLine("transi")
                    position.X -= transitionOffset * 256;
                }
                else
                {
                    // move out of the screen
                    position.X += transitionOffset * 512;
                }
                menuEntries[i].position = position;
              
                position.Y += menuEntries[i].GetHeight(this) + 30;
            }
        }
        public override void Update(bool coveredByOtherScreen, bool otherScreenHasFocus, GameTime gameTime)
        {
            base.Update(coveredByOtherScreen, otherScreenHasFocus, gameTime);
             for(int i = 0; i < menuEntries.Count; i++)
             {
                 menuEntries[i].Update(gameTime, i == selectedEntry);
             }
             UpdatePositions();  // update menu entries
       
        }
        public override void Draw(GameTime gameTime)
        {
            for(int i = 0; i < menuEntries.Count; i++)
            {
                menuEntries[i].Draw(this, gameTime, i == selectedEntry);
            }

            Vector2 instructionVector = Vector2.Zero;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont spriteFont = ScreenManager.SpriteFont;
            // draw the title
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            Vector2 position = Vector2.Zero;
            position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2  - spriteFont.MeasureString(title).X/2;
            position.Y = 120f;     
            if(screenstate == ScreenState.TransitionOn)
            {
                position.Y -= transitionOffset * 100;
            }
            else
            {
                position.Y += transitionOffset * 512;
            }
            Color color = new Color(192, 192, 192) * TransitionAlpha;
            spriteBatch.DrawString(spriteFont, title, position, color, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 0);
            // do the same for the instructions
            
            instructionVector.Y = ScreenManager.GraphicsDevice.Viewport.Height - position.Y - 30f;
             if(screenstate == ScreenState.TransitionOff)
            {
               // use the same variable, only with a different 'x'
                instructionVector.Y += transitionOffset * 100;
            }
           
            foreach (String instruction in instructions)
            {
                instructionVector.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - spriteFont.MeasureString(instruction).X / 2 - 100f;
                spriteBatch.DrawString(spriteFont, instruction, instructionVector, color, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
                instructionVector.Y += ScreenManager.SpriteFont.MeasureString(instruction).Y + 10f;
                   
            }
            base.Draw(gameTime);
        }
       public virtual void PlayOptionClickSound(String soundName)
        {
            ScreenManager.soundBank.PlayCue(soundName);
        }

    }
}
