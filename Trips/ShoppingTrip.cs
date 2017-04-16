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
    public class ShoppingTrip : Trip
    {
        [XmlElement("ShopСenters")]
        public string ShopСenters { get; set; }

        public ShoppingTrip() { }

        public ShoppingTrip(string tripId_, string purpose_, string country_, string dateFrom_, int duration_, double price_, string transport_, string foodType_, string shopCenters_)
            : base(tripId_, purpose_, country_, dateFrom_, duration_, price_, transport_, foodType_)
        {
            ShopСenters = shopCenters_;
        }
        public ShoppingTrip(string shopCenters_, Trip trip)
            : base(trip)
        {
            ShopСenters = shopCenters_;
        }

        public override void showDescription()
        {
            base.showDescription();
            Console.WriteLine(" Shop centers which you will attend - {0}", ShopСenters);
        }

        public override void writeToText(string fileName)
        {
            base.writeToText(fileName);
            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(";" + ShopСenters);
            }
        }
    }
}
