using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Dvd.Menu
{
    class ConsoleMenuElement
    {
        public string Label { get; set; }
        public Action ActionToRun { get; set; }
        public ConsoleMenuElement(string label, Action actionToRun)
        {
            Label = label;
            ActionToRun = actionToRun;
        }
        public override string ToString()
        {
            return Label;
        }
    }
}
