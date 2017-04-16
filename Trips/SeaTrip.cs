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
    public class SeaTrip : Trip
    {
        [XmlElement("DistanceFromSea")]
        public double DistanceFromSea { get; set; }
        [XmlElement("BeachType")]
        public string BeachType { get; set; }

        public SeaTrip() { }

        public SeaTrip(string tripId_, string purpose_, string country_, string dateFrom_, int duration_, double price_, string transport_, string foodType_, double distance, string beachType_)
            : base(tripId_, purpose_, country_, dateFrom_, duration_, price_, transport_, foodType_)
        {
            DistanceFromSea = distance;
            BeachType = beachType_;
        }
        public SeaTrip(double distance, string beachType_, Trip trip)
            : base(trip)
        {
            DistanceFromSea = distance;
            BeachType = beachType_;
        }

        public override void showDescription()
        {
            base.showDescription();
            Console.WriteLine(" Distance from sea - {0}, \n Beach Type - {1}", DistanceFromSea, BeachType);
        }

        public override void writeToText(string fileName)
        {
            base.writeToText(fileName);
            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(";" + DistanceFromSea + ";" + BeachType);
            }
        }
    }
}
