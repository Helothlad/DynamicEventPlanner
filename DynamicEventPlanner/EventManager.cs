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
        private EventDbContext db;
        public event EventHandler EventAdded;
        public event EventHandler EventRemoved;
        public event EventHandler EventEdited;
        public event EventHandler EventDelited;
        public event EventFunctionComplete EventDeleteCompleted;
        public event EventFunctionComplete DisplayEventsCompleted;
        public event EventFunctionComplete AddEventCompleted;
        public event EventFunctionComplete EventUpdated;
        public EventManager()
        {
            Events = new List<Event>();
            db = new EventDbContext();
        }
        public void AddEvent(Event newEvent)
        {
            
            db.Events.Add(newEvent);
            db.SaveChanges();
            
            EventAdded?.Invoke(newEvent.Name + " event has been added");
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
            Console.Clear();
            AddEventCompleted?.Invoke();
        }
        
        public void displayEvents()
        {
            Console.Clear();
            Console.WriteLine("All events: ");
            Console.WriteLine("--------------------------------------------");
            
            
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
                
            
            Console.Clear();
            DisplayEventsCompleted?.Invoke();
        }
        public void UpdateEvent(string eventName, string propertyName, string newValue)
        {
            
            var eventToEdit = db.Events.FirstOrDefault(t =>  t.Name == eventName);
                
            if (eventToEdit == null)
            {
                Console.WriteLine("Event not found in database.");
                return;
            }
            var property = eventToEdit.GetType().GetProperty(propertyName);
            if (property == null || !property.CanWrite) 
            {
                Console.WriteLine("Property cannot be edited or does not exist.");
                return;
            }
            try
            {
                object convertValue = Convert.ChangeType(newValue, property.PropertyType);
                property.SetValue(eventToEdit, convertValue);
                db.SaveChanges();
                EventEdited?.Invoke("Event updated: " + eventName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update {propertyName}: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
            Console.Clear();
            EventUpdated?.Invoke();
            
        }
        public void DeleteEvent(Event eventToDel)
        {
            var actualEvent = db.Events.FirstOrDefault(t => t.Equals(eventToDel));
            db.Events.Remove(actualEvent);
            db.SaveChanges();
            EventDelited?.Invoke("Event deleted: " + eventToDel.Name);

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
            Console.Clear();

            EventDeleteCompleted?.Invoke();
        }
        
    }
}
