using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicEventPlanner
{
    public delegate void EventHandler(string messege);
    class EventManager
    {
        public List<Event> Events;
        public event EventHandler EventAdded;
        public event EventHandler EventRemoved;
        public event EventHandler EventEdited;
        public EventManager()
        {
            Events = new List<Event>();
            
        }
        public void AddEvent(Event newEvent)
        {
            using(var context = new EventDbContext())
            {
                context.Events.Add(newEvent);
                context.SaveChanges();
            }
            Events.Add(newEvent);
            EventAdded?.Invoke(newEvent.Name + " event has been added");
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
        
    }
}
