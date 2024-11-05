using System;
using System.Collections.Generic;
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
            Events.Add(newEvent);
        }
        public void EditEvent(Event newEvent)
        {
            if(DateTime.Now > newEvent.Date)
            {
                throw new Exception("The event has already passed");
            }
            var desiredEvent = Events.FirstOrDefault(t => t.Name == newEvent.Name && t.Date == newEvent.Date);
            
            if(desiredEvent != null)
            {
                desiredEvent.Date = newEvent.Date;
                desiredEvent.Duration = newEvent.Duration;
                desiredEvent.Name = newEvent.Name;
                desiredEvent.Honorarium = newEvent.Honorarium;
                desiredEvent.EnergyConsumptions = newEvent.EnergyConsumptions;
                desiredEvent.Status = newEvent.Status;
                desiredEvent.Type = newEvent.Type;
                
                EventEdited?.Invoke(desiredEvent.Name + " event has been updated");
            }
        }
    }
}
