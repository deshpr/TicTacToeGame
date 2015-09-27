using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace TheTicTacToeGame.Screens
{
    public class SettingsMenuScreen : MenuScreen
    {
        private int songIndex;
        private String[] songs = new String[] { "All In One Ear", "Back Against the Wall" };
        private String playSoundText;
        private String playBackSongText;
      
        private MenuEntry playBackMusic;
        private MenuEntry back;
        private MenuEntry playSound;
        private MenuEntry volumeSelection;
        private float volume;

        public SettingsMenuScreen(ScreenManager screenManager) : base("Settings")
        {
            this.ScreenManager = screenManager;
      //      playBackMusic = new MenuEntry("");
            back = new MenuEntry("Back");
            playSound = new MenuEntry("");

            back.entryClicked += BackClicked;
      //      playBackMusic.entryClicked += SongClicked;
            playSound.entryClicked += PlaySound;
            volumeSelection = new MenuEntry("");
            volumeSelection.entryClicked += ChangeVolume;
            volume = 80f;
            // initialize the texts of all the menu items
            InitializeText();
     //       menuEntries.Add(playBackMusic);
            menuEntries.Add(playSound);
            menuEntries.Add(volumeSelection);
            menuEntries.Add(back);
        }

        public void InitializeText()
        {
    //        playBackMusic.title = "Song To Play: " + songs[songIndex];
            if(ScreenManager!=null)
            playSound.title = "Play Sound? " + ScreenManager.playSound;
            else
                playSound.title = "Play Sound? " + true;
            volumeSelection.title = "Volume:  < " + volume + " > ";
            
        }
        
        public void BackClicked(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
            IsExiting = true;
        }
        /*
        public void SongClicked(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
            songIndex = (++songIndex) % 2;
            this.InitializeText();
        }
        */
        public void PlaySound(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
            ScreenManager.playSound = !ScreenManager.playSound;
            this.InitializeText();
        }
       
        public void ChangeVolume(object sender, Screen_Event_Handlers.ScreenEventArgs e)
        {
            Keys key = e.KeyPressed;
            if(key == Keys.Left)
            {
                volume--;
            }
            else
            {
                volume++;
            }
            ScreenManager.audioCategory.SetVolume( volume);
            this.InitializeText();
        }
    }
}
