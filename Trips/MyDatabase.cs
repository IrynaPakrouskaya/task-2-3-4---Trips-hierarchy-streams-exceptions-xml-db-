using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trips
{
    public static class MyDatabase
    {
        public static void readFromDB(SqlConnection cn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = "Select * from Orders Where EmployeeID = 2";
            var orders = new DataTable();
            using (var dr = cmd.ExecuteReader())
            {
                orders.Load(dr);
            }
            foreach (DataRow row in orders.Rows)
            {
                Console.WriteLine(row["OrderID"] + " " + row["CustomerID"] + " " + row["ShipCountry"]);
            }            
        }
        public static int executeNonQuery(SqlConnection cn, string command)
        {
            int numberOfAffectedRows = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = command;
            try
            {
                numberOfAffectedRows = cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);                
            }  
            return numberOfAffectedRows;
        }
        public static void executeSalesByCategoryProc(SqlConnection cn, string categoryName, string ordYear)
        {
            var SalesByCategory = new DataTable();            
            using(SqlCommand cmd = new SqlCommand("SalesByCategory", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter CategoryName = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 15);
                CategoryName.Value = categoryName;
                cmd.Parameters.Add(CategoryName);

                SqlParameter OrdYear = new SqlParameter("@OrdYear", SqlDbType.NVarChar, 4);
                OrdYear.Value = ordYear;
                cmd.Parameters.Add(OrdYear);

                var dr = cmd.ExecuteReader();
                SalesByCategory.Load(dr);
            }      
            foreach (DataRow row in SalesByCategory.Rows)
            {
                Console.WriteLine("Product Name = {0}, Purchase = {1}", row["ProductName"], row["TotalPurchase"]);
            }            
        }
    }
}
