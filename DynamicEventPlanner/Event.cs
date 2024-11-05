using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicEventPlanner
{
    //Haliey nem szeret korán kelni, unja a félórásnál hosszabb eseményeket, és frusztrálják a nagy konferencia termek.
    public enum EventType
    {
        TvAppearance,
        Podcast,
        Roundtable,
        Lecture,
        Concert
    }
    public class Event
    {
        public Event(string name, double honorarium, int energyConsumption, DateTime date, TimeSpan duration, EventType typeOfEvent)
        {
            Date = date;
            Duration = duration;
            Name = name;
            Honorarium = honorarium;
            EnergyConsumption = energyConsumption;
            Status = false;
            TypeOfEvent = typeOfEvent;
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Honorarium { get; set; }
        [Required]
        public int EnergyConsumption { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public EventType TypeOfEvent { get; set; }

    }
}
