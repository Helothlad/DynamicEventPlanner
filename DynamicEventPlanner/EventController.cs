using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace DynamicEventPlanner
{
    public class EventController
    {
        private EventManager eventManager;
        public EventController()
        {
            eventManager = new EventManager();
            eventManager.DisplayEventsCompleted += DisplayOptions;
            eventManager.AddEventCompleted += DisplayOptions;

            DisplayOptions();
        }
        public void DisplayOptions()
        {
            Console.WriteLine("Event handler");
            Console.WriteLine("1: Add new event");
            Console.WriteLine("2: Display existing events");
            Console.WriteLine("3: Edit event");
            Console.WriteLine("4: Delete event");
            Console.WriteLine("5: Back to menu");
            
            if(int.TryParse(Console.ReadLine(), out int input) && (input == 1 || input == 2 || input == 3 || input == 4 || input == 5))
            {
                InputHandler(input);
                return;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number");
                DisplayOptions();
            }

        }
        private void InputHandler(int input)
        {
            switch (input)
            {
                case 1:
                    AddNewEvent();
                    break;
                case 2:
                    DisplayExistingEvents();
                    break;
                case 3:
                    EditEvent();
                    break;
                case 4:
                    DeleteEvent();
                    break;
                case 5:
                    BackToMenu();
                    break;
                default:
                    Console.WriteLine("You shouldn't be seeing this xd");
                    break;
            }
        }
        private void AddNewEvent()
        {
            Console.Clear();
            //string name, double honorarium, int energyConsumption, DateTime date, TimeSpan duration
            Console.Write("Event name: ");
            string name = Console.ReadLine();


            double honorarium;
            while (true)
            {
                Console.Write("Honorarium ($): ");
                if(double.TryParse(Console.ReadLine(), out honorarium) && honorarium >= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a valid positive number for honorarium.");
                }
            }

            
            int energyConsumption;
            while (true)
            {
                Console.Write("Energy consumption: ");
                if (int.TryParse(Console.ReadLine(), out energyConsumption) && energyConsumption >= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a valid positive number for energy consumption.");
                }
            }

            
            DateTime date;
            while (true)
            {
                Console.WriteLine("Date (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out date))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a valid date in the format yyyy-MM-dd.");
                }
            }

            
            TimeSpan duration;
            while (true)
            {
                Console.WriteLine("Duration (hh:mm): ");
                string durationInput = Console.ReadLine();
                if (TimeSpan.TryParse(durationInput, out duration))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid duration format. Please enter in hh:mm format.");
                }
            }
            EventType type;
            while (true)
            {
                Console.WriteLine("Event type (TvAppearance, Podcast, Roundtable, Lecture, Concert): ");
                string typeInput = Console.ReadLine();
                if (Enum.TryParse(typeInput, true, out type))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid event type. Please enter a valid type from the list.");
                }
            }
            Event newEvent = new Event(name, honorarium, energyConsumption, date, duration, type);
            eventManager.EventAdded += EventAddedDisplay;

            eventManager.AddEvent(newEvent);

            
        }
        private void DisplayExistingEvents()
        {
            eventManager.displayEvents();
        }
        private void EditEvent()
        {
            
            Console.Clear();
            Console.Write("Name of event to edit: ");
            EventDbContext db = new EventDbContext();
            string name = Console.ReadLine();
            var eventToFind = db.Events.FirstOrDefault(t => t.Name == name);

            if (name != null && db.Events.ToList().Contains(eventToFind))
            {
                
            }
            

            /*Event ev = new Event("test", 12, 12, new DateTime(203), new TimeSpan(13), EventType.Concert);
            int counter = 1;
            foreach (var prop in ev.GetType().GetProperties())
            {
                Console.WriteLine("1: " + ev.Name);
            }*/
            
        }
        private void DeleteEvent()
        {
            Console.WriteLine("Delete: ");
        }
        private void BackToMenu()
        {
            Console.Clear();
            MainMenu.DisplayMenu();
        }
        private void EventAddedDisplay(string messege)
        {
            Console.WriteLine(messege);
        }
        

    }
}
