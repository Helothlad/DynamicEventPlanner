using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicEventPlanner
{
    public delegate void EventHandler(string messege);
    public delegate void EventFunctionComplete();
    class EventManager
    {
        public List<Event> Events;

        public event EventHandler EventAdded;
        public event EventHandler EventRemoved;
        public event EventHandler EventEdited;
        public event EventFunctionComplete DisplayEventsCompleted;
        public event EventFunctionComplete AddEventCompleted;
        public EventManager()
        {
            Events = new List<Event>();
            
        }
        public void AddEvent(Event newEvent)
        {
            using (var context = new EventDbContext())
            {
                context.Events.Add(newEvent);
                context.SaveChanges();
            }
            
            EventAdded?.Invoke(newEvent.Name + " event has been added");
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
            Console.Clear();
            AddEventCompleted?.Invoke();
        }
        public void EditEvent(Event newEvent)
        {
            if(DateTime.Now > newEvent.Date)
            {
                throw new Exception("You cannot edit past events");
            }
            var desiredEvent = Events.FirstOrDefault(t => t.Name == newEvent.Name && t.Date == newEvent.Date);
            
            if(desiredEvent != null)
            {
                desiredEvent.Date = newEvent.Date;
                desiredEvent.Duration = newEvent.Duration;
                desiredEvent.Name = newEvent.Name;
                desiredEvent.Honorarium = newEvent.Honorarium;
                desiredEvent.EnergyConsumption = newEvent.EnergyConsumption;
                desiredEvent.Status = newEvent.Status;
                desiredEvent.TypeOfEvent = newEvent.TypeOfEvent;
                
                EventEdited?.Invoke(desiredEvent.Name + " event has been updated");
            }
            else
            {
                throw new Exception("There was no such event");
            }
        }
        public void displayEvents()
        {
            Console.Clear();
            Console.WriteLine("All events: ");
            Console.WriteLine("--------------------------------------------");
            using (EventDbContext db = new EventDbContext())
            {
                var events = db.Events.OrderBy(t => t.Date).ToList();

                if (events.Count == 0)
                {
                    Console.WriteLine("No events found.");
                }
                else
                {
                    foreach (var ev in events)
                    {
                        foreach (var prop in ev.GetType().GetProperties())
                        {
                            string propName = prop.Name;
                            var propValue = prop.GetValue(ev, null);
                            Console.WriteLine($"{propName}: {propValue}");
                        }
                        Console.WriteLine("--------------------------------------------");
                    }
                }

                // Pause after displaying events to give the user a chance to review them
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
                
            }
            Console.Clear();
            DisplayEventsCompleted?.Invoke();
            

        }
        
    }
}
