using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using TheTicTacToeGame.Game_Objects;


namespace TheTicTacToeGame.Screens
{ 
    public class TicTacToeScreen :  GameScreen
    {
// Declare the reference types
// Board of the game.
        Board board;
        static GraphicsDevice myGraphicsDevice;
        public static bool playSound = true;
        public static bool HideMe = false;
// Each component may have its own SpriteBatch object
        SpriteBatch spriteBatch;
        Texture2D pixelToUseForDrawing;
        Color[] colorForPixel;
        KeyboardState keyboardState;
        HumanMachine humanMachine;
        Texture2D backgroundTexture;
        public static int ThreadCount = 0;
       
// Declare the primitive types.
// Player specific parameters.
        int userIndex;
        bool markPlayerPosition;
        char characterToMark;
        bool displayPlayerChance = true;
        String messageToDisplay = String.Empty;
        public char PlayerCharacter { get; set; }
        Dictionary<int, int> positionMapper;

// HumanMachine specific parameters.
        int machineIndex;
        bool markMachinePosition;
        bool machineIsThinking;
        int humanMachineDecision;
        Vector2 instructionVector;

// Use this thread to traverse the AITree and 
// determine the decision to make.
        Thread machineDecisionThread;
 

// Overall game parameters.

// Primitive types.
        int checkWin;
        bool drawStrike = false;
        bool gameOver = false;
        bool isDraw = false;
        String instructions = String.Empty;  
// Reference types.
        SpriteFont spriteFont;
        float timeFromWin = 0.0f;
       
        Cue gameOverCue;
         PauseScreen pauseScreen;
        
        // Character that won in a row or column or diagonal.
        char winningCharacter;
        public enum DrawStrike{Row, Column, Diagonal, NoDecision};
        DrawStrike strikeDecision;
        
// RandomNumberGenerator for waiting the 
// thread obtaining the machine decision.
        Random random;
     
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public TicTacToeScreen()
        {
            // TODO: Add your initialization code here
// Initialize primitive types.
// Player specific parameters.
            userIndex = 0;
            markPlayerPosition = false;
            instructionVector = Vector2.Zero;
// Machine specific parameters
            markMachinePosition = false;
     //       humanMachine = new HumanMachine(ScreenManager.PlayerCharacterIsO ? 'O' : 'X');
            
// Overall Game parameters
            positionMapper = null;
            instructions = "Pres 'Enter' to place your character.\n Press Escape Key to Pause the Game!";
            // Create the AITree
            
            machineIsThinking = false;
// Seed = 2
            random = new Random(2);
            machineDecisionThread = new Thread(MachineDecisionThreadDelegate);
            
            base.Initialize();
        }
        public override void LoadContent()
        {
           
            myGraphicsDevice = ScreenManager.GraphicsDevice;
            spriteBatch = ScreenManager.SpriteBatch;
            spriteFont = ScreenManager.Game.Content.Load<SpriteFont>(@"fonts\Segoe UI Mono");
            pixelToUseForDrawing = new Texture2D(ScreenManager.GraphicsDevice, 3, 3);
            colorForPixel = new Color[9];
            for (int i = 0; i < 9; i++)
                colorForPixel[i] = Color.Red;
            pixelToUseForDrawing.SetData<Color>(colorForPixel);
            board = new Board(pixelToUseForDrawing, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
            backgroundTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Images\wallpaper");

            gameOverCue = ScreenManager.soundBank.GetCue("taDa");
// The mapping to create relies on te board.
// Therefore, place mapping initialization
// in the LoadContent() method.
            positionMapper = GenerateMapping();
            PlayerCharacter = ScreenManager.PlayerCharacterIsO ? 'O' : 'X';
            humanMachine = new HumanMachine(PlayerCharacter == 'O' ?  'X' : 'O');
            characterToMark = humanMachine.Character;
   
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(bool coveredByOtherScreen, bool otherScreenHasFocus, GameTime gameTime)
        {
            if (!HideMe)
            {
                base.Update(coveredByOtherScreen, otherScreenHasFocus, gameTime);
                positionMapper = null;
                TimeSpan timeFromWin = TimeSpan.Zero;

                // Detech which key was pressed
                // and determine which box to highlight
                int i = 0, j = 0;
                KeyboardState newKeyboardState = Keyboard.GetState();
                if (!gameOver)
                {
                    if (keyboardState != null)
                    {
                        // Create the mapper only if 
                        // a key was pressed
                        // Drawback: mapper is generated regardless if user
                        // presses key into an already marked box.
                        // Furthermore, create the mapper only if it's null,
                        // and the mapper is only made null after the humanMachine has made
                        // a move.
                        if (keyboardState.GetPressedKeys() != null)
                        {
                            // The player pressed a Key. Now is the time to 
                            // generate the mapping to determine the position the user
                            // pressed, to pass to the game's AI module, or named as
                            //  the HumanMachine!
                            positionMapper = GenerateMapping();
                            // Left and right key presses
                            if (keyboardState.IsKeyDown(Keys.Right) && newKeyboardState.IsKeyUp(Keys.Right))
                            {
                                ScreenManager.PlaySound("menuMouseClick");
                                userIndex++;
                                int limiter = 3;
                                if (userIndex >= 3 && userIndex <= 5)
                                    limiter = 6;
                                if (userIndex >= 6 && userIndex <= 8)
                                    limiter = 9;
                                userIndex %= limiter;
                            }
                            else if (keyboardState.IsKeyDown(Keys.Left) && newKeyboardState.IsKeyUp(Keys.Left))
                            {
                                ScreenManager.PlaySound("menuMouseClick");
                                userIndex--;
                                if (userIndex < 0)
                                    userIndex += 3;
                            }
                            // Down Key Presses
                            if (keyboardState.IsKeyDown(Keys.Down) && newKeyboardState.IsKeyUp(Keys.Down))
                            {
                                ScreenManager.PlaySound("menuMouseClick");

                                userIndex += 3;
                                userIndex %= 9;
                            }
                            // Up Key presses
                            if (keyboardState.IsKeyDown(Keys.Up) && newKeyboardState.IsKeyUp(Keys.Up))
                            {

                                ScreenManager.PlaySound("menuMouseClick");
                                userIndex -= 3;
                                if (userIndex < 0)
                                    userIndex += 3;
                            }

                            else if (keyboardState.IsKeyDown(Keys.Enter) && newKeyboardState.IsKeyUp(Keys.Enter))
                            {

                                ScreenManager.PlaySound("boardClick");

                                if (!machineIsThinking)
                                {
                                    markPlayerPosition = true;
                                }
                                //   characterToMark = 'O';
                                System.Diagnostics.Debug.WriteLine("Pressed O");
                            }
                            // Additional functionality to add  -PAUSE SCREEN
/*
                         else if (keyboardState.IsKeyDown(Keys.Escape))
                        {

                            pauseScreen = new PauseScreen();
                            pauseScreen.ScreenManager = ScreenManager;
                            ScreenManager.Screens.Add(pauseScreen);
                            ScreenManager.Screens[ScreenManager.Screens.Count - 1].LoadContent();
                            HideMe = true;
                        }
  
 */
                        }
                    }
                    checkWin = board.CheckWin(out strikeDecision, out winningCharacter);
                    if (checkWin != -1)
                    {
                        // This means the player has won the game. Therefore,
                        // do not bother to obtain the HumanMachine's decision
                        // since he has lost!
                        //  Enable drawing a strike. Do not 
                        // check for the logic in the Draw method.
                        gameOver = true;
                        if (!gameOverCue.IsPlaying && !gameOverCue.IsStopped && !gameOverCue.IsStopping)
                        {
                            if (ScreenManager.playSound)
                                gameOverCue.Play();
                        }
                        drawStrike = true;
                    }
                        // Calls update for every box in the board
                    if (markPlayerPosition && !machineIsThinking && !gameOver)
                    {
                        // Player marked an option.
                        // Get the machine's move.
                        // Only enter this block of code if the Thread
                        // has ended. Do not enter while it is sleeping.
                        if (machineDecisionThread.ThreadState != System.Threading.ThreadState.Running || machineDecisionThread.ThreadState != System.Threading.ThreadState.WaitSleepJoin)
                        {

                            machineDecisionThread.Start();
                            machineDecisionThread = new Thread(MachineDecisionThreadDelegate);
                        }
                        // Now reinitalize the thread, since it is dead, and is required
                        // next time the user makes a move!
                    }
                    Debug.WriteLine("The human Machune character is: {0}, and {1}", characterToMark, humanMachine.Character);
                }
                else
                {
                    // end the machineDecision Thread if it is running
                    if (machineDecisionThread.IsAlive)
                    {
                        machineDecisionThread.Join();
                    }
                    // now check if user pressed 'R' to restart the game!
                    if (keyboardState.IsKeyDown(Keys.R) && newKeyboardState.IsKeyUp(Keys.R) && gameOver)
                    {
                        // end the game
                        LoadingScreen screen = new LoadingScreen(new GameScreen[] { new BackgroundScreen(), new ChooseCharacterScreen() }, true, ScreenManager);
                    }
                    else if (keyboardState.IsKeyDown(Keys.Q) && newKeyboardState.IsKeyUp(Keys.Q) && gameOver)
                    {
                        // end the game
                        LoadingScreen screen = new LoadingScreen(new GameScreen[] { new BackgroundScreen(), new StartMenuScreen() }, true, ScreenManager);
                    }
                    if (drawStrike == false)
                    {
                        // this means the game is a draw

                    }
                }
                keyboardState = newKeyboardState;

                board.Update(gameTime);
            }
            else
            {
                Debug.WriteLine("do nothing, since HideMe = {0}", HideMe);
            }
        }
        public override void Draw(GameTime gameTime)
        {
          
  //          spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
// Draw instructions
            if(!drawStrike)
            {
                // game not over yet
                Vector2 position = Vector2.Zero;
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - ScreenManager.SpriteFont.MeasureString(instructions).X / 2;
                position.Y = 10f;
                spriteBatch.DrawString(ScreenManager.SpriteFont, instructions, position, Color.Yellow);
            }
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.Green);
            board.Draw(gameTime, spriteBatch, 0);
            board.HighlightBox(userIndex, GetTextureForHighlighting(Color.Yellow,7), gameTime, spriteBatch);
            if (markPlayerPosition)
            {
     //           if (characterToMark.Equals('X'))
                    board.MarkBox(userIndex, spriteBatch, PlayerCharacter, GetTextureForHighlighting(Color.Green, 9));
  //              else if (characterToMark.Equals('O'))
  //                  board.MarkBox(userIndex, spriteBatch, PlayerCharacter, GetTextureForHighlighting(Color.Green, 9));
            }

            markPlayerPosition = false;
            if(markMachinePosition)
            {
     //           if (humanMachine.Character.Equals('X'))
                    board.MarkBox(humanMachineDecision, spriteBatch, humanMachine.Character, GetTextureForHighlighting(Color.Green, 9));
  //              else if (humanMachine.Character.Equals('O'))
  //                  board.MarkBox(humanMachineDecision, spriteBatch, humanMachine.Character, GetTextureForHighlighting(Color.Green, 9));
            }
            markMachinePosition = false;

            Vector2 messageVector = Vector2.Zero;
            if (displayPlayerChance && !gameOver)
            {
                messageToDisplay = "Please use the arrow keys to move to a Box!\n";
                instructions = "Press Enter to place your character!\n";
            }
            else
            {
                messageToDisplay = "I'm thinking.......";
                instructions = String.Empty;
            }
            if(drawStrike)
            {
// This means the game is over.
                board.DrawStrike(checkWin, strikeDecision, spriteBatch, gameTime,GetTextureForHighlighting(Color.Purple, 9));
//                spriteBatch.DrawString(spriteFont, "GAME OVER!", new Vector2(ScreenManager.GraphicsDevice.Viewport.Width/2 - 20f,30f),Color.DarkGreen,0,Vector2.Zero,2.5f,SpriteEffects.None, 0);
      
                if(winningCharacter.Equals(PlayerCharacter))
                {

                    messageToDisplay = "You won the game!";
                }
                else 
                {
// HumanMachine won the Game!
                    messageToDisplay = "You Lost!!!";
                }
            }
            if(gameOver==true && !drawStrike)
            {
                // it's a draw
                messageToDisplay = "It's a draw! Well done! Would you\n like to play again? ";
            //    instructions = "Would  you like to play again?";
                instructions = "Press 'R' to restart, or 'Q' to quit :)";
            }
           else if(gameOver==true && drawStrike)
            {
                instructions = "Press 'R' to restart, or 'Q' to quit :)";
            }
            messageVector = spriteFont.MeasureString(messageToDisplay);
            messageVector.Y = ScreenManager.GraphicsDevice.Viewport.Height / 2 - 2 * Board.BoxDimension - messageVector.Y;
            messageVector.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - messageVector.X / 2;
            spriteBatch.DrawString(spriteFont, messageToDisplay,
                messageVector, Color.Yellow, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            if(instructions!= String.Empty)
            {
                instructionVector.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - spriteFont.MeasureString(instructions).X / 2 - 10f;
                instructionVector.Y = messageVector.Y + spriteFont.MeasureString(messageToDisplay).Y + 10f;
                spriteBatch.DrawString(spriteFont, instructions, instructionVector, Color.Yellow,0,Vector2.Zero,1.5f,SpriteEffects.None,0);
            }
 //           spriteBatch.End();
            base.Draw(gameTime);
        }

        public static Texture2D GetTextureForHighlighting(Color color, int dimension)
        {
            Color[] colors = new Color[dimension * dimension];
            for (int i = 0; i < dimension * dimension; i++)
            {
                colors[i] = color;
            }
            Texture2D newTexture = new Texture2D(myGraphicsDevice, dimension, dimension);
            newTexture.SetData<Color>(colors);
            return newTexture;
        }

        public Dictionary<int,int> GenerateMapping()
        {
            Dictionary<int, int> mapper = new Dictionary<int, int>();
            int i = 0, j = 0;
            foreach(Box b in board.Boxes)
            {
// This box has no character, and is hence
// empty. Include it in the mapping.
                if(!b.Marked)
                {
                    mapper.Add(i, j++);
                }
                ++i;
            }
            return mapper;
        }

        public Keys GetKeyForCharacter(char ch)
        {
            return ch == 'O' ? Keys.O : Keys.X;
        }

        public void MachineDecisionThreadDelegate()
        {
            displayPlayerChance = false;
            machineIsThinking = true;
            ThreadCount++;
            Debug.WriteLine("Increased ThreadCount is: {0}", ThreadCount);
            // Generate mappping again
            int temp = positionMapper[userIndex];
            // At least make the thread sleep for 3 seconds.
            Thread.Sleep(random.Next(3, 5) * 1000);
            // The HumanMachine has now made a move after the player,
            // therefore, since the player has made a mark, generate a 
            // new mapping, which maps the humanMachine's move to the
            // index on the Board
            positionMapper = GenerateMapping();
            Debug.WriteLine("Move made by the player is: {0}", temp);
            humanMachineDecision = humanMachine.GetStrategicMove(temp);

            if (humanMachineDecision == -1)
            {
                // Game is Over. Do not stop, since the 
                // result has to be drawn onto the screen
                markMachinePosition = false;
                gameOver = true;
            }
            else
            {
                markMachinePosition = true;
            }
            // Obtain key corresponding to the move made
            // this represents the position in the board to mark
            foreach (var key in positionMapper.Keys)
            {
                if (positionMapper[key] == humanMachineDecision)
                {
                    // Now, humanMachineDecision represents the index in the Board
                    // to mark the player.
                    humanMachineDecision = key;
                    ScreenManager.PlaySound("boardClick");
                     break;
                }
            }
            positionMapper = null;
            machineIsThinking = false;
            markPlayerPosition = false;
            displayPlayerChance = true;
        }
    }
}



