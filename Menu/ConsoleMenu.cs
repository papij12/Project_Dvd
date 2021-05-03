using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Dvd.Menu
{
    class ConsoleMenu
    {
        private string Description { get; set; }
        private ConsoleMenuElement[] MenuElements { get; set; }
        public int SelectedElement { get; private set; }
        public ConsoleMenu(string description, ConsoleMenuElement[] menuElements)
        {
            SelectedElement = 0;
            Description = description;
            MenuElements = menuElements;
        }
        public void PrintMenu()
        {
            Console.WriteLine(Description);
            for (int i = 0; i < MenuElements.Length; i++)
            {
                if (i == SelectedElement)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(MenuElements[i]);
                Console.ResetColor();
            }
        }
        public void RunMenu()
        {
            while (true)
            {
                PrintMenu();
                ConsoleKeyInfo pressedKey = Console.ReadKey();
                switch (pressedKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        SelectedElement = (SelectedElement - 1 + MenuElements.Length) % MenuElements.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        SelectedElement = (SelectedElement + 1) % MenuElements.Length;
                        break;
                    case ConsoleKey.Enter:
                        MenuElements[SelectedElement].ActionToRun();
                        break;
                }
                //Console.Clear();
                Console.SetCursorPosition(0, 0);
            }
        }
    }
}