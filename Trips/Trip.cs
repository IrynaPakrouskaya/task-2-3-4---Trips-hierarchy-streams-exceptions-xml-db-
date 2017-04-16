using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trips
{
    [Serializable]
    public class Trip
    {
        [XmlElement ("TripId")]
        public string TripId { get; set; }
        [XmlElement ("Purpose")]
        public string Purpose { get; set; }
        [XmlElement ("Country")]
        public string Country { get; set; }
        [XmlElement ("DateFrom")]
        public string DateFrom { get; set; }
        [XmlElement ("Duration")]
        public int Duration { get; set;}
        [XmlElement ("Price")]
        public double Price { get; set; }
        [XmlElement ("Transport")]
        public string Transport { get; set; }
        [XmlElement ("FoodTYpe")]   
        public string FoodType { get; set; }   
               
        public Trip(string tripId_, string purpose_, string country_, string dateFrom_, int duration_, double price_, string transport_, string foodType_)
        {
            TripId = tripId_;
            Purpose = purpose_;
            Country = country_;
            DateFrom = dateFrom_;
            Duration = duration_;
            Price = price_;
            Transport = transport_;
            FoodType = foodType_;
        }
        public Trip(Trip trip_)
        {
            TripId = trip_.TripId;
            Purpose = trip_.Purpose;
            Country = trip_.Country;
            DateFrom = trip_.DateFrom;
            Duration = trip_.Duration;
            Price = trip_.Price;
            Transport = trip_.Transport;
            FoodType = trip_.FoodType;
        }
        public Trip() { }


        public virtual void showDescription()
        {
            Console.WriteLine("Trip details: \n Country - {0}, \n Starte Date - {1}, \n Duration - {2} days, \n Price - {3}$, \n Transport - {4}, \n Food Type - {5}", Country, DateFrom, Duration, Price, Transport, FoodType);
        }

        public virtual void writeToText(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))                
                {
                    sw.Write(Purpose + ";" + TripId + ";" + Country + ";" + DateFrom + ";" + Duration + ";" +  Price + ";" + Transport + ";" + FoodType);
                }
        }
    }
    }
