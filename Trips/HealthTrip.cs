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
    public class HealthTrip : Trip
    {
        [XmlElement("MedicalProcedures")]
        public string MedicalProcedures { get; set; }

        public HealthTrip() { }

        public HealthTrip(string tripId_, string purpose_, string country_, string dateFrom_, int duration_, double price_, string transport_, string foodType_, string medicalProcedures_)
            : base(tripId_, purpose_, country_, dateFrom_, duration_, price_, transport_, foodType_)
        {
            MedicalProcedures = medicalProcedures_;
        }
        public HealthTrip(string medicalProcedures_, Trip trip)
            : base(trip)
        {
            MedicalProcedures = medicalProcedures_;
        }

        public override void showDescription()
        {
            base.showDescription();
            Console.WriteLine(" List of available medical procedures - {0}", MedicalProcedures);
        }

        public override void writeToText(string fileName)
        {
            base.writeToText(fileName);
            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(";" + MedicalProcedures);
            }
        }
    }
}
