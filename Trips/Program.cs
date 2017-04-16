using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trips
{
    class Program
    {       
        static void Main(string[] args)
        {
            string purposeParameter;
            string transportParameter;
            string foodTypeParameter;
            int durationParameter;
            List<Trip> catalog = new List<Trip>();
            List<Trip> bufferTrips = new List<Trip>(); 
            string fileNameText = "TripsCatalog.txt";
            string fileNameBin = "TripsCatalog.dat";
            string fileNameXml = "TripsCatalog.xml";
            List<Trip> tripsFromTextFile = new List<Trip>();
            List<Trip> tripsFromBinFile = new List<Trip>();
            List<Trip> tripsFromXmlFile = new List<Trip>(); 

            //create trips
            SeaTrip sea1 = new SeaTrip("1", "sea", "Italy", "04-22-2009", 10, 250.4, "plane", "AI", 200.34, "private");
            ExcursionTrip excurs1 = new ExcursionTrip("2", "excursion", "Czech Republic", "06-12-2017", 7, 76.5, "bus", "BB", "Explore the historic UNESCO World Heritage Site of Kutná Hora on a tour from Prague. See the late Gothic St. Barbara's Church and stroll around the town center.", "Visit the chapel of human bones at the Roman Catholic Cemetery Church of All Saints.", 25.7);
            ShoppingTrip shop1 = new ShoppingTrip("3", "shopping", "Poland", "05-28-2017", 2, 30.6, "bus", "BC", "Zlote Tarasy, Arkadia Shopping Mall, Hala Koszyki");
            HealthTrip health1 = new HealthTrip("4", "health", "Israel", "05-10-2017", 7, 315.75, "train", "BB", "Ьassage, Inhalation, Mineral Baths");

            //add trips to catalog
            catalog.Add(sea1);
            catalog.Add(excurs1);
            catalog.Add(shop1);
            catalog.Add(health1);
            
            //ask user about filter parameters 
            do
            {
                Console.Write("Which purpose of your trip? (sea, excursion, shopping, health) : ");
                try
                {
                    purposeParameter = Console.ReadLine();
                    if (!purposeParameter.ToLower().Equals("sea") && !purposeParameter.ToLower().Equals("excursion") && !purposeParameter.ToLower().Equals("shopping") && !purposeParameter.ToLower().Equals("health"))
                    {
                        throw new InvalidUserInputException("Invalid purpose. Try again.");
                    }
                }
                catch (InvalidUserInputException e)
                {
                    Console.WriteLine(e.Message);
                    purposeParameter = null;
                }
            }
            while (purposeParameter == null);
            do
            {
                Console.Write("Which transport you prefer? (plain, bus, train) : ");
                try
                {
                    transportParameter = Console.ReadLine();
                    if (!transportParameter.ToLower().Equals("plain") && !transportParameter.ToLower().Equals("bus") && !transportParameter.ToLower().Equals("train"))
                    {
                        throw new InvalidUserInputException("Invalid transport. Try again.");
                    }
                }
                catch (InvalidUserInputException e)
                {
                    Console.WriteLine(e.Message);
                    transportParameter = null;
                }
            }
            while (transportParameter == null);
            do
            {
                Console.Write("Which food type you prefer? (AI, BC, BB) : ");
                try
                {
                    foodTypeParameter = Console.ReadLine();
                    if (!foodTypeParameter.ToUpper().Equals("AI") && !foodTypeParameter.ToUpper().Equals("BC") && !foodTypeParameter.ToUpper().Equals("BB"))
                    {
                        throw new InvalidUserInputException("Invalid food type. Try again.");
                    }
                }
                catch (InvalidUserInputException e)
                {
                    Console.WriteLine(e.Message);
                    foodTypeParameter = null;
                }
            }
            while (foodTypeParameter == null);
            do
            {
                Console.Write("Enter trip duration : ");
                try
                {
                    durationParameter = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException e)
                {
                    e = new FormatException("Duration should be integer value.");
                    Console.WriteLine(e.Message);
                    durationParameter = 0;
                }
            }
            while (durationParameter == 0);        
                        
            
            //filter the records
            bufferTrips =  catalog;
            bufferTrips = Utility.filterByPurpose(purposeParameter, bufferTrips);
            bufferTrips = Utility.filterByTransport(transportParameter, bufferTrips);
            bufferTrips = Utility.filterByFoodType(foodTypeParameter, bufferTrips);
            bufferTrips = Utility.filterByDuration(durationParameter, bufferTrips);

            Utility.sortingByPrice(bufferTrips);
            Utility.displayOrders(bufferTrips);

            //read & write from txt file
            Utility.writeToText(fileNameText, catalog);
            ArrayList tripsArray = Utility.readFromText(fileNameText);
            for (int i = 0; i < tripsArray.Count; i++ )
            {   
                try
                {
                    tripsFromTextFile.Add(Utility.stringParser((string)tripsArray[i]));
                }
                catch (TripRecordIsNotReadException e)
                {
                    Console.WriteLine("Trip record is not read." + "\nException details:" + "\n" + e.Message + "\nObject = " + e.Source + "\nMethod - " + e.TargetSite + "\nCall stack - " + e.StackTrace);
                }                
            }
            Console.WriteLine("Trips from txt file:");
            Utility.displayOrders(tripsFromTextFile);


            //read & write from binary file
            Utility.writeToBinary(fileNameBin, catalog);
            tripsFromBinFile = Utility.readFromBinary(fileNameBin);
            Console.WriteLine("Trips from dat file:");
            Utility.displayOrders(tripsFromBinFile);

            Utility.SerializeToXml(fileNameXml, catalog);
            tripsFromXmlFile = Utility.DeserializeFromXml(fileNameXml);
            Console.WriteLine("Trips from XML file:");
            Utility.displayOrders(tripsFromXmlFile);
                        
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.AppSettings["cnStr"];
            cn.Open();
            
            MyDatabase.readFromDB(cn);

            int numberOfAffectedRows;
            string command = "DELETE FROM dbo.Products WHERE UnitPrice = (SELECT MAX(UnitPrice) FROM dbo.Products)";
            numberOfAffectedRows = MyDatabase.executeNonQuery(cn, command);
            Console.WriteLine("Number of affected rows: {0}", numberOfAffectedRows);

            command = "UPDATE dbo.Shippers SET CompanyName = 'test' WHERE ShipperID = 1";
            numberOfAffectedRows = MyDatabase.executeNonQuery(cn, command);
            Console.WriteLine("Number of affected rows: {0}", numberOfAffectedRows);

            command = "INSERT INTO dbo.Region ( RegionID, RegionDescription ) VALUES ( 6, N'RegionFromProgram')";
            numberOfAffectedRows = MyDatabase.executeNonQuery(cn, command);
            Console.WriteLine("Number of affected rows: {0}", numberOfAffectedRows);

            string categoryName = "Beverages";
            string ordYear = "1996";
            MyDatabase.executeSalesByCategoryProc(cn, categoryName, ordYear);

            Console.ReadLine();
        }
    }
}
