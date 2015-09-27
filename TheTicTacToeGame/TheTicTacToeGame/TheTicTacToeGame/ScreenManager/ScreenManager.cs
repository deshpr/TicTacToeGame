using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
// Imports Game type
using Microsoft.Xna.Framework.Content;
// SpriteBatch
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TheTicTacToeGame
{
    public class ScreenManager : Microsoft.Xna.Framework.DrawableGameComponent
    {


        private bool playerCharacterIsO;
        public bool PlayerCharacterIsO
        {
            get { return playerCharacterIsO; }
            set { playerCharacterIsO = value; }
        }
        List<GameScreen> screens = new List<GameScreen>();
        Stack<GameScreen> screensToUpdate = new Stack<GameScreen>();
        bool coveredByOtherScreen;
        static int i = 0;
        bool otherScreenHasFocus;
        private SpriteBatch spriteBatch;
        public bool playSound =true;
        public AudioEngine audioEngine;
        public SoundBank soundBank;
        public WaveBank waveBank;
        public Cue bakcgroundMusic;
        public AudioCategory audioCategory;

        public List<GameScreen> Screens
        {
            get { return screens; }
        }
        
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch;    }
            set
            {
                spriteBatch = value;
            }
        }

       

        public float GetFontHeight(String title)
        {
                return spriteFont.LineSpacing;
        }

        public float GetFontWidth(String title)
        {
            return spriteFont.MeasureString(title).X;
        }

        private SpriteFont spriteFont;
        public SpriteFont SpriteFont
        {
            get { return spriteFont; }
            set
            {
                spriteFont = value;
            }
        }
        public ScreenManager(Game game) : base(game)
        {
              // Start background music
            audioEngine = new AudioEngine(@"Content\Audio\boardClickSound.xgs");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            audioCategory = audioEngine.GetCategory("background");
        }

        public override void Initialize()
        {
            spriteFont = Game.Content.Load<SpriteFont>(@"Fonts\Segoe UI Mono");
             
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            audioEngine = new AudioEngine(@"Content\Audio\boardClickSound.xgs");
            soundBank = new SoundBank(audioEngine,@"Content\Audio\Sound Bank.xsb");
            waveBank = new WaveBank(audioEngine,@"Content\Audio\Wave Bank.xwb");
            
            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            coveredByOtherScreen = false;
            otherScreenHasFocus = !Game.IsActive; ;
            screensToUpdate.Clear();
            for (int i = 0; i < screens.Count; i++)
            {
                screensToUpdate.Push(screens[i]);
            }

            while(screensToUpdate.Count > 0)
            {
                GameScreen screen = screensToUpdate.Pop();
                screen.Update(coveredByOtherScreen, otherScreenHasFocus, gameTime);
       
                if(screen.ScreenState==ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                   if(!otherScreenHasFocus)
                   {
                       screen.ObtainInput();
                       otherScreenHasFocus = true;
                   }
                        
                       coveredByOtherScreen = true;
                }
              
            }
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

             foreach(GameScreen screen in screens)
             {
                 screen.Draw(gameTime);
             }
             spriteBatch.End();

            base.Draw(gameTime);
            
        }
        public void AddScreen(GameScreen gameScreen)
        {
            gameScreen.ScreenManager = this;
            screens.Add(gameScreen);
        }
        public void RemoveScreen(GameScreen gameScreen)
        {
            screens.Remove(gameScreen);
        }
        public void PlaySound(String cueName)
        {
            if(playSound)
            soundBank.PlayCue(cueName);
        }
    
    }


}
