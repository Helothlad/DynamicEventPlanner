using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicEventPlanner
{
    static class MainMenu
    {
        public static void DisplayMenu()
        {
            Console.WriteLine("Main menu:");
            Console.WriteLine("1: Event manager");
            Console.WriteLine("2: Simulate events");
            Console.WriteLine("3: Reports");
            Console.WriteLine("4: Exit program");

            if (int.TryParse(Console.ReadLine(), out int input) && (input == 1 || input == 2 || input == 3 || input == 4))
            {
                InputHandler(input);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number");
                DisplayMenu();
            }
        }
        private static void InputHandler(int input)
        {
            switch (input)
            {
                case 1:
                    Console.Clear();
                    EventController eventController = new EventController();
                    eventController.DisplayOptions();
                    break;
                case 2:
                    Console.WriteLine("Simulate");
                    //Simulate();
                    break;
                case 3:
                    Console.WriteLine("Reports");
                    //Reports();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("You shouldn't be seeing this xd");
                    break;
            }
        }
    }
}
