using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// Import GameTime, Game
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TheTicTacToeGame
{

    public enum ScreenState{
        TransitionOn,
        Active,
        TransitionOff,
        InActive
    }

    public class GameScreen
    {
        // do not set to Zero!
        private float transitionPosition = 1;
        public float TransitionPosition
        {
            get
            {
                return transitionPosition;
            }
            set
            {
                transitionPosition = value;
            }
        }

        private bool isExiting = false;
        public bool IsExiting
        {
            get { return isExiting; }
            set { isExiting = value; }
        }

        private TimeSpan transitionOnTime = TimeSpan.Zero;
        public TimeSpan TransitionOnTime
        {
            get
            {
                return transitionOnTime;

            }
            set
            {
                transitionOnTime = value;
            }
        }

        private TimeSpan transitionOfftime = TimeSpan.Zero;
        public TimeSpan TransitionOffTime
        {
            get
            {
                return transitionOfftime;
            }
            set
            {
                transitionOfftime = value;
            }
        }


        public float TransitionAlpha
        {
            get
            {
                return 1 - transitionPosition;
            }
        }

       ///<summary>
       ///Contains the screen state 
       ///of the game screen
       ///</summary>
        protected ScreenState screenstate = ScreenState.TransitionOn;
        public ScreenState ScreenState
        {
            get { return screenstate;  }
            set { screenstate = value; }
        }

        protected ScreenManager screenManager;
        public ScreenManager ScreenManager
        {
            get
            {
                return screenManager;
            }
            set
            {
                screenManager =value;
            }
        }


        public virtual void ObtainInput() { }
        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void Update(bool coveredByOtherScreen, bool otherScreenHasFocus,GameTime gameTime)
        {
                    if(IsExiting)
                    {
                        screenstate = ScreenState.TransitionOff;
                        

                        if(FinishedTransition(gameTime, transitionOfftime,+1))
                       {
                           ScreenManager.RemoveScreen(this);
                        }
                       
                    }
                   else if(coveredByOtherScreen)
                   {
                       // dimnish this screen.
                       if(!FinishedTransition(gameTime, TransitionOffTime, +1))
                       {
                           screenstate = ScreenState.TransitionOff;
                       }
                       else
                       {
                           screenstate = ScreenState.InActive;
                       }
                   }
                   else
                   {
                       // display this screen
                       if(!FinishedTransition(gameTime,transitionOnTime,-1))
                       {
                           screenstate = ScreenState.TransitionOn;
                       }
                       else
                       {
                           screenstate = ScreenState.Active;
                       }
                   }
        }
        

        public virtual void Draw(GameTime gameTime){
        // Draw the trademark
            String tradeMarkMessage = "Created By Rahul, Inc";
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont spriteFont = ScreenManager.SpriteFont;
            Vector2 location = new Vector2
                (
                    ScreenManager.GraphicsDevice.Viewport.Width/2 - spriteFont.MeasureString(tradeMarkMessage).X/2,
                    ScreenManager.GraphicsDevice.Viewport.Height - 1.6f * spriteFont.MeasureString(tradeMarkMessage).Y 
                );
            spriteBatch.DrawString(spriteFont, "Created By Rahul, Inc", location, Color.White);
        }

         public bool FinishedTransition(GameTime gameTime, TimeSpan time, int direction)
        {
             // Update the position
             // increase the speed as time progresses
            float transitionDelta = (float)gameTime.ElapsedGameTime.TotalMilliseconds /
                                           time.Milliseconds;
            transitionPosition += transitionDelta * direction;

            // Update the transitionPosition
             if(transitionPosition <= 0 && direction < 0 || transitionPosition >= 1 && direction > 0)
             {
                 transitionPosition = (float)MathHelper.Clamp(transitionPosition, 0, 1);
                 // finished transitioning!
                 return true;
             }
             // still busy transitioning
             return false;
        }

  


       

    }
}
