using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheTicTacToeGame.Screens.Screen_Event_Handlers;

namespace TheTicTacToeGame.Screens
{
    public class StartMenuScreen : MenuScreen
    {

        public StartMenuScreen() : base("Main Menu")
        {
            MenuEntry startGame = new MenuEntry("Start Game!");
            MenuEntry settings = new MenuEntry("Settings");
            MenuEntry quit = new MenuEntry("Quit");
            startGame.entryClicked += StartGame;
            settings.entryClicked += OpenSettingsMenuScreen;
            quit.entryClicked += OnQuit;
            menuEntries.Add(startGame);
            menuEntries.Add(settings);
            menuEntries.Add(quit);
         }
      public void OpenSettingsMenuScreen(object sender, ScreenEventArgs args)
        {
            SettingsMenuScreen settings = new SettingsMenuScreen(ScreenManager);
            settings.ScreenManager = ScreenManager;
            screenManager.AddScreen(settings); 
        }

        public void StartGame(object sender, ScreenEventArgs args)
        {

          ChooseCharacterScreen chooseCharacterScreen = new ChooseCharacterScreen();
         IsExiting = true;
          ScreenManager.AddScreen(chooseCharacterScreen);
          ScreenManager.Screens[ScreenManager.Screens.Count() - 1].LoadContent();
         // LoadingScreen loadingScreen = new LoadingScreen(new GameScreen[] { new ChooseDifficulty() }, true, ScreenManager);
     }
        public void OnQuit(object sender, ScreenEventArgs e)
        {
       //     IsExiting = true;

            // do not remove the current screen
            
            ExitScreen exitScreen = new ExitScreen();
            exitScreen.ScreenManager = ScreenManager;
            exitScreen.LoadContent();
            ScreenManager.AddScreen(exitScreen);
            ScreenManager.Screens[ScreenManager.Screens.Count() - 1].LoadContent();
 //           screenstate = ScreenState.Active;
    
        }
    }
}
