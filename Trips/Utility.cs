using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trips
{
    public static class Utility
    {
        public static List<Trip> filterByPurpose(string purpose, List<Trip> trips)
        {
            List<Trip> filteredList = trips.FindAll(delegate(Trip trip) { return trip.Purpose.Equals(purpose); });
            return filteredList;
        }
        public static List<Trip> filterByTransport(string transport, List<Trip> trips)
        {
            List<Trip> filteredList = trips.FindAll(delegate(Trip trip) { return trip.Transport.Equals(transport); });
            return filteredList;
        }
        public static List<Trip> filterByFoodType(string foodType, List<Trip> trips)
        {
            List<Trip> filteredList = trips.FindAll(delegate(Trip trip) { return trip.FoodType.Equals(foodType); });
            return filteredList;
        }
        public static List<Trip> filterByDuration(int duration, List<Trip> trips)
        {
            List<Trip> filteredList = trips.FindAll(delegate(Trip trip) { return trip.Duration.Equals(duration); });
            return filteredList;
        }

        private static int compareByPrice(Trip a, Trip b)
        {
            if (a.Price == b.Price)
                return 0;
            else
            {
                if (a.Price > b.Price)
                    return 1;
                else
                    return -1;
            }
        }
        public static void sortingByPrice(List<Trip> trips)
        {
            trips.Sort(compareByPrice);
        }

        public static void displayOrders(List<Trip> trips)
        {
            for (int i = 0; i < trips.Count; i++)
            {
                trips[i].showDescription();
            }
        }

        public static void writeToText(string fileName, List<Trip> trips)
        {
            for(int i = 0; i < trips.Count; i++)
            {
                trips[i].writeToText(fileName);
            }
        }
        public static ArrayList readFromText(string fileName)
        {
            ArrayList tripsArray = new ArrayList();
            using (StreamReader sr = new StreamReader(fileName))
            {
                string input = null;
                while ((input = sr.ReadLine()) != null)
                {
                    tripsArray.Add(input);
                }
            }
            return tripsArray;
        }
        public static Trip stringParser (string stringToParse)
        {            
            char delimiterChar = ';';
            string[] fields = stringToParse.Split(delimiterChar);
            Trip parsedTrip = new Trip();
            try
            {
                parsedTrip = new Trip(fields[1], fields[0], fields[2], fields[3], Convert.ToInt32(fields[4]), Convert.ToDouble(fields[5]), fields[6], fields[7]);
                if (fields[0] == "sea")
                {
                    parsedTrip = new SeaTrip(Convert.ToDouble(fields[8]), fields[9], parsedTrip);
                }
                else if (fields[0] == "excursion")
                {
                    parsedTrip = new ExcursionTrip(fields[8], fields[9], Convert.ToDouble(fields[10]), parsedTrip);
                }
                else if (fields[0] == "health")
                {
                    parsedTrip = new HealthTrip(fields[8], parsedTrip);
                }
                else if (fields[0] == "shopping")
                {
                    parsedTrip = new ShoppingTrip(fields[8], parsedTrip);
                }
                else
                {
                    throw new TripPurposeIsInvalidException(string.Format("Trip record has invalid purpose - {0}.", fields[0]));
                }
            }               
            catch(IndexOutOfRangeException e)
            {                
                Console.WriteLine("Trip record has incorrect number of fields." + "\nException details:" + "\n" + e.Message + "\nObject = " + e.Source + "\nMethod - " + e.TargetSite + "\nCall stack - " + e.StackTrace);
                throw new TripRecordIsNotReadException(string.Format("Invalid trip record - {0}", stringToParse));
            }
            catch(TripPurposeIsInvalidException e)
            {
                Console.WriteLine("Trip record can't be recognized." + "\nException details:" + "\n" + e.Message + "\nObject = " + e.Source + "\nMethod - " + e.TargetSite + "\nCall stack - " + e.StackTrace);
                throw new TripRecordIsNotReadException(string.Format("Invalid trip record - {0}", stringToParse));
            }
            catch(FormatException e)
            {
                Console.WriteLine("Trip record contains field(s) with incorrect format" + "\nException details:" + "\n" + e.Message + "\nObject = " + e.Source + "\nMethod - " + e.TargetSite + "\nCall stack - " + e.StackTrace);
                throw new TripRecordIsNotReadException(string.Format("Invalid trip record - {0}", stringToParse));
            }
            return parsedTrip;
        }

        public static void writeToBinary(string fileName, List<Trip> trips)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            {
                formatter.Serialize(fs, trips);
            }
        }
        public static List<Trip> readFromBinary(string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            List<Trip> trips = new List<Trip>();
            using(FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                try
                {
                    trips = (List<Trip>)formatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Binary file is corrupted." + "\nException details:" + "\n" + e.Message + "\nObject = " + e.Source + "\nMethod - " + e.TargetSite + "\nCall stack - " + e.StackTrace);                    
                }
            }
            return trips;
        }

        public static void SerializeToXml(string fileName, List<Trip> trips)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Trip>), new Type[] {typeof(ExcursionTrip), typeof(HealthTrip), typeof(ShoppingTrip), typeof(SeaTrip)});
            TextWriter textWriter = new StreamWriter(fileName);
            serializer.Serialize(textWriter, trips);
            textWriter.Close();
        }
        public static List<Trip> DeserializeFromXml(string filename)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Trip>), new Type[] { typeof(ExcursionTrip), typeof(HealthTrip), typeof(ShoppingTrip), typeof(SeaTrip) });
            TextReader textReader = new StreamReader(filename);
            List<Trip> trips = new List<Trip>();
            trips = (List<Trip>)deserializer.Deserialize(textReader);
            textReader.Close();
            return trips;
        }
    }
}
