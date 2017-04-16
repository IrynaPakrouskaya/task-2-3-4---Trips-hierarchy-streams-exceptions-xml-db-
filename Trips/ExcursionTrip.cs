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
    public class ExcursionTrip : Trip
    {
        [XmlElement("ProgramDesciption")]
        public string ProgramDesciption { get; set; }
        [XmlElement("AdditionalExcursion")]
        public string AdditionalExcursion { get; set; }
        [XmlElement("PriceForAdditionalExcursion")]
        public double PriceForAdditionalExcursion { get; set; }

        public ExcursionTrip() { }

        public ExcursionTrip(string tripId_, string purpose_, string country_, string dateFrom_, int duration_, double price_, string transport_, string foodType_, string description_, string additionalExcursion_, double additionalPrice)
            : base(tripId_, purpose_, country_, dateFrom_, duration_, price_, transport_, foodType_)
        {
            ProgramDesciption = description_;
            AdditionalExcursion = additionalExcursion_;
            PriceForAdditionalExcursion = additionalPrice;
        }
        public ExcursionTrip (string description_, string additionalExcursion_, double additionalPrice, Trip trip)
            : base(trip)
        {
            ProgramDesciption = description_;
            AdditionalExcursion = additionalExcursion_;
            PriceForAdditionalExcursion = additionalPrice;
        }

        public override void showDescription()
        {
            base.showDescription();
            Console.WriteLine(" Program - {0} \n Optional excursions which you may attend - {1} \n Price for additional excursions - {2}$", ProgramDesciption, AdditionalExcursion, PriceForAdditionalExcursion);
        }

        public override void writeToText(string fileName)
        {
            base.writeToText(fileName);
            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(";" + ProgramDesciption + ";" + AdditionalExcursion + ";" + PriceForAdditionalExcursion);
            }
        }
    }
}
