using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoItX3Lib;
using AutoIt;

namespace IanAutomation.Apps
{
    public class Notepad
    {
        public Notepad() {
            // Start AutoItX
            //AutoItX3.AutoItSetOption("WinTitleMatchMode", 2); // Set the title matching mode to match any substring

            // Open Notepad
            AutoItX.Run("notepad.exe", "", AutoItX.SW_SHOW);

            // Wait for Notepad to open
            AutoItX.WinWait("[CLASS:Notepad]", "", 10);

            AutoItX.Send("I'm in notepad");

            // Close Notepad
            AutoItX.WinClose("[CLASS:Notepad]");
        }
    }
}
