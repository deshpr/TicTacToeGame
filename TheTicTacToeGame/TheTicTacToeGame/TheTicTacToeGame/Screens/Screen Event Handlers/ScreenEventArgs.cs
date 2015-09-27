using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using  Microsoft.Xna.Framework.Input;


namespace TheTicTacToeGame.Screens.Screen_Event_Handlers
{
    public class ScreenEventArgs : EventArgs
    {
        private Keys keyPressed;

        public Keys KeyPressed
        {
            get { return keyPressed; }
            set { keyPressed = value; }
        }
        public ScreenEventArgs(String menuTitle, Keys keyPressed = Keys.None)
        {
            this.keyPressed = keyPressed;
        }
    }
}
