using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheTicTacToeGame.Screens
{
    public class ChooseCharacterScreen : MenuScreen
    {
        private MenuEntry choice;
        private MenuEntry next;
        private MenuEntry back;
        private bool isO = true;


       public ChooseCharacterScreen():base("Choose Your Character")
        {
            choice = new MenuEntry("");
            next = new MenuEntry("Next");
            back = new MenuEntry("Back");
            choice.entryClicked += ChangeChoice;
            next.entryClicked += OpenChooseDifficultyScreen;
            back.entryClicked += GoBackToStartScreen;
            InitText();
            menuEntries.Add(choice);
            menuEntries.Add(next);
            menuEntries.Add(back);
       }
        public void ChangeChoice(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
           isO = !isO;
           InitText();
        }
        public void OpenChooseDifficultyScreen(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {   
           // time to save the isO value to the ScreenManager
            // we could also obtain this value from ChooseCharacterScreen 
            // when removing the screens in the LoadingScreen  class' Update method.
            // However, doing so may couple the code.

            ScreenManager.PlayerCharacterIsO = isO; // true or false 
            
            ChooseDifficultyScreen chooseDifficulty = new ChooseDifficultyScreen();
            IsExiting = true;
            ScreenManager.AddScreen(chooseDifficulty);
            screenManager.Screens[ScreenManager.Screens.Count() - 1].LoadContent();
            
        }
        public void GoBackToStartScreen(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
            StartMenuScreen startScreen = new StartMenuScreen();
            IsExiting = true;
            ScreenManager.AddScreen(startScreen);
            screenManager.Screens[ScreenManager.Screens.Count() - 1].LoadContent();
        }

        public void InitText()
        {
            choice.title = "Choose: " + (isO ? "O" : "X");     
        }
    }
}
