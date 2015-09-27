using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheTicTacToeGame.Game_Objects;


namespace TheTicTacToeGame.Screens
{
    public class ChooseDifficultyScreen : MenuScreen
    {
        private MenuEntry easy;
        private MenuEntry hard;
        private MenuEntry back;

        public ChooseDifficultyScreen() : base("Choose Difficulty Level!!")
        {
            easy = new MenuEntry("1.    Easy???");
            hard = new MenuEntry("2.    Hard???");
            back = new MenuEntry("Back");
            easy.entryClicked += OnEasyClicked;
            hard.entryClicked += OnHardClicked;
            back.entryClicked += OnBackClicked;
            menuEntries.Add(easy);
            menuEntries.Add(hard);
            menuEntries.Add(back);
        }
        private void OnEasyClicked(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
            // find the TicTacToeScreen
            HumanMachine.gamePlayLevel = 1;
            StartGame();
        }

        private void OnHardClicked(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
            HumanMachine.gamePlayLevel = 2;
            StartGame();
        }
        private void OnBackClicked(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
            IsExiting = true;
            ScreenManager.AddScreen(new ChooseCharacterScreen());
            screenManager.Screens[ScreenManager.Screens.Count() - 1].LoadContent();
        }

        private void StartGame()
        {
            TicTacToeScreen theAwesomeGame = new TicTacToeScreen();
        //    ScreenManager.AddScreen(theAwesomeGame);
        //    ScreenManager.Screens[ScreenManager.Screens.Count() - 1].LoadContent();
            IsExiting = true;
            LoadingScreen loadingScreen = new LoadingScreen(new GameScreen[] { theAwesomeGame }, true, ScreenManager);

        }

    }
}
