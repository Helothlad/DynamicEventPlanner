using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace DynamicEventPlanner
{
    public class EventController
    {
        private EventManager eventManager;
        private EventDbContext db;
        public EventController()
        {
            eventManager = new EventManager();
            db = new EventDbContext();
            eventManager.DisplayEventsCompleted += DisplayOptions;
            eventManager.AddEventCompleted += DisplayOptions;
            eventManager.EventDeleteCompleted += DisplayOptions;
            eventManager.EventUpdated += DisplayOptions;

            eventManager.EventDelited += DisplayMess;
            eventManager.EventAdded += DisplayMess;
            eventManager.EventEdited += DisplayMess;
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
                    Console.Clear();
                    EditEventWithCatch();
                    break;
                case 4:
                    DeleteEventWithCatch();
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

            Console.WriteLine("Enter 0 to return to the menu ");
            Console.Write("Event name: ");
            string name = Console.ReadLine();

            if (name == "0")
            {
                Console.Clear();
                DisplayOptions();
                return;
            }

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
                if (int.TryParse(Console.ReadLine(), out energyConsumption) && energyConsumption >= 0 && energyConsumption <= 100)
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

            eventManager.AddEvent(newEvent);

            
        }
        private void DisplayExistingEvents()
        {
            eventManager.displayEvents();
        }
        public void EditEventWithCatch()
        {
            try
            {
                EditEvent();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
            }
        }
        public void DeleteEventWithCatch()
        {
            try
            {
                DeleteEvent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private void EditEvent()
        {
            
            Console.WriteLine("Enter 0 to return to the menu ");
            Console.Write("Name of event to edit: ");
            string name = Console.ReadLine();
            
            
            if(name == "0")
            {
                Console.Clear();
                DisplayOptions();
                return;
            }
            
            var eventToFind = db.Events.FirstOrDefault(t => t.Name == name);

            if (eventToFind == null)
            {
                Console.WriteLine("Event not found.");
                EditEventWithCatch();
                return;
            }

                
            if (eventToFind.Date < DateTime.Now)
            {
                Console.WriteLine("You cannot edit past events.");
                EditEventWithCatch();
                return;
            }

            int counter = 1;
            Console.WriteLine("Choose the property to edit: ");
            var properties = eventToFind.GetType().GetProperties();
            foreach (var prop in properties)
            {
                Console.WriteLine($"{counter}: {prop.Name} - {prop.GetValue(eventToFind)}");
                counter++;
            }

            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= counter)
            {
                var propertyToEdit = properties[choice - 1].Name;
                Console.Write($"Enter new value for {propertyToEdit}: ");
                string newValue = Console.ReadLine();
                if(propertyToEdit == "EnergyConsumption" && (int.Parse(newValue) < 0 || int.Parse(newValue) > 100))
                {
                    throw new Exception("The energy consumption can only be between 0 and 100");
                }
                try
                {
                    eventManager.UpdateEvent(name, properties[choice - 1].Name, newValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
            
        }
        private void DeleteEvent()
        {
            Console.WriteLine("Enter 0 to return to the menu ");
            Console.Write("Name of event to delete: ");
            string name = Console.ReadLine();
            if (name == "0")
            {
                Console.Clear();
                DisplayOptions();
                return;
            }
            var eventToDel = db.Events.FirstOrDefault(t => t.Name == name);

            if (eventToDel == null)
            {
                Console.WriteLine("Event not found.");
                DeleteEventWithCatch();
                return;
            }
            try
            {
                eventManager.DeleteEvent(eventToDel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            } 
        }
        private void BackToMenu()
        {
            Console.Clear();
            MainMenu.DisplayMenu();
        }
        private void DisplayMess(string messege)
        {
            Console.WriteLine(messege);
        }
        

    }
}
